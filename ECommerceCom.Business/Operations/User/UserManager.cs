using ECommerceCom.Business.DataProtection;
using ECommerceCom.Business.Operations.User.Dtos;
using ECommerceCom.Business.Types;
using ECommerceCom.Data.Entities;
using ECommerceCom.Data.Enums;
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
            var hasMail = _userRepository.GetAll(x => x.Email.ToLower() == user.Email.ToLower());

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
                Role = UserRole.Customer

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

        public ServiceMessage<UserInfoDto> LoginUser(LoginUserDto user)
        {
            var userEntity = _userRepository.Get(x => x.Email.ToLower() == user.Email.ToLower());
            if (userEntity == null)
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceed = false,
                    Message = "Kullanici adi ve sifre hatali."

                };
            }
            var unprotectedText = _protector.UnProtect(userEntity.Password);
            if (unprotectedText == user.Password)
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceed = true,
                    Data = new UserInfoDto
                    {
                        Email = userEntity.Email,
                        FirstName = userEntity.FirstName,
                        LastName = userEntity.LastName,
                        Role = userEntity.Role,
                    }
                };
            }
            else
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceed = false,
                    Message = "Kullanici adi ve sifre hatali."

                };
            }
            
        }
    }
}
