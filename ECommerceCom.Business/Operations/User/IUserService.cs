using ECommerceCom.Business.Operations.Order;
using ECommerceCom.Business.Operations.Order.Dtos;
using ECommerceCom.Business.Operations.User.Dtos;
using ECommerceCom.Business.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceCom.Business.Operations.User
{
    public interface IUserService
    {
        Task<ServiceMessage> AddUser(AddUserDto user);
        ServiceMessage<UserInfoDto> LoginUser(LoginUserDto user);
        Task<UserInfoDto> GetUser(int id);
        Task<List<UserInfoDto>> GetUsers();
        Task<ServiceMessage> DeleteUser(int id);
        Task<ServiceMessage> AdjustUserEmail(int id, string changeTo);
        Task<ServiceMessage> UpdateUser(UpdateUserDto user);
    }
}
