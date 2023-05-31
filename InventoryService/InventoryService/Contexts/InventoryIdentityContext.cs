using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Contexts
{
    public class InventoryIdentityContext:IdentityDbContext<IdentityUser>
    {
        public InventoryIdentityContext(DbContextOptions<InventoryIdentityContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
