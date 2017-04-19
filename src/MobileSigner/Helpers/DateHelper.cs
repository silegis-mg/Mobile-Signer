using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almg.MobileSigner.Helpers
{
    public class DateHelper
    {
        public static string DateStr(DateTime date)
        {
            var now = DateTime.Now;
            if(now.Date == date.Date)
            {
                //same day
                return date.ToString("hh:mm");
            } else
            {
                if (now.Year != date.Year)
                {
                    return date.ToString("dd/MM/yyyy");
                } else
                {
                    return date.ToString("dd/MM");
                }  
            }
        }

        public static string ReadableDate(DateTime date)
        {
            string dateStr = "";

            var now = DateTime.Now;
            if (date.Kind == DateTimeKind.Utc)
            {
                now = now.ToUniversalTime();
            }
            
            var value = now - date;
            
            if (value.TotalDays > 30)
            {
                if (now.Year > date.Year)
                {
                    dateStr = date.ToString("dd/MM/yyyy");
                }
                else
                {
                    dateStr = date.ToString("dd/MM");
                }
                return dateStr;
            }
            else if (value.TotalDays >= 1)
            {
                var totalDays = (int)value.TotalDays;
                dateStr = totalDays + " dia" + (totalDays > 1 ? "s" : string.Empty);
            }
            else if (value.TotalHours >= 1)
            {
                var totalHours = (int)value.TotalHours;
                dateStr += totalHours + " hora" + (totalHours > 1 ? "s" : string.Empty);
            }
            else if (value.TotalMinutes >= 1)
            {
                var totalMinutes = (int)value.TotalMinutes;
                dateStr += totalMinutes + " minuto" + (totalMinutes > 1 ? "s" : string.Empty);
            }
            else if (value.TotalSeconds >= 1)
            {
                var totalSeconds = (int)value.TotalSeconds;
                dateStr += totalSeconds + " segundo" + (totalSeconds > 1 ? "s" : string.Empty);
            }

            if (!string.IsNullOrEmpty(dateStr)) { 
                return dateStr + " atrás";
            }
            else
            {
                return dateStr;
            }
        }
    }
}
