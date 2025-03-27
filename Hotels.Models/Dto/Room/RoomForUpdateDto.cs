using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Models.Dto.Room
{
    public class RoomForUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public bool IsAvailable { get; set; }

        [Range(1, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
