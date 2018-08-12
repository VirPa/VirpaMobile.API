namespace Virpa.Mobile.BLL.v1.Helpers {

    public class ResponseBadRequest {
        private const int CImageFormat = 1;
        private const int CFileEmpty = 2;
        private const int CFieldEmpty = 3;
        private const int CInvalidEmailFormat = 4;
        private const int CInvalidType = 5;
        private const int CFileTooLarge = 6;
        private const int CNegativeNumber = 7;
        private const int CNumberTooLarge = 8;
        private const int CAcceptsOnlyOneFile = 9;

        public static int ErrImageFormat => CImageFormat;
        public static int ErrFileEmpty => CFileEmpty;
        public static int ErrFieldEmpty => CFieldEmpty;
        public static int ErrorInvalidEmailFormat => CInvalidEmailFormat;
        public static int ErrorInvalidType => CInvalidType;
        public static int ErrFileTooLarge => CFileTooLarge;
        public static int ErrNegativeNumber => CNegativeNumber;
        public static int ErrNumberTooLarge => CNumberTooLarge;
        public static int ErrAcceptsOnlyOneFile => CAcceptsOnlyOneFile;

        public Error ShowError(int errorCode, int maxValue = 0) {

            return GetCorrespondingError(errorCode, maxValue);
        }

        private Error GetCorrespondingError(int errorCode, int maxValue = 0) {
            //Generic
            
            if (errorCode == ErrImageFormat) {
                return new Error {
                     Message = "Sketch, Psd, Pdf, Png, Jpeg and Gif only. File format not supported."
                };
            }

            if (errorCode == ErrFileEmpty) {
                return new Error {
                     Message = "File is empty. Kindly upload atleast one(1) attachment file."
                };
            }

            if (errorCode == ErrFieldEmpty) {
                return new Error {
                     Message = "Required field is empty."
                };
            }

            if (errorCode == ErrorInvalidEmailFormat) {
                return new Error {
                     Message = "Email format is invalid."
                };
            }

            if (errorCode == ErrorInvalidType) {
                return new Error {
                    Message = "Invalid token resource type."
                };
            }

            if (errorCode == ErrFileTooLarge) {
                return new Error {
                    Message = "File is too large. Limit of 20Mb only."
                };
            }

            if (errorCode == ErrNegativeNumber) {
                return new Error {
                    Message = "Numeric must greater than zero."
                };
            }

            if (errorCode == ErrNumberTooLarge) {
                return new Error {
                    Message = $"Numeric must less than {maxValue}."
                };
            }

            if (errorCode == ErrAcceptsOnlyOneFile) {
                return new Error {
                    Message = $"System accepts only one(1) file."
                };
            }

            return null;
        }
        public class Error {
            public string Message { get; set; }
        }
    }
}