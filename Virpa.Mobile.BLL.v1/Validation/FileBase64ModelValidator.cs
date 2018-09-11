using FluentValidation;
using Virpa.Mobile.BLL.v1.Helpers;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.Validation {

    public class FileBase64ModelValidator : AbstractValidator<FileBase64Model> {
        public FileBase64ModelValidator() {

            RuleFor(a => a.Email).EmailAddress().WithMessage(ResponseBadRequest.ErrorInvalidEmailFormat.ToString());
            
        }
    }
}
