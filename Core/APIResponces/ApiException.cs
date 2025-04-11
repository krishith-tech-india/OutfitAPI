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
        public HttpStatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public ApiException(HttpStatusCode httpStatusCode, string errorMessage)
        {
            ErrorMessage = errorMessage;
            StatusCode = httpStatusCode;
        }
    }
}
