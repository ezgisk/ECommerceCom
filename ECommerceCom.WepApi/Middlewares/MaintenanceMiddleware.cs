using ECommerceCom.Business.Operations.Setting.Dtos;

namespace ECommerceCom.WepApi.Middlewares
{
    public class MaintenanceMiddleware
    {
        private readonly RequestDelegate _next;
  
        public MaintenanceMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            var _settingService = context.RequestServices.GetRequiredService<ISettingService>();
            bool maintenanceMode = _settingService.GetMaintanenceState();
            if(context.Request.Path.StartsWithSegments("/api/auth/login") || context.Request.Path.StartsWithSegments("/api/settings"))
            {
                await _next(context);
                return;
            }
            if (maintenanceMode)
            {
                await context.Response.WriteAsync("Su anda hizmet verememekteyiz..");
            }
            else 
            {
                await _next(context);
            }

        }
    }
}
