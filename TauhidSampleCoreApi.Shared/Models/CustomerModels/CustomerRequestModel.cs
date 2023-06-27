using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zero.SharedKernel.Constants;

namespace TauhidSampleCoreApi.Shared.Models.CustomerModels
{
    public class CustomerRequestModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Mobile number is required.")]
        [RegularExpression(RegexPatterns.MobileNumber, ErrorMessage = "Mobile number is invalid.")]
        public string? MobileNumber { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [RegularExpression(RegexPatterns.EmailAddress, ErrorMessage = "Email address is invalid.")]
        public string? EmailAddress { get; set; }
    }
}
