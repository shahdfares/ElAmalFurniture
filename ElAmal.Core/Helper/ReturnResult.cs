using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElAmal.Core.Helper
{
    public class ReturnResult<T>
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public Dictionary<string, string> Errors { get; set; } = new();
        public T? Data { get; set; }

       public static ReturnResult<T> Success(T data, string? message = null)
        {
            return new ReturnResult<T>
            {
                IsSuccess = true,
                Message = message,
                Data =  data
            };
        }


        public static ReturnResult<T> Fail(string? message = null, Dictionary<string, string>? errors = null)
        {
            return new ReturnResult<T>
            {
                IsSuccess = false,
                Message = message,
                Errors = errors ?? new()
            };
        }
    
    }
}
