using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Virpa.Mobile.BLL.v1.Helpers;
using Virpa.Mobile.BLL.v1.Repositories.Interface;
using Virpa.Mobile.BLL.v1.Validation;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.API.v1.Controllers {

    [AllowAnonymous]
    [Route("User")]
    [ApiVersion("1.0")]
    public class UserController : BaseController {

        #region Initialization

        private readonly List<string> _infos = new List<string>();

        private readonly IMyUser _user;
        private readonly IMyFiles _myFiles;

        private readonly ResponseBadRequest _badRequest;
        private readonly UserModelValidator _userModelValidator;
        private readonly UpdateUserModelValidator _updateUserModelValidator;
        private readonly SendEmailConfirmationValidator _sendEmailConfirmationValidator;
        private readonly ConfirmEmailValidator _confirmEmailValidator;
        private readonly ChangePasswordValidator _changePasswordValidator;
        private readonly ForgotPasswordValidator _forgotPasswordValidator;
        private readonly ResetPasswordValidator _resetPasswordValidator;
        private readonly FileModelValidator _fileModelValidator;

        #endregion

        #region Constructor

        public UserController(IMyUser user,
            IMyFiles myFiles,
            UserModelValidator userModelValidator,
            UpdateUserModelValidator updateUserModelValidator,
            SendEmailConfirmationValidator sendEmailConfirmationValidator,
            ConfirmEmailValidator confirmEmailValidator,
            ChangePasswordValidator changePasswordValidator,
            ForgotPasswordValidator forgotPasswordValidator,
            ResetPasswordValidator resetPasswordValidator,
            FileModelValidator fileModelValidator,
            ResponseBadRequest badRequest) {

            _user = user;
            _myFiles = myFiles;

            _badRequest = badRequest;
            _userModelValidator = userModelValidator;
            _updateUserModelValidator = updateUserModelValidator;
            _sendEmailConfirmationValidator = sendEmailConfirmationValidator;
            _confirmEmailValidator = confirmEmailValidator;
            _changePasswordValidator = changePasswordValidator;
            _forgotPasswordValidator = forgotPasswordValidator;
            _resetPasswordValidator = resetPasswordValidator;
            _fileModelValidator = fileModelValidator;
        }

        #endregion

        #region Get Users

        [HttpGet("{userid}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(GetUserModel model) {

            var gotUser = await _user.GetUser(model);

            return Ok(gotUser);
        }

        #endregion

        #region Post Users

        [HttpPost("Create", Name = "CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserModel model) {

            #region Validate Model

            var userInputValidated = _userModelValidator.Validate(model);

            if (!userInputValidated.IsValid) {
                _infos.Add(_badRequest.ShowError(int.Parse(userInputValidated.Errors[0].ErrorMessage)).Message);

                return BadRequest(new CustomResponse<string> {
                    Message = _infos
                });
            }

            #endregion

            var createdUser = await _user.CreateUser(model);

            return Ok(createdUser);
        }

        [HttpPost("Update", Name = "UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserModel model) {

            #region Validate Model

            var userInputValidated = _updateUserModelValidator.Validate(model);

            if (!userInputValidated.IsValid) {
                _infos.Add(_badRequest.ShowError(int.Parse(userInputValidated.Errors[0].ErrorMessage)).Message);

                return BadRequest(new CustomResponse<string> {
                    Message = _infos
                });
            }

            #endregion

            var updatedUser = await _user.UpdateUser(model);

            return Ok(updatedUser);
        }

        [HttpPost("Send-Email-Confirmation", Name = "SendEmailConfirmation")]
        public async Task<IActionResult> GenerateEmailConfirmation([FromBody] SendEmailConfirmation model) {

            #region Validate Model

            var userInputValidated = _sendEmailConfirmationValidator.Validate(model);

            if (!userInputValidated.IsValid) {
                _infos.Add(_badRequest.ShowError(int.Parse(userInputValidated.Errors[0].ErrorMessage)).Message);

                return BadRequest(new CustomResponse<string> {
                    Message = _infos
                });
            }

            #endregion

            var sentEmailConfirmation = await _user.SendEmailConfirmation(model);

            return Ok(sentEmailConfirmation);
        }

        [HttpPost("Confirm-Email", Name = "ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailModel model) {

            #region Validate Model

            var userInputValidated = _confirmEmailValidator.Validate(model);

            if (!userInputValidated.IsValid) {
                _infos.Add(_badRequest.ShowError(int.Parse(userInputValidated.Errors[0].ErrorMessage)).Message);

                return BadRequest(new CustomResponse<string> {
                    Message = _infos
                });
            }

            #endregion

            var confirmedEmail = await _user.ConfirmEmail(model);

            return Ok(confirmedEmail);
        }

        [HttpPost("Change-Password", Name = "ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model) {

            #region Validate Model

            var userInputValidated = _changePasswordValidator.Validate(model);

            if (!userInputValidated.IsValid) {
                _infos.Add(_badRequest.ShowError(int.Parse(userInputValidated.Errors[0].ErrorMessage)).Message);

                return BadRequest(new CustomResponse<string> {
                    Message = _infos
                });
            }

            #endregion

            var changedPassword = await _user.ChangePassword(model);

            return Ok(changedPassword);
        }

        [HttpPost("Forgot-Password", Name = "ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model) {

            #region Validate Model

            var userInputValidated = _forgotPasswordValidator.Validate(model);

            if (!userInputValidated.IsValid) {
                _infos.Add(_badRequest.ShowError(int.Parse(userInputValidated.Errors[0].ErrorMessage)).Message);

                return BadRequest(new CustomResponse<string> {
                    Message = _infos
                });
            }

            #endregion

            var sentResetPassword = await _user.ForgotPassword(model);

            return Ok(sentResetPassword);
        }

        [HttpPost("Reset-Password", Name = "ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model) {

            #region Validate Model

            var userInputValidated = _resetPasswordValidator.Validate(model);

            if (!userInputValidated.IsValid) {
                _infos.Add(_badRequest.ShowError(int.Parse(userInputValidated.Errors[0].ErrorMessage)).Message);

                return BadRequest(new CustomResponse<string> {
                    Message = _infos
                });
            }

            #endregion

            var resetPassword = await _user.ResetPassword(model);

            return Ok(resetPassword);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("ProfilePicture/Change", Name = "ChangeProfilePicture")]
        public async Task<IActionResult> ChangeProfilePicture(PostFilesModel postModel) {

            var model = new FileModel {
                Files = postModel.Files,
                Email = UserEmail,
                FileId = postModel.FileId,
                Type = 3
            };

            #region Validate Model

            if (model.Files == null) {

                _infos.Add(_badRequest.ShowError(ResponseBadRequest.ErrFileEmpty).Message);

                return BadRequest(new CustomResponse<string> {
                    Message = _infos
                });
            }

            var userInputValidated = _fileModelValidator.Validate(model);

            if (!userInputValidated.IsValid) {
                _infos.Add(_badRequest.ShowError(int.Parse(userInputValidated.Errors[0].ErrorMessage)).Message);

                return BadRequest(new CustomResponse<string> {
                    Message = _infos
                });
            }

            #endregion

            var savedFiles = await _myFiles.PostFiles(model);

            return Ok(savedFiles);
        }

        #endregion
    }
}
