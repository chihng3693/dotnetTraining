using GraphQL.Types;
using InventoryService.Mutations;
using InventoryService.Queries;

namespace InventoryService.Schemas
{
    public class CategorySchema:Schema
    {
        public CategorySchema(IServiceProvider ServiceProvider)
        {
            Query = ServiceProvider.GetRequiredService<RootQuery>();
            Mutation = ServiceProvider.GetRequiredService<RootMutation>();
           

        }
    }
}
