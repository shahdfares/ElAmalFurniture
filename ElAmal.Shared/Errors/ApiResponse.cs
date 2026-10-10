using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElAmal.Shared.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public ApiResponse(int statuseCode, string? message = null) 
        {
            StatusCode = statuseCode;
            Message = message?? GetMessageFromStatuseCode(statuseCode);
        }

        private string? GetMessageFromStatuseCode(int statuseCode)
        {
            return statuseCode switch
            {
                400 => "BadRequest",
                404 => "NotFound",
                401 => "UnAuthorized",
                500 => "Internal Server Error",
                _ => null
            };
        }
    }
}
