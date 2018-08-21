using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Virpa.Mobile.BLL.v1.OtherServices.Interface;
using Virpa.Mobile.BLL.v1.Repositories.Interface;
using Virpa.Mobile.DAL.v1.Entities.Mobile;
using Virpa.Mobile.DAL.v1.Identity;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.Repositories {
    public class MyUser : IMyUser {

        #region Initialization

        private readonly List<string> _infos = new List<string>();

        private readonly IOptions<Manifest> _options;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly VirpaMobileContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;

        #endregion

        #region Constructor

        public MyUser(IOptions<Manifest> options
            , IEmailSender emailSender
            , IMapper mapper
            , UserManager<ApplicationUser> userManager
            , SignInManager<ApplicationUser> signInManager
            , VirpaMobileContext context) {

            _options = options;
            _emailSender = emailSender;
            _mapper = mapper;

            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        #endregion

        #region Get Users

        public async Task<CustomResponse<UserResponse>> GetUser(GetUserModel model) {

            return await Task.Run(() => {

                var user = _context.AspNetUsers.FirstOrDefault(u => u.Id == model.UserId);

                #region Validate User

                if (user == null) {
                    _infos.Add("User not exist.");

                    return new CustomResponse<UserResponse> {
                        Message = _infos
                    };
                };

                #endregion

                return new CustomResponse<UserResponse> {
                    Succeed = true,
                    Data = new UserResponse {
                        User = _mapper.Map<UserDetails>(user)
                    }
                };
            });
        }

        #endregion

        #region Post Users

        public async Task<CustomResponse<UserResponse>> CreateUser(CreateUserModel model) {

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null) {

                _infos.Add("Email address already exist.");

                return new CustomResponse<UserResponse> {
                    Data = _mapper.Map<UserResponse>(user),
                    Message = _infos
                };
            }

            model.UserName = model.UserName ?? model.Email;

            var newUser = _mapper.Map<ApplicationUser>(model);

            var newUserCreated = await _userManager.CreateAsync(newUser, model.Password);

            if (!newUserCreated.Succeeded) _infos.Add(string.Join("xx | xx", newUserCreated.Errors));

            return new CustomResponse<UserResponse> {
                Succeed = newUserCreated.Succeeded,
                Data = GetUser(new GetUserModel {
                    UserId = newUser.Id
                }).Result.Data
            };
        }

        public async Task<CustomResponse<UserResponse>> UpdateUser(UpdateUserModel model) {

            var user = _context.AspNetUsers.FirstOrDefault(u => u.Id == model.UserId);

            #region Validate User

            if (user == null) {
                _infos.Add("User not exist.");

                return new CustomResponse<UserResponse> {
                    Message = _infos
                };
            };

            #endregion

            user.Fullname = string.IsNullOrEmpty(model.Fullname) ? user.Fullname : model.Fullname;
            user.MobileNumber = string.IsNullOrEmpty(model.MobileNumber) ? user.MobileNumber : model.MobileNumber;
            user.UpdatedAt = DateTime.UtcNow;
            user.BackgroundSummary = model.BackgroundSummary;

            _context.Update(user);

            await _context.SaveChangesAsync();

            return new CustomResponse<UserResponse> {
                Succeed = true,
                Data = GetUser(new GetUserModel {
                    UserId = user.Id
                }).Result.Data
            };
        }

        public async Task<CustomResponse<ConfirmEmailModel>> SendEmailConfirmation(SendEmailConfirmation model) {

            var user = await _userManager.FindByEmailAsync(model.Email);

            #region Validate User

            if (user == null) {
                _infos.Add("User not exist.");

                return new CustomResponse<ConfirmEmailModel> {
                    Message = _infos
                };
            };

            #endregion

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            if (_options.Value.SendEmail) {

                await _emailSender.SendEmailWithTemplateAsync(SendEmailWrapper());
            }

            return new CustomResponse<ConfirmEmailModel> {
                Succeed = true,
                Data = new ConfirmEmailModel {
                    UserId = user.Id,
                    Token = token
                }
            };

            #region Local Methods

            SendEmailWithTemplateModel SendEmailWrapper() {

                return new SendEmailWithTemplateModel {
                    Recipient = model.Email,
                    FullName = user.Fullname,
                    Subject = "Email Confirmation",
                    Template = "Confirm-Email-Template",
                    Link = model.Uri + $"/user/confirm-email?id={user.Id}&token={token}&mode=confirm"
                };
            }

            #endregion
        }

        public async Task<CustomResponse<string>> ConfirmEmail(ConfirmEmailModel model) {

            var user = await _userManager.FindByIdAsync(model.UserId);

            #region Validate User

            if (user == null) {
                _infos.Add("User not exist.");

                return new CustomResponse<string> {
                    Message = _infos
                };
            };

            #endregion

            var result = await _userManager.ConfirmEmailAsync(user, model.Token);

            if (!result.Succeeded) _infos.Add("Failed to confirm email.");

            return new CustomResponse<string> {
                Succeed = result.Succeeded,
                Data = null,
                Message = _infos
            };
        }

        public async Task<CustomResponse<string>> ChangePassword(ChangePasswordModel model) {

            var user = await _userManager.FindByEmailAsync(model.Email);

            #region Validate User

            if (user == null) {
                _infos.Add("User not exist.");

                return new CustomResponse<string> {
                    Message = _infos
                };
            };

            #endregion

            var isPasswordGood = await _userManager.CheckPasswordAsync(user, model.OldPassword);

            if (!isPasswordGood) _infos.Add("You entered wrong Old password.");

            return new CustomResponse<string> {
                Succeed = isPasswordGood,
                Data = null,
                Message = _infos
            };
        }

        public async Task<CustomResponse<ForgotPasswordResponseModel>> ForgotPassword(ForgotPasswordModel model) {

            var user = await _userManager.FindByEmailAsync(model.Email);

            #region Validate User

            if (user == null) {
                _infos.Add("User not exist.");

                return new CustomResponse<ForgotPasswordResponseModel> {
                    Message = _infos
                };
            };

            #endregion

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            if (_options.Value.SendEmail) {

                await _emailSender.SendEmailWithTemplateAsync(SendEmailWrapper());
            }

            return new CustomResponse<ForgotPasswordResponseModel> {
                Succeed = true,
                Data = new ForgotPasswordResponseModel {
                    UserId = user.Id,
                    Token = token
                }
            };

            #region Local Methods

            SendEmailWithTemplateModel SendEmailWrapper() {

                return new SendEmailWithTemplateModel {
                    Recipient = model.Email,
                    FullName = user.Fullname,
                    Subject = "Reset Password",
                    Template = "Forgot-Password-Template",
                    Link = model.Uri + $"/user/reset-password?id={user.Id}&token={token}&mode=forgot"
                };
            }

            #endregion
        }

        public async Task<CustomResponse<string>> ResetPassword(ResetPasswordModel model) {

            var user = await _userManager.FindByIdAsync(model.UserId);

            #region Validate User

            if (user == null) {
                _infos.Add("User not exist.");

                return new CustomResponse<string> {
                    Message = _infos
                };
            };

            #endregion

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            if (!result.Succeeded) _infos.Add("Failed to reset password.");

            return new CustomResponse<string> {
                Succeed = result.Succeeded,
                Data = null,
                Message = _infos
            };
        }

        #endregion
    }
}
