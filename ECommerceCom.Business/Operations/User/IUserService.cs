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
    }
}
