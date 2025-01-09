using ECommerceCom.Business.DataProtection;
using ECommerceCom.Business.Operations.User.Dtos;
using ECommerceCom.Business.Types;
using ECommerceCom.Data.Entities;
using ECommerceCom.Data.Repositories;
using ECommerceCom.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceCom.Business.Operations.User
{
    public class UserManager : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<UserEntity> _userRepository;
        private readonly IDataProtection _protector;
        public UserManager(IUnitOfWork unitOfWork, IRepository<UserEntity> UserRepository, IDataProtection protector)
        {
            _unitOfWork = unitOfWork;
            _userRepository = UserRepository;
            _protector = protector;
        }
        public async Task<ServiceMessage> AddUser(AddUserDto user)
        {
            var hasMail = _userRepository.GetAll(x=>x.Email.ToLower() == user.Email.ToLower());

            if (hasMail.Any()) 
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Email adresi zaten mevcut"
                };
            }
            var userEntity = new UserEntity()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = _protector.Protect(user.Password),

            };
            _userRepository.Add(userEntity);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Kullanici kaydi sirasinda bir hata olustu: {ex.Message}", ex);
            }
            return new ServiceMessage 
            {
                IsSucceed = true 
            };

        }
    }
}
