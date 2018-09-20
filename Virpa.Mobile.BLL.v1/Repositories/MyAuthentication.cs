using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Virpa.Mobile.BLL.v1.OtherServices.Interface;
using Virpa.Mobile.BLL.v1.Repositories.Interface;
using Virpa.Mobile.DAL.v1.Entities.Mobile;
using Virpa.Mobile.DAL.v1.Identity;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.Repositories {
    internal class MyAuthentication : IMyAuthentication {

        #region Initialization

        private readonly List<string> _infos = new List<string>();

        private readonly IOptions<Manifest> _options;
        private readonly IMapper _mapper;
        private readonly IProcessRefreshToken _refreshToken;
        private readonly IMyUser _user;

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly VirpaMobileContext _context;

        #endregion

        #region Constructor

        public MyAuthentication(IOptions<Manifest> options,
            IMapper mapper,
            IProcessRefreshToken refreshToken,
            IMyUser user,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            VirpaMobileContext context) {

            _options = options;
            _mapper = mapper;
            _refreshToken = refreshToken;
            _user = user;

            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        #endregion

        public async Task<CustomResponse<SignInReponseModel>> SignInTheReturnUser(SignInModel model) {

            var signedIn = await _signInManager.PasswordSignInAsync(
                  model.Email, 
                  model.Password,
                  model.RememberMe,
                  lockoutOnFailure: false);

            #region Validate LogIn

            if (!signedIn.Succeeded) {

                _infos.Add("You have entered an invalid email or password.");

                return new CustomResponse<SignInReponseModel> {
                    Message = _infos
                };
            }

            #endregion

            var user = await _userManager.FindByEmailAsync(model.Email);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var sessionToken = new JwtSecurityToken(_options.Value.Issuer,
                _options.Value.Issuer,
                claims,
                expires: DateTime.UtcNow.AddMinutes(_options.Value.AccessTokenExpiryMins),
                signingCredentials: creds);
            
            var refreshedToken = _refreshToken.GenerateRefreshToken(new RefreshTokenGetModel {
                UserId = user.Id,
                ApiVersion = model.ApiVersion,
                AppVersion = model.AppVersion,
                UserAgent = model.UserAgent
            });

            #region Validate Refresh Token

            if (!refreshedToken.Result.Succeed) {
                return new CustomResponse<SignInReponseModel> {
                    Message = refreshedToken.Result.Message
                };
            }

            #endregion

            var accessToken = new JwtSecurityTokenHandler().WriteToken(sessionToken);
            
            return new CustomResponse<SignInReponseModel> {
                Succeed = true,
                Data = new SignInReponseModel {
                    Authorization = new SignInReturnToken {
                        SessionToken = new TokenResource {
                            Token = accessToken,
                            ExpiredAt = sessionToken.ValidTo
                        },
                        RefreshToken = new TokenResource {
                            Token = refreshedToken.Result.Data.Token,
                            ExpiredAt = refreshedToken.Result.Data.ExpiredAt
                        }
                    },
                    User = new UserResponse {
                        Detail = _mapper.Map<UserDetails>(user),
                        ProfilePicture = _user.GetProfilePicture(user.Id)
                    }
                }
            };
        }

        public async Task<CustomResponse<string>> SignOut(SignOutModel model) {

            var user = await _userManager.FindByEmailAsync(model.Email);

            var userSession = _context.AspNetUserSessions.FirstOrDefault(t => t.UserId == user.Id && t.DeviceName == model.UserAgent);

            #region Validate userSession

            if (userSession == null) {
                _infos.Add("User don't have registered session");

                return new CustomResponse<string> {
                    Message = _infos
                };
            }

            #endregion
            
            userSession.Validity = false;

            _context.Update(userSession);

            await _context.SaveChangesAsync();

            await _signInManager.SignOutAsync();

            return new CustomResponse<string> {
                Succeed = true
            };
        }

        public async Task<CustomResponse<GenerateTokenResponseModel>> GenerateToken(GenerateTokenModel model) {

            var user = await _userManager.FindByEmailAsync(model.UserName);

            #region Validate User

            if (user == null) {

                _infos.Add("Username/email not exist.");

                return new CustomResponse<GenerateTokenResponseModel> {
                    Message = _infos
                };
            }
            #endregion

            var userToken = _context.AspNetUserSessions.FirstOrDefault(t => t.UserId == user.Id && t.Token == model.TokenResource.Token);

            #region Validate Token if Registered

            if (userToken == null) {
                _infos.Add("The account never Signed In or the Refresh Token is wrong.");

                return new CustomResponse<GenerateTokenResponseModel> {
                    Message = _infos
                };
            }

            #endregion

            #region Validate Token Type

            if (model.TokenResource.Type != "refresh" && model.TokenResource.Type != "session") _infos.Add("The Token Type is invalid.");

            #endregion

            #region Check Refresh Token Validity

            if (!(userToken.Validity ?? true)) _infos.Add("The Refresh Token is invalid.");

            if (userToken.TokenExpiredAt < DateTime.UtcNow && model.TokenResource.Type == "session") _infos.Add("The Refresh Token is expired.");
            
            if (_infos.Count > 0) {
                return new CustomResponse<GenerateTokenResponseModel> {
                    Message = _infos
                };
            }

            #endregion

            var refreshedToken = RefreshToken(model.TokenResource);

            return refreshedToken;

            #region Local Methods

            CustomResponse<GenerateTokenResponseModel> RefreshToken(GenerateTokenResourceModel tokenResourceModel) {

                if (tokenResourceModel.Type == "session") {
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Key));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(_options.Value.Issuer,
                        _options.Value.Issuer,
                        claims,
                        expires: DateTime.Now.AddMinutes(_options.Value.AccessTokenExpiryMins),
                        signingCredentials: creds);

                    return new CustomResponse<GenerateTokenResponseModel> {
                        Succeed = true,
                        Data = new GenerateTokenResponseModel {
                            Authorization = new TokenResource {
                                Token = new JwtSecurityTokenHandler().WriteToken(token),
                                ExpiredAt = token.ValidTo
                            },
                            User = _user.GetUser(new GetUserModel {
                                    UserId = user.Id
                                }).Result.Data
                        }
                    };
                }

                var refreshToken = _refreshToken.GenerateRefreshToken(new RefreshTokenGetModel {
                    UserId = user.Id,
                    UserAgent = model.UserAgent,
                    AppVersion = model.AppVersion,
                    ApiVersion = "1.0",
                    Token = tokenResourceModel.Token
                });

                return new CustomResponse<GenerateTokenResponseModel> {
                    Succeed = refreshToken.Result.Succeed,
                    Data = new GenerateTokenResponseModel {
                        Authorization = refreshToken.Result.Data,
                        User = _user.GetUser(new GetUserModel {
                                UserId = user.Id
                            }).Result.Data
                        },
                    Message = refreshToken.Result.Message
                };
            }

            #endregion
        }
    }
}