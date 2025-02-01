using OnlineShop.Models.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using OnlineShop.Data;

namespace OnlineShop.Services
{
    public class UserService
    {
        private readonly OnlineShopContext _context;

        public UserService(OnlineShopContext context)
        {
            _context = context;
        }

        public bool IsEmailDuplicate(string email)
        {
            return _context.Users.Any(x => x.Email == email);
        }

        public void CreateUser(User user)
        {
            user.RegisterDate = DateTime.Now;
            user.IsAdmin = false;
            user.Email = user.Email.Trim();
            user.Password = user.Password.Trim();
            user.FullName = user.FullName?.Trim();
            user.RecoveryCode = 0;

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public User AuthenticateUser(string email, string password)
        {
            return _context.Users.FirstOrDefault(x => x.Email.Trim() == email.Trim() && x.Password.Trim() == password.Trim());
        }

        public ClaimsPrincipal GenerateClaimsPrincipal(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.IsAdmin ? "admin" : "user")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            return new ClaimsPrincipal(identity);
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(x => x.Email == email.Trim());
        }

        public int GenerateRecoveryCode(User user)
        {
            var recoveryCode = new Random().Next(10000, 100000);
            user.RecoveryCode = recoveryCode;
            _context.Users.Update(user);
            _context.SaveChanges();

            return recoveryCode;
        }

        public bool VerifyRecoveryCode(string email, int recoveryCode)
        {
            var user = GetUserByEmail(email);
            return user != null && user.RecoveryCode == recoveryCode;
        }

        public void UpdatePassword(string email, string newPassword)
        {
            var user = GetUserByEmail(email);
            if (user != null)
            {
                user.Password = newPassword;
                _context.Users.Update(user);
                _context.SaveChanges();
            }
        }
    }
}

