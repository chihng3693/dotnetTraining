using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceConsoleApp.Models;

//immutable
public record Category
(long CategoryId, string? CategoryName,DateTime CreatedDate, string? CreatedBy);
   
    ////initialized at constructor level
    //public long CategoryId { get; init; }
    //public string? CategoryName { get; init; }
    ////nullability test
    //public DateTime? CreatedDate { get; init; }
    //public DateTime? ModifiedDate { get; set;}
    //public string? CreatedBy { get; init; }
    //public string? ModifiedBy { get; set; }
    //public bool? IsActive { get; set; }

//}
