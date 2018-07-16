namespace Virpa.Mobile.BLL.v1.Helpers {

    public class ResponseBadRequest {
        private const int CImageFormat = 1;
        private const int CFileEmpty = 2;
        private const int CTokenMissing = 3;
        private const int CAntiForgeryTokenExpiry = 4;
        private const int CAntiForgeryRelated = 5;
        private const int CFieldEmpty = 6;
        private const int CInvalidEmailFormat = 7;
        private const int CInvalidType = 8;

        public int ErrImageFormat => CImageFormat;
        public int ErrFileEmpty => CFileEmpty;
        public int ErrTokenMissing => CTokenMissing;
        public int ErrAntiForgeryTokenExpiry => CAntiForgeryTokenExpiry;
        public int ErrAntiForgeryRelated => CAntiForgeryRelated;
        public int ErrFieldEmpty => CFieldEmpty;
        public int ErrorInvalidEmailFormat => CInvalidEmailFormat;
        public int ErrorInvalidType => CInvalidType;

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
                     Message = "File is empty. Kindly upload atleast one(1) document file."
                };
            }

            if (errorCode == ErrTokenMissing) {
                return new Error {
                     Message = "X-XSRF-TOKEN is missing. Request headers must contain the Antiforgery token."
                };
            }

            if (errorCode == ErrAntiForgeryTokenExpiry) {
                return new Error {
                     Message = "The Antiforgery token is no longer valid since it had expired."
                };
            }

            if (errorCode == ErrAntiForgeryRelated) {
                return new Error {
                     Message = "Antiforgery Related Error."
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

            return null;
        }
        public class Error {
            public string Message { get; set; }
        }
    }
}