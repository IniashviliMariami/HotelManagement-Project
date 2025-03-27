using Hotels.Models.Dto.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Service.Interface
{
    public interface IRoomService
    {
        Task AddRoomAsync(RoomForCreateDto roomDto);
        Task UpdateRoomAsync(RoomForUpdateDto roomDto);
        Task DeleteRoomAsync(int roomId);
        Task<List<RoomForGettingDto>> GetAllRoomsAsync(RoomForFilterDto filter);
        Task<RoomForGettingDto> GetRoomByIdAsync(int roomId);

        Task SaveRoom();
    }
}
