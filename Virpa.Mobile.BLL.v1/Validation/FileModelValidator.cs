using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Virpa.Mobile.BLL.v1.Helpers;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.Validation {

    public class FileModelValidator : AbstractValidator<FileModel> {
        public FileModelValidator() {

            RuleFor(a => a.Email).EmailAddress().WithMessage(ResponseBadRequest.ErrorInvalidEmailFormat.ToString());

            RuleFor(a => a.Files).Must(IsFileValid)
                .WithMessage(ResponseBadRequest.ErrImageFormat.ToString());

            RuleFor(a => a.Files).Must(NotExceedTheFileSizeLimit)
                .WithMessage(ResponseBadRequest.ErrFileTooLarge.ToString());
        }

        #region Local Methods

        private static bool IsFileValid(ICollection<IFormFile> files) {

            const string pattern = @"\.(?i)(jpe?g|png|gif|pdf|sketch|psd)$";

            var isValidFile = false;

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

            foreach (var file in files) {
                totalFileSize += file.Length;
            }

            return totalFileSize <= fileSizeLimit;
        }

        #endregion
    }
}
