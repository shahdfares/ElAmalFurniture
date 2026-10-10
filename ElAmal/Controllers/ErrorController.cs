using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElAmal.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("error/{code}")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        public ActionResult Error(int code)
        {
            return code switch
            {
                400 => BadRequest("Bad Request"),
                401 => Unauthorized("Unauthorized"),
                404 => NotFound("Not Found"),
                500 => StatusCode(500, "Internal Server Error"),
                _ => StatusCode(code, "UnExpected Error")
            };
        }
    }
}
