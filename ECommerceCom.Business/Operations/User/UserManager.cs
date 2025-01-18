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
                    Message = "The email address is already in use"
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
                throw new Exception($"An error occurred while registering the user: {ex.Message}", ex);
            }
            return new ServiceMessage
            {
                IsSucceed = true
            };
        }

        public async Task<ServiceMessage> AdjustUserEmail(int id, string changeTo)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "User with the given ID not found."
                };
            user.Email = changeTo;
            _userRepository.Update(user);
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while changing the email: {ex.Message}", ex);
            }

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Email successfully changed."
            };
        }

        public async Task<ServiceMessage> DeleteUser(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "User with the given ID not found."
                };
            }
            _userRepository.Delete(id);
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the user: {ex.Message}", ex);
            }

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "User successfully deleted."
            };
        }

        public async Task<UserInfoDto> GetUser(int id)
        {
            var userEntity = _userRepository.Get(x => x.Id == id);

            if (userEntity == null)
            {
                throw new Exception("User not found");
            }

            var userDto = new UserInfoDto
            {
                Id = userEntity.Id,
                Email = userEntity.Email,
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                Role = userEntity.Role
            };

            return userDto;
        }

        public async Task<List<UserInfoDto>> GetUsers()
        {
            var userEntities = _userRepository.GetAll();

            var userDtos = userEntities.Select(user => new UserInfoDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role
            }).ToList();

            return userDtos;
        }

        public ServiceMessage<UserInfoDto> LoginUser(LoginUserDto user)
        {
            var userEntity = _userRepository.Get(x => x.Email.ToLower() == user.Email.ToLower());
            if (userEntity == null)
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceed = false,
                    Message = "Username or password is incorrect."
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
                    Message = "Username or password is incorrect."
                };
            }
        }

        public async Task<ServiceMessage> UpdateUser(UpdateUserDto user)
        {
            var userEntity = _userRepository.GetById(user.Id);
            if (userEntity == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "User not found."
                };
            }

            await _unitOfWork.BeginTransaction();
            try
            {
                if (!string.IsNullOrEmpty(user.FirstName))
                {
                    userEntity.FirstName = user.FirstName;
                }

                if (!string.IsNullOrEmpty(user.LastName))
                {
                    userEntity.LastName = user.LastName;
                }

                if (!string.IsNullOrEmpty(user.Email))
                {
                    var existingEmailUser = _userRepository.GetAll(x => x.Email.ToLower() == user.Email.ToLower() && x.Id != user.Id).FirstOrDefault();
                    if (existingEmailUser != null)
                    {
                        return new ServiceMessage
                        {
                            IsSucceed = false,
                            Message = "This email address is already used by another user."
                        };
                    }

                    userEntity.Email = user.Email;
                }

                if (user.Role != null)
                {
                    userEntity.Role = user.Role;
                }

                _userRepository.Update(userEntity);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransaction();

                return new ServiceMessage
                {
                    IsSucceed = true,
                    Message = "User successfully updated."
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackTransaction();
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = $"An error occurred while updating the user: {ex.Message}"
                };
            }
        }
    }
}
