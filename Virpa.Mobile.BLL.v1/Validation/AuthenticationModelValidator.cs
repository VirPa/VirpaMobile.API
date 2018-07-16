﻿using FluentValidation;
using Virpa.Mobile.BLL.v1.Helpers;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.Validation {

    public class SignInModelValidator : AbstractValidator<SignInModel> {
        public SignInModelValidator(ResponseBadRequest badRequest) {

            RuleFor(a => a.Email).EmailAddress().WithMessage(badRequest.ErrorInvalidEmailFormat.ToString());

            RuleFor(a => a.Password).NotEmpty().WithMessage(badRequest.ErrFieldEmpty.ToString());
        }
    }

    public class SignOutModelValidator : AbstractValidator<SignOutModel> {
        public SignOutModelValidator(ResponseBadRequest badRequest) {

            RuleFor(a => a.Email).EmailAddress().WithMessage(badRequest.ErrorInvalidEmailFormat.ToString());
        }
    }

    public class GenerateTokenModelValidator : AbstractValidator<GenerateTokenModel> {
        public GenerateTokenModelValidator(ResponseBadRequest badRequest) {

            RuleFor(a => a.UserName).EmailAddress().WithMessage(badRequest.ErrorInvalidEmailFormat.ToString());

            RuleFor(a => a.TokenResource.Token).NotEmpty().WithMessage(badRequest.ErrFieldEmpty.ToString());

            RuleFor(a => a.TokenResource.Type).NotEmpty().WithMessage(badRequest.ErrFieldEmpty.ToString());

            RuleFor(a => a.TokenResource.Type).Must(TypeValid).WithMessage(badRequest.ErrorInvalidType.ToString());
        }

        private static bool TypeValid(string type) {

            return type == "session" || type == "refresh";
        }
    }
}