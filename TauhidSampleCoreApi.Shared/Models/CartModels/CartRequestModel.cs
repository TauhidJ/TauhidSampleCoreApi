using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TauhidSampleCoreApi.Shared.Models.CartModels
{
    public class CartRequestModel
    {
        [Required(ErrorMessage = "Product id is required.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        public int Quantity { get; set; }
    }
}
