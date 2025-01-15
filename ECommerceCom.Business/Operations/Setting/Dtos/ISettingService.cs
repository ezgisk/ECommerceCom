using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceCom.Business.Operations.Setting.Dtos
{
    public interface ISettingService
    {
        Task ToggleMaintenence();
    }
}
