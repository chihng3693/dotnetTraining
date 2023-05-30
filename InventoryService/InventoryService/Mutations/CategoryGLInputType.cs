using GraphQL.Language.AST;
using GraphQL.Types;
using InventoryService.Models;

namespace InventoryService.Mutations
{
    public class CategoryGLInputType: InputObjectGraphType<Category>
    {
        public CategoryGLInputType()
        {
            Name = "CategoryInput";
            Field<NonNullGraphType< StringGraphType >> ("CategoryName");
            Field<DateGraphType>("CreatedDate");
            Field<DateGraphType>("LastUpdatedDate");
            Field<NonNullGraphType<StringGraphType>>("CreatedBy");
            Field<NonNullGraphType<StringGraphType>>("LastUpdatedBy");

        }
    }
}
