using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class ApiException : Exception
    {
        [NotNull]
        private string errorMessage;
        public HttpStatusCode StatusCode { get; set; }
        public override string Message { get { return errorMessage; } }
        public ApiException(HttpStatusCode httpStatusCode, string errorMsg)
        {
            errorMessage = errorMsg;
            StatusCode = httpStatusCode;
        }
    }
}
