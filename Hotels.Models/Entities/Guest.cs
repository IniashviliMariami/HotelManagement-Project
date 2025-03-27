using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Models.Entities
{
    public class Guest
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }


        public string PersonalNumber { get; set; }


        public string PhoneNumber { get; set; }
        
        public List<GuestReservation> GuestReservations { get; set; } = new List<GuestReservation>();
    }
}
