namespace ReportesInmobiliaria.Utilities
{
    public class Validators
    {
        public string? GetNullableString(string value) => !string.IsNullOrWhiteSpace(value) && value.ToUpper().Contains("NULL") ? null : value;
    }
}
