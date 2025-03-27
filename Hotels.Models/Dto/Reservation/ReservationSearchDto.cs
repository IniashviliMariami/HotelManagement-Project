using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Models.Dto.Reservation
{
    public class ReservationSearchDto
    {
        public int? HotelId { get; set; }
        public int? GuestId { get; set; }
        public int? RoomId { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
    }
}
