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

        public async Task<ServiceMessage> AdjustUserEmail(int id, string changeTo)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Bu id ile eslesen user bulanamadi."
                };
            user.Email = changeTo;
            _userRepository.Update(user);
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Email degistirilirken bir hata oluştu: {ex.Message}", ex);
            }

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Email başarıyla degistirildi."
            };
        }

        public async Task<ServiceMessage> DeleteUser(int id)
        {
            var order = _userRepository.GetById(id);
            if (order == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Bu id ile eslesen user bulanamadi."
                };
            }
            _userRepository.Delete(id);
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"User silinirken bir hata oluştu: {ex.Message}", ex);
            }

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "User başarıyla silindi."
            };
        }

        public async Task<UserInfoDto> GetUser(int id)
        {
            // Kullanıcıyı ID'ye göre veritabanından al
            var userEntity = _userRepository.Get(x => x.Id == id); // Asenkron çağrı ile kullanıcıyı bul

            // Eğer kullanıcı bulunamazsa, null dönülecek
            if (userEntity == null)
            {
                throw new Exception("Kullanıcı bulunamadı");
            }

            // UserEntity'yi UserInfoDto'ya dönüştür
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
            // Tüm kullanıcıları veritabanından al
            var userEntities = _userRepository.GetAll(); // Asenkron çağrı

            // UserEntity listesini UserInfoDto listesine dönüştür
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

        public async Task<ServiceMessage> UpdateUser(UpdateUserDto user)
        {
            var userEntity = _userRepository.GetById(user.Id);
            if (userEntity == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "User bulunamadı."
                };
            }

            await _unitOfWork.BeginTransaction();
            try
            {
                // Kullanıcı bilgilerini güncelleyerek işlemi başlatıyoruz
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
                    // Email adresini kontrol et (eğer varsa başka bir kullanıcıya ait olmasın)
                    var existingEmailUser = _userRepository.GetAll(x => x.Email.ToLower() == user.Email.ToLower() && x.Id != user.Id).FirstOrDefault();
                    if (existingEmailUser != null)
                    {
                        return new ServiceMessage
                        {
                            IsSucceed = false,
                            Message = "Bu email adresi zaten başka bir kullanıcı tarafından kullanılmaktadır."
                        };
                    }

                    userEntity.Email = user.Email;
                }

                if (user.Role != null)
                {
                    userEntity.Role = user.Role;
                }

                // Kullanıcıyı güncelliyoruz
                _userRepository.Update(userEntity);

                // Tüm değişiklikleri kaydediyoruz
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransaction();

                return new ServiceMessage
                {
                    IsSucceed = true,
                    Message = "Kullanıcı başarıyla güncellendi."
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackTransaction();
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = $"Kullanıcı güncellenirken bir hata oluştu: {ex.Message}"
                };
            }
        }
    }
}
