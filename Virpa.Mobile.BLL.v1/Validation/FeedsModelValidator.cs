using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Virpa.Mobile.BLL.v1.Helpers;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.Validation {

    public class FeedsModelValidator : AbstractValidator<PostMyFeedModel> {
        public FeedsModelValidator() {

            RuleFor(a => a.Budget).GreaterThanOrEqualTo(0).WithMessage(ResponseBadRequest.ErrNegativeNumber.ToString());

            RuleFor(a => a.ExpiredOn).GreaterThanOrEqualTo(0).WithMessage(ResponseBadRequest.ErrNegativeNumber.ToString());

            RuleFor(a => a.ExpiredOn).LessThanOrEqualTo(30).WithMessage(ResponseBadRequest.ErrNegativeNumber.ToString());

            RuleFor(a => a.Type).GreaterThanOrEqualTo(0).WithMessage(ResponseBadRequest.ErrNegativeNumber.ToString());

            RuleFor(a => a.Body).NotNull().WithMessage(ResponseBadRequest.ErrFieldEmpty.ToString());
        }
    }

    public class FeedCoverPhotoModelValidator : AbstractValidator<UpdateCoverPhotoModel> {
        public FeedCoverPhotoModelValidator() {

            RuleFor(a => a.Id).NotNull().WithMessage(ResponseBadRequest.ErrFieldEmpty.ToString());

            RuleFor(a => a.CoverPhoto).NotNull().WithMessage(ResponseBadRequest.ErrFieldEmpty.ToString());

            RuleFor(a => a.CoverPhoto).Must(IsFileValid)
                .WithMessage(ResponseBadRequest.ErrImageFormat.ToString());

            RuleFor(a => a.CoverPhoto).Must(NotExceedTheFileSizeLimit)
                .WithMessage(ResponseBadRequest.ErrFileTooLarge.ToString());

            RuleFor(a => a.CoverPhoto).Must(NotExceedTheFileCountLimit)
                .WithMessage(ResponseBadRequest.ErrAcceptsOnlyOneFile.ToString());
        }

        #region Local Methods
        
        private static bool NotExceedTheFileCountLimit(ICollection<IFormFile> files) {

            if (files == null) {
                return true;
            }

            return !(files.Count > 1);
        }

        private static bool IsFileValid(ICollection<IFormFile> files) {

            const string pattern = @"\.(?i)(jpe?g|png|gif|pdf|sketch|psd)$";

            var isValidFile = false;

            if (files == null) {
                return true;
            }

            foreach (var file in files) {
                isValidFile = Regex.Match(file.FileName, pattern).Success;
                if (!isValidFile) {
                    break;
                }
            }
            return isValidFile;
        }

        private static bool NotExceedTheFileSizeLimit(ICollection<IFormFile> files) {
            //bytes
            const double fileSizeLimit = 2e+7;
            double totalFileSize = 0;

            if (files == null) {
                return true;
            }

            foreach (var file in files) {
                totalFileSize += file.Length;
            }

            return totalFileSize <= fileSizeLimit;
        }

        #endregion
    }
}
