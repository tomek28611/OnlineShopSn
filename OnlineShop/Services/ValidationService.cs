using System.Text.RegularExpressions;

namespace OnlineShop.Services
{
    public class ValidationService
    {
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;

            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            return regex.IsMatch(email);
        }
    }
}
