using FluentValidation;
using Virpa.Mobile.BLL.v1.Helpers;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.Validation {

    public class LocationModelValidator : AbstractValidator<PinLocationModel> {

        public LocationModelValidator() {

            RuleFor(a => a.Latitude).NotNull().WithMessage(ResponseBadRequest.ErrFieldEmpty.ToString());

            RuleFor(a => a.Longitude).NotNull().WithMessage(ResponseBadRequest.ErrFieldEmpty.ToString());
        }
    }
}
