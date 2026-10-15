using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Controllers
{
    [Route("api/dashboard/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
    }
}
