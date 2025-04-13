using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Core
{
    public class ApiResponse : ActionResult
    {
        [NotNull]
        public HttpStatusCode StatusCode { get; set; }
        public dynamic? Data { get; set; }
        public ApiResponse(HttpStatusCode httpStatusCode, dynamic? data = null)
        {
            Data = data;
            StatusCode = httpStatusCode;
        }
    }
}
