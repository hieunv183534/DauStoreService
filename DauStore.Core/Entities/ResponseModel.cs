using System;
using System.Collections.Generic;
using System.Text;

namespace DauStore.Core.Entities
{
    public class ResponseModel
    {

        public ResponseModel(int code, string message)
        {
            this.Code = code;
            this.Message = message;
        }

        public ResponseModel(int code, string message, object data)
        {
            this.Code = code;
            this.Message = message;
            this.Data = data;
        }

        public int Code { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }

    }
}
