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
        
        private readonly IOptions<Manifest> _options;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly VirpaMobileContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;

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

        public async Task<CustomResponse<UserResponse>> CreateUser(CreateUserModel model) {

            var infos = new List<string>();

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null) {

                infos.Add("Email address already exist.");

                var checkedPassword = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                if (!checkedPassword.Succeeded) infos.Add("Failed to LogIn.");
                   
                return new CustomResponse<UserResponse> {
                    Succeed = checkedPassword.Succeeded,
                    Data = _mapper.Map<UserResponse>(user),
                    Message = infos
                };
            }

            model.UserName = model.UserName ?? model.Email;

            var newUser = _mapper.Map<ApplicationUser>(model);

            var newUserCreated = await _userManager.CreateAsync(newUser, model.Password);

            if (!newUserCreated.Succeeded) infos.Add(string.Join("xx | xx", newUserCreated.Errors));

            return new CustomResponse<UserResponse> {
                Succeed = newUserCreated.Succeeded,
                Data = _mapper.Map<UserResponse>(newUser),
                Message = infos
            };
        }

        public async Task<CustomResponse<UserResponse>> UpdateUser(UpdateUserModel model) {

            var infos = new List<string>();

            var user = _context.AspNetUsers.FirstOrDefault(u => u.Id == model.UserId);

            #region Validate User

            if (user == null) {
                infos.Add("User not exist.");

                return new CustomResponse<UserResponse> {
                    Message = infos
                };
            };

            #endregion
            
            user.Fullname = string.IsNullOrEmpty(model.Fullname) ? user.Fullname : model.Fullname;
            user.MobileNumber = string.IsNullOrEmpty(model.MobileNumber) ? user.MobileNumber : model.MobileNumber;
            user.UpdatedAt = DateTime.UtcNow;

            _context.Update(user);

            await _context.SaveChangesAsync();

            return new CustomResponse<UserResponse> {
                Succeed = true,
                Data = _mapper.Map<UserResponse>(user)
            };
        }

        public async Task<CustomResponse<UserResponse>> GetUser(GetUserModel model) {

            var infos = new List<string>();

            return await Task.Run(() => {

                var user = _context.AspNetUsers.FirstOrDefault(u => u.Id == model.UserId);

                #region Validate User

                if (user == null) {
                    infos.Add("User not exist.");

                    return new CustomResponse<UserResponse> {
                        Message = infos
                    };
                };

                #endregion

                return new CustomResponse<UserResponse> {
                    Succeed = true,
                    Data = _mapper.Map<UserResponse>(user)
                };
            });
        }

        public async Task<CustomResponse<ConfirmEmailModel>> SendEmailConfirmation(SendEmailConfirmation model) {

            var infos = new List<string>();

            var user = await _userManager.FindByEmailAsync(model.Email);

            #region Validate User

            if (user == null) {
                infos.Add("User not exist.");

                return new CustomResponse<ConfirmEmailModel> {
                    Message = infos
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

            var infos = new List<string>();

            var user = await _userManager.FindByIdAsync(model.UserId);

            #region Validate User

            if (user == null) {
                infos.Add("User not exist.");

                return new CustomResponse<string> {
                    Message = infos
                };
            };

            #endregion

            var result = await _userManager.ConfirmEmailAsync(user, model.Token);

            if (!result.Succeeded) infos.Add("Failed to confirm email.");

            return new CustomResponse<string> {
                Succeed = result.Succeeded,
                Data = null,
                Message = infos
            };
        }

        public async Task<CustomResponse<string>> ChangePassword(ChangePasswordModel model) {

            var infos = new List<string>();

            var user = await _userManager.FindByEmailAsync(model.Email);

            #region Validate User

            if (user == null) {
                infos.Add("User not exist.");

                return new CustomResponse<string> {
                    Message = infos
                };
            };

            #endregion

            var isPasswordGood = await _userManager.CheckPasswordAsync(user, model.OldPassword);

            if (!isPasswordGood) infos.Add("You entered wrong Old password.");

            return new CustomResponse<string> {
                Succeed = isPasswordGood,
                Data = null,
                Message = infos
            };
        }

        public async Task<CustomResponse<ForgotPasswordResponseModel>> ForgotPassword(ForgotPasswordModel model) {

            var infos = new List<string>();

            var user = await _userManager.FindByEmailAsync(model.Email);

            #region Validate User

            if (user == null) {
                infos.Add("User not exist.");

                return new CustomResponse<ForgotPasswordResponseModel> {
                    Message = infos
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

            var infos = new List<string>();

            var user = await _userManager.FindByIdAsync(model.UserId);

            #region Validate User

            if (user == null) {
                infos.Add("User not exist.");

                return new CustomResponse<string> {
                    Message = infos
                };
            };

            #endregion

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            if (!result.Succeeded) infos.Add("Failed to reset password.");

            return new CustomResponse<string> {
                Succeed = result.Succeeded,
                Data = null,
                Message = infos
            };
        }
    }
}
