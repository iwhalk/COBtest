using SharedLibrary.Models;
using System.Globalization;

namespace ReportesObra.Utilities
{
    public class AuxiliaryMethods
    {
        //public Dates ObtenerFechas(string? day, string? month, string? week)
        //{
        //    Dates? dates = new();

        //    if (!string.IsNullOrEmpty(day))
        //    {
        //        TimeSpan ts = new TimeSpan(00, 00, 0);
        //        dates.StartDate = DateTime.Parse(day).Date + ts;
        //        dates.EndDate = dates.StartDate.AddHours(23).AddMinutes(59);
        //    }
        //    else if (!string.IsNullOrEmpty(month))
        //    {
        //        string[] dateSection = month.Split('-');

        //        DateTime startDate = new DateTime(int.Parse(dateSection[0]), int.Parse(dateSection[1]), 1);
        //        DateTime endDate = new DateTime(int.Parse(dateSection[0]), int.Parse(dateSection[1]), DateTime.DaysInMonth(int.Parse(dateSection[0]), int.Parse(dateSection[1])));

        //        dates.StartDate = startDate;
        //        dates.EndDate = endDate.AddHours(23).AddMinutes(59);
        //    }
        //    else if (!string.IsNullOrEmpty(week))
        //    {
        //        string[] dateSection = week.Split("-W");
        //        DateTime startDate = FirstDateOfWeek(int.Parse(dateSection[0]), int.Parse(dateSection[1]));
        //        DateTime endDate = startDate.AddDays(6);

        //        dates.StartDate = startDate;
        //        dates.EndDate = endDate.AddHours(23).AddMinutes(59);
        //    }
        //    else
        //        return dates = null;

        //    return dates;
        //}

        //public static DateTime FirstDateOfWeek(int year, int weekOfYear)
        //{
        //    DateTime jan1 = new DateTime(year, 1, 1);
        //    int daysOffset = Convert.ToInt32(CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek) - Convert.ToInt32(jan1.DayOfWeek);
        //    DateTime firstWeekDay = jan1.AddDays(daysOffset);
        //    CultureInfo curCulture = CultureInfo.CurrentCulture;
        //    int firstWeek = curCulture.Calendar.GetWeekOfYear(jan1, curCulture.DateTimeFormat.CalendarWeekRule, curCulture.DateTimeFormat.FirstDayOfWeek);
        //    if (firstWeek <= 1)
        //    {
        //        weekOfYear -= 1;
        //    }
        //    return firstWeekDay.AddDays(weekOfYear * 7);
        //}
    }
}