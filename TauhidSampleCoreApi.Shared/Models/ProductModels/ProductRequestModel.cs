using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TauhidSampleCoreApi.Shared.Models.ProductModels
{
    public class ProductRequestModel
    {
        [Required(ErrorMessage = "Product name is required.")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        public int? Quantity { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        public int? Price { get; set; }
    }
}
