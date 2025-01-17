using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace ECommerceCom.WepApi.Filters
{
    public class TimeControllerFilter : ActionFilterAttribute
    {
        public string StartTime { get; set; } // Başlangıç saati
        public string EndTime { get; set; } // Bitiş saati

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Şu anki UTC zamanı alıyoruz
            var utcNow = DateTime.UtcNow;

            // Loglama ile şu anki UTC zamanı
            Console.WriteLine($"UTC Time: {utcNow}");

            // StartTime ve EndTime saatlerini doğru formatta ayarlıyoruz
            // UTC zaman diliminde geçerli bir format belirliyoruz
            StartTime = "01:00 PM";  // Örnek başlangıç saati
            EndTime = "02:00 PM";    // Örnek bitiş saati

            // StartTime ve EndTime saatlerini DateTime.ParseExact ile doğru formatta dönüştürüyoruz
            var startTime = DateTime.ParseExact(StartTime, "hh:mm tt", null);
            var endTime = DateTime.ParseExact(EndTime, "hh:mm tt", null);

            // Loglama ile saatleri görelim
            Console.WriteLine($"Start Time (UTC): {startTime}, End Time (UTC): {endTime}");

            // UTC zamanını saat diliminden bağımsız olarak karşılaştırıyoruz
            if (utcNow.TimeOfDay >= startTime.TimeOfDay && utcNow.TimeOfDay <= endTime.TimeOfDay)
            {
                // Saat dilimi arasında istek kabul ediliyor
                base.OnActionExecuting(context);
            }
            else
            {
                // Saat dilimi dışındaysa 403 hata kodu döndürüyoruz
                context.Result = new ContentResult
                {
                    Content = "Bu saatler arasında endpoint'e istek atılamaz.",
                    StatusCode = 403
                };
            }
        }
    }
}
