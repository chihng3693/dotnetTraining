using InventoryService.Models;

namespace InventoryService.Repositories
{
    public interface ILdapService
    {
        User SearchUser(string userId);
    }
}
