using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace ECommerceCom.WepApi.Filters
{
    public class TimeControllerFilter : ActionFilterAttribute
    {
        public string StartTime { get; set; } // Start time
        public string EndTime { get; set; } // End time

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Get the current UTC time
            var utcNow = DateTime.UtcNow;

            // Log the current UTC time
            Console.WriteLine($"UTC Time: {utcNow}");

            // Set the start and end times in UTC
            StartTime = "01:00 AM";  // Start time: 1:00 AM UTC
            EndTime = "02:00 AM";    // End time: 2:00 AM UTC

            // Convert the start and end times to DateTime objects
            var startTime = DateTime.ParseExact(StartTime, "hh:mm tt", null).TimeOfDay;
            var endTime = DateTime.ParseExact(EndTime, "hh:mm tt", null).TimeOfDay;

            // Log the start and end times
            Console.WriteLine($"Start Time (UTC): {startTime}, End Time (UTC): {endTime}");

            // Compare the current UTC time against the start and end times
            if (utcNow.TimeOfDay >= startTime && utcNow.TimeOfDay < endTime)
            {
                // If the request is within the restricted time range, return 403 Forbidden
                context.Result = new ContentResult
                {
                    Content = "Requests cannot be made to this endpoint during this time frame.",
                    StatusCode = 403
                };
            }
            else
            {
                // If the request is outside the restricted time range, allow the request to proceed
                base.OnActionExecuting(context);
            }
        }
    }
}
