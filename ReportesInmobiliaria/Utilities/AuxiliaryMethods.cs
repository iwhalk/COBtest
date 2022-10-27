using Shared.Models;
using System.Globalization;

namespace ReportesInmobiliaria.Utilities
{
    public class AuxiliaryMethods
    {
        public Fechas ObtenerFechas(string? dia, string? mes, string? semana)
        {
            Fechas? fechas = new();

            if (!string.IsNullOrEmpty(dia))
            {
                TimeSpan ts = new TimeSpan(00, 00, 0);
                fechas.FechaInicio = DateTime.Parse(dia).Date + ts;
                fechas.FechaFin = fechas.FechaInicio.AddHours(23).AddMinutes(59);
            }
            else if (!string.IsNullOrEmpty(mes))
            {
                string[] fechaPartes = mes.Split('-');

                DateTime fechaInicio = new DateTime(int.Parse(fechaPartes[0]), int.Parse(fechaPartes[1]), 1);
                DateTime fechaFin = new DateTime(int.Parse(fechaPartes[0]), int.Parse(fechaPartes[1]), DateTime.DaysInMonth(int.Parse(fechaPartes[0]), int.Parse(fechaPartes[1])));

                fechas.FechaInicio = fechaInicio;
                fechas.FechaFin = fechaFin.AddHours(23).AddMinutes(59);
            }
            else if (!string.IsNullOrEmpty(semana))
            {
                string[] fechaPartes = semana.Split("-W");
                DateTime fechaInicio = FirstDateOfWeek(int.Parse(fechaPartes[0]), int.Parse(fechaPartes[1]));
                DateTime fechaFin = fechaInicio.AddDays(6);

                fechas.FechaInicio = fechaInicio;
                fechas.FechaFin = fechaFin.AddHours(23).AddMinutes(59);
            }
            else
                return fechas = null;

            return fechas;
        }

        public static DateTime FirstDateOfWeek(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = Convert.ToInt32(CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek) - Convert.ToInt32(jan1.DayOfWeek);
            DateTime firstWeekDay = jan1.AddDays(daysOffset);
            CultureInfo curCulture = CultureInfo.CurrentCulture;
            int firstWeek = curCulture.Calendar.GetWeekOfYear(jan1, curCulture.DateTimeFormat.CalendarWeekRule, curCulture.DateTimeFormat.FirstDayOfWeek);
            if (firstWeek <= 1)
            {
                weekOfYear -= 1;
            }
            return firstWeekDay.AddDays(weekOfYear * 7);
        }
    }
}
