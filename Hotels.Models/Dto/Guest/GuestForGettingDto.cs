using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Models.Dto.Guest
{
    public class GuestForGettingDto
    {
        [Required]
        public int Id { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Personal number must be exactly 11 characters.")]
        [RegularExpression("^[0-9]{11}$", ErrorMessage = "Personal number must contain only digits.")]
        public string PersonalNumber { get; set; }
    }
}
