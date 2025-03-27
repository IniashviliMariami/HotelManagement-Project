using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Models.Dto.Hotel
{
    public class HotelForFilterDto
    {
        public string Country { get; set; }
        public string City { get; set; }
        public int Rating { get; set; }
    }
}
