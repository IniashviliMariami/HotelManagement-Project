using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Models.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAvailable { get; set; }
        public decimal Price { get; set; }

        public int HotelId { get; set; }
        public Hotel Hotel { get; set; } // 1:M კავშირი

        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
