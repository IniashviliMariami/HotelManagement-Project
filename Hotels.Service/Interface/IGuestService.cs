using Hotels.Models.Dto.Guest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Service.Interface
{
    public interface IGuestService
    {
        Task AddGuestAsync(GuestForCreateDto guestDto);
        Task UpdateGuestAsync(GuestForUpdateDto guestDto);
        Task DeleteGuestAsync(int guestId);
        Task<List<GuestForGettingDto>> GetAllGuestsAsync();
        Task<GuestForGettingDto> GetGuestAsync(int guestId);
        Task SaveGuest();
    }
}
