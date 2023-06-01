using InventoryService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class LdapController : ControllerBase
    {
        private ILdapService ldapService;
        public LdapController(ILdapService ldapService)
        {
            this.ldapService = ldapService;
        }
        [HttpGet("search")]
        public IActionResult Search(string input)
        {

            var user = this.ldapService.SearchUser(input);
            return new ObjectResult(user);
        }
    }
}
