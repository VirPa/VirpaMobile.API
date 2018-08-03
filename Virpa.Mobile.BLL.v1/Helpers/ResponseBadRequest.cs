namespace Virpa.Mobile.BLL.v1.Helpers {

    public class ResponseBadRequest {
        private const int CImageFormat = 1;
        private const int CFileEmpty = 2;
        private const int CFieldEmpty = 3;
        private const int CInvalidEmailFormat = 4;
        private const int CInvalidType = 5;
        private const int CFileTooLarge = 6;

        public static int ErrImageFormat => CImageFormat;
        public static int ErrFileEmpty => CFileEmpty;
        public static int ErrFieldEmpty => CFieldEmpty;
        public static int ErrorInvalidEmailFormat => CInvalidEmailFormat;
        public static int ErrorInvalidType => CInvalidType;
        public static int ErrFileTooLarge => CFileTooLarge;

        public Error ShowError(int errorCode) {

            return GetCorrespondingError(errorCode);
        }

        private Error GetCorrespondingError(int errorCode) {
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

            return null;
        }
        public class Error {
            public string Message { get; set; }
        }
    }
}