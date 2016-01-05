using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace E5R.Product.WebSite.Controllers
{
    [Route("account")]
    public class Account : Controller
    {
        [HttpGet("login")]
        public async Task<IActionResult> SignIn()
        {
            return Ok("Página de login");
        }
    }
}