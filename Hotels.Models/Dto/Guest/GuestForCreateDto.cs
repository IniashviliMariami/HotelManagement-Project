using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Models.Dto.Guest
{
    public class GuestForCreateDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Personal number must be 11 characters.")]
        public string PersonalNumber { get; set; } 

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
