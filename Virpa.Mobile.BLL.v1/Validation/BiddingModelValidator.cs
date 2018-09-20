using FluentValidation;
using Virpa.Mobile.BLL.v1.Helpers;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.Validation {

    public class BiddingModelValidator : AbstractValidator<PostBidderModel> {

        public BiddingModelValidator() {

            RuleFor(a => a.FeedId).NotNull().WithMessage(ResponseBadRequest.ErrFieldEmpty.ToString());
        }
    }
}
