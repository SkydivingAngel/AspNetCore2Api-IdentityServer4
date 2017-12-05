using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServerClient.Controllers
{
    [Route("identity")]
    [Authorize]
    public class IdentityController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            //return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
            return new JsonResult("Suka!");
        }
    }
}
