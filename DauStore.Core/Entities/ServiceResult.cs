using System;
using System.Collections.Generic;
using System.Text;

namespace DauStore.Core.Entities
{
    public class ServiceResult
    {
        public ResponseModel Response { get; set; }

        public int StatusCode { get; set; }
    }
}
