using System.Text.Json;

namespace InventoryService.Exceptions
{
    public class ErrorResponse
    {
      
            public bool Success { get; set; }
            public string? Message { get; set; }
        
    }
}
