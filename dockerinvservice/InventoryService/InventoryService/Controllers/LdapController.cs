using InventoryService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
            try
            {
                var user = this.ldapService.SearchUser(input);
                if (user == null)
                {

                    throw new KeyNotFoundException("Users not found. Try again");
                }
                return new ObjectResult(user);
            }
            catch(Exception ex)
            {
               return new ObjectResult(ex);
            }
          
        }
    }
}
