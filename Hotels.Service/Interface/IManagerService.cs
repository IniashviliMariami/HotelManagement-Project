using Hotels.Models.Dto.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Service.Interface
{
    public interface IManagerService
    {
        Task AddManagerAsync(ManagerForCreateDto managerDto);
        Task UpdateManagerAsync(ManagerForUpdateDto managerDto);
        Task DeleteManagerAsync(int managerId);
      
        Task<List<ManagerForGettingDto>> GetAllManagersAsync();
        Task SaveManager();
    }
}
