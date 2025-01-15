using ECommerceCom.Business.Operations.Setting.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace ECommerceCom.WepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingService _settingService;
        public SettingsController(ISettingService settingService)
        {
            _settingService = settingService;
        }
        [HttpPatch]
        public async Task<IActionResult> ToggleMaintenence()
        {
            await _settingService.ToggleMaintenence();
            return Ok();
        }
    }
}
