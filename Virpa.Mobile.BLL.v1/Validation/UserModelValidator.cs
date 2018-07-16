using FluentValidation;
using Virpa.Mobile.BLL.v1.Helpers;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.Validation {

    public class UserModelValidator : AbstractValidator<CreateUserModel> {
        protected UserModelValidator(ResponseBadRequest badRequest) {

            RuleFor(a => a.Fullname).NotEmpty().WithMessage(badRequest.ErrFieldEmpty.ToString());

            RuleFor(a => a.Email).EmailAddress().WithMessage(badRequest.ErrorInvalidEmailFormat.ToString());

            RuleFor(a => a.Password).NotEmpty().WithMessage(badRequest.ErrFieldEmpty.ToString());
        }
    }

    public class UpdateUserModelValidator : AbstractValidator<UpdateUserModel> {
        protected UpdateUserModelValidator(ResponseBadRequest badRequest) {

            RuleFor(a => a.UserId).NotEmpty().WithMessage(badRequest.ErrFieldEmpty.ToString());
        }
    }

    public class SendEmailConfirmationValidator : AbstractValidator<SendEmailConfirmation> {
        protected SendEmailConfirmationValidator(ResponseBadRequest badRequest) {

            RuleFor(a => a.Email).EmailAddress().WithMessage(badRequest.ErrorInvalidEmailFormat.ToString());
        }
    }

    public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailModel> {
        protected ConfirmEmailValidator(ResponseBadRequest badRequest) {

            RuleFor(a => a.UserId).NotEmpty().WithMessage(badRequest.ErrFieldEmpty.ToString());

            RuleFor(a => a.Token).NotEmpty().WithMessage(badRequest.ErrFieldEmpty.ToString());
        }
    }

    public class ChangePasswordValidator : AbstractValidator<ChangePasswordModel> {
        protected ChangePasswordValidator(ResponseBadRequest badRequest) {

            RuleFor(a => a.Email).EmailAddress().WithMessage(badRequest.ErrorInvalidEmailFormat.ToString());

            RuleFor(a => a.OldPassword).NotEmpty().WithMessage(badRequest.ErrFieldEmpty.ToString());

            RuleFor(a => a.NewPassword).NotEmpty().WithMessage(badRequest.ErrFieldEmpty.ToString());
        }
    }

    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordModel> {
        protected ForgotPasswordValidator(ResponseBadRequest badRequest) {

            RuleFor(a => a.Email).EmailAddress().WithMessage(badRequest.ErrorInvalidEmailFormat.ToString());
        }
    }

    public class ResetPasswordValidator : AbstractValidator<ResetPasswordModel> {
        protected ResetPasswordValidator(ResponseBadRequest badRequest) {

            RuleFor(a => a.UserId).NotEmpty().WithMessage(badRequest.ErrFieldEmpty.ToString());

            RuleFor(a => a.Token).NotEmpty().WithMessage(badRequest.ErrFieldEmpty.ToString());

            RuleFor(a => a.NewPassword).NotEmpty().WithMessage(badRequest.ErrFieldEmpty.ToString());
        }
    }
}
