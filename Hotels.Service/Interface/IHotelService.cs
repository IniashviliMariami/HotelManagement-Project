using Hotels.Models.Dto.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Service.Interface
{
    public interface IHotelService
    {
        Task AddHotelAsync(HotelForCreateDto hotelDto);
        Task UpdateHotelAsync(HotelForUpdateingDto hotelDto);
        Task DeleteHotelAsync(int hotelId);
        Task<HotelForGettingDto> GetHotelAsync(int hotelId);
        Task<List<HotelForGettingDto>> GetAllHotelsAsync(string country, string city, int? rating);
        Task SaveHotel();
       
    }
}
