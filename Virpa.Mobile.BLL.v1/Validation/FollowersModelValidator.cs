using FluentValidation;
using Virpa.Mobile.BLL.v1.Helpers;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.Validation {

    public class FollowersModelValidator : AbstractValidator<PostMyFollowerModel> {
        public FollowersModelValidator() {

            RuleFor(a => a.FollowedId).NotNull().WithMessage(ResponseBadRequest.ErrFieldEmpty.ToString());
        }
    }
}
