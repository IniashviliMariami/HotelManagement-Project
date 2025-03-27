using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Models.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public bool IsActive { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; } // 1:M კავშირი

        public List<GuestReservation> GuestReservations { get; set; } = new List<GuestReservation>(); // M:N კავშირი
      //  public int GuestId { get; set; }//დავამატე
    }
}
