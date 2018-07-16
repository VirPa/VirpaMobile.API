using System.Collections.Generic;

namespace Virpa.Mobile.DAL.v1.Model {

    public class CustomResponse<T> {

        public CustomResponse() {
            Message = new List<string>();
        }

        public bool Succeed { get; set; }

        public T Data { get; set; }

        public List<string> Message { get; set; }
    }
}