using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Models.Entities
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }

        public int ManagerId { get; set; }
        public Manager Manager { get; set; }

        public List<Room> Rooms { get; set; } = new List<Room>();
    }
}
