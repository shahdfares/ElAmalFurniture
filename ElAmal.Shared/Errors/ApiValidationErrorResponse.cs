using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElAmal.Shared.Errors
{
    public class ApiValidationErrorResponse : ApiResponse
    {
        public Dictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();

        public ApiValidationErrorResponse(): base(400)  
        {
            
            
        }
    }
}
