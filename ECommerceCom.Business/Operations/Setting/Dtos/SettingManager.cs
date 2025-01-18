using ECommerceCom.Data.Entities;
using ECommerceCom.Data.Migrations;
using ECommerceCom.Data.Repositories;
using ECommerceCom.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceCom.Business.Operations.Setting.Dtos
{
    public class SettingManager : ISettingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<SettingEntity>_settingRepository;
        public SettingManager(IUnitOfWork unitOfWork, IRepository<SettingEntity> settingRepository)
        {
            _settingRepository = settingRepository;
            _unitOfWork = unitOfWork;
        }

        public bool GetMaintanenceState()
        {
            var maintenanceState = _settingRepository.GetById(1).MaintanenceMode;
            return maintenanceState;
        }

        public async Task ToggleMaintenence()
        {
            var setting = _settingRepository.GetById(1);
            setting.MaintanenceMode = !setting.MaintanenceMode;
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception) 
            {
                throw new Exception("An error occurred while performing maintenance");
            }
        }
    }
}
