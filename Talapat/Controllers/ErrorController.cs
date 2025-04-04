using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talapat.Errors;

namespace Talapat.Controllers
{
    [Route("errors/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        public IActionResult Error(int code)
        {
            return NotFound(new ApiResponse(code));

        }
        // unauthorized
        [Route("unauthorized")]
        public IActionResult Unauthorized()
        {
            return Unauthorized(new ApiResponse(401));
        }
    }
}
