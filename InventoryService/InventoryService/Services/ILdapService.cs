using InventoryService.Models;

namespace InventoryService.Services
{
    public interface ILdapService
    {
        User SearchUser(string userId);
    }
}
