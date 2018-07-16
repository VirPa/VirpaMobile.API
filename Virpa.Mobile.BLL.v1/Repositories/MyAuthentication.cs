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

        private readonly IOptions<Manifest> _options;
        private readonly IMapper _mapper;
        private readonly IProcessRefreshToken _refreshToken;

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly VirpaMobileContext _context;

        public MyAuthentication(IOptions<Manifest> options,
                            IMapper mapper,
                            IProcessRefreshToken refreshToken,
                            SignInManager<ApplicationUser> signInManager,
                            UserManager<ApplicationUser> userManager,
                            VirpaMobileContext context) {

            _options = options;
            _mapper = mapper;
            _refreshToken = refreshToken;

            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        public async Task<CustomResponse<SignInReponseModel>> SignInTheReturnUser(SignInModel model) {

            var infos = new List<string>();

            var signedIn = await _signInManager.PasswordSignInAsync(
                  model.Email, 
                  model.Password,
                  model.RememberMe,
                  lockoutOnFailure: false);

            #region Validate LogIn

            if (!signedIn.Succeeded) {

                infos.Add("You have entered an invalid email or password.");

                return new CustomResponse<SignInReponseModel> {
                    Message = infos
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
                    User = _mapper.Map<UserResponse>(user)
                }
            };
        }

        public async Task<CustomResponse<string>> SignOut(SignOutModel model) {

            var infos = new List<string>();

            var user = await _userManager.FindByEmailAsync(model.Email);

            var userSession = _context.AspNetUserSessions.FirstOrDefault(t => t.UserId == user.Id && t.DeviceName == model.UserAgent);

            if (userSession == null) {
                infos.Add("User don't have registered session");

                return new CustomResponse<string> {
                    Message = infos
                };
            }

            userSession.Validity = false;

            _context.Update(userSession);

            await _context.SaveChangesAsync();

            await _signInManager.SignOutAsync();

            return new CustomResponse<string> {
                Succeed = true
            };
        }

        public async Task<CustomResponse<TokenResource>> GenerateToken(GenerateTokenModel model) {

            var infos = new List<string>();

            var user = await _userManager.FindByEmailAsync(model.UserName);

            #region Validate User

            if (user == null) {

                infos.Add("Username/email not exist.");

                return new CustomResponse<TokenResource> {
                    Message = infos
                };
            }
            #endregion

            var userToken = _context.AspNetUserSessions.FirstOrDefault(t => t.UserId == user.Id && t.DeviceName == model.UserAgent);

            #region Validate Token if Registered

            if (userToken == null) {
                infos.Add("The account never Signed In.");

                return new CustomResponse<TokenResource> {
                    Message = infos
                };
            }

            #endregion

            #region Validate Token Type

            if (model.TokenResource.Type != "refresh" && model.TokenResource.Type != "session") infos.Add("The Token Type is invalid.");

            #endregion

            #region Check Refresh Token Validity
            if (userToken.Token != model.TokenResource.Token) infos.Add("The Refresh Token is wrong.");

            if (!(userToken.Validity ?? true)) infos.Add("The Refresh Token is invalid.");

            if (userToken.TokenExpiredAt < DateTime.UtcNow && model.TokenResource.Type == "session") infos.Add("The Refresh Token is expired.");
            
            if (infos.Count > 0) {
                return new CustomResponse<TokenResource> {
                    Message = infos
                };
            }

            #endregion

            var refreshedToken = RefreshToken(model.TokenResource);

            return refreshedToken;

            #region Local Methods

            CustomResponse<TokenResource> RefreshToken(GenerateTokenResourceModel tokenResourceModel) {

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
                        expires: DateTime.Now.AddDays(_options.Value.AccessTokenExpiryMins),
                        signingCredentials: creds);

                    return new CustomResponse<TokenResource> {
                        Succeed = true,
                        Data = new TokenResource {
                            Token = new JwtSecurityTokenHandler().WriteToken(token),
                            ExpiredAt = token.ValidTo
                        }
                    };
                }

                var refreshToken = _refreshToken.GenerateRefreshToken(new RefreshTokenGetModel {
                    UserId = user.Id,
                    UserAgent = model.UserAgent,
                    AppVersion = model.AppVersion,
                    ApiVersion = "1.0"
                });

                return new CustomResponse<TokenResource> {
                    Succeed = refreshToken.Result.Succeed,
                    Data = refreshToken.Result.Data,
                    Message = refreshToken.Result.Message
                };
            }

            #endregion
        }
    }
}