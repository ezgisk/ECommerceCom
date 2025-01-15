using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace ECommerceCom.WepApi.Filters
{
    public class TimeControllerFilter : ActionFilterAttribute
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Şu anki UTC zamanı alıyoruz
            var utcNow = DateTime.UtcNow;

            // Amerikan zaman dilimine dönüştürme (Örnek: Eastern Standard Time - EST / UTC-5)
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            var americanTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, timeZoneInfo);

            // Zamanı loglayalım
            Console.WriteLine($"UTC Time: {utcNow}, American Time: {americanTime}");

            // StartTime ve EndTime saatlerini doğru formatta ayarlıyoruz
            StartTime = "10:00 PM";  // Doğru format
            EndTime = "11:59 PM";    // Doğru format

            // StartTime ve EndTime saatlerini DateTime.ParseExact ile doğru formatta dönüştürüyoruz
            var startTime = DateTime.ParseExact(StartTime, "hh:mm tt", null);
            var endTime = DateTime.ParseExact(EndTime, "hh:mm tt", null);

            // Loglama ile saat dilimi farklarını görelim
            Console.WriteLine($"Current Time: {americanTime}, Start Time: {startTime}, End Time: {endTime}");

            // Saatlerin sadece TimeOfDay kısmını karşılaştırıyoruz
            if (americanTime.TimeOfDay >= startTime.TimeOfDay && americanTime.TimeOfDay <= endTime.TimeOfDay)
            {
                base.OnActionExecuting(context);
            }
            else
            {
                context.Result = new ContentResult
                {
                    Content = "Bu saatler arasında end-pointe istek atılamaz.",
                    StatusCode = 403
                };
            }
        }
    }
}
