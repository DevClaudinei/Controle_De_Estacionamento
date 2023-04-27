using System.Text.RegularExpressions;

namespace AppServices.Validations
{
    public static class StringExtensions
    {
        public static bool LicensePlateIsValid(this string plate)
        {
            var expression = "[A-z]{3}-\\d[A-j0-9]\\d{2}";
            return Regex.Match(plate, expression).Success;
        }
    }
}
