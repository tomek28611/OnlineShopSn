using System.ComponentModel.DataAnnotations;

namespace OnlineShop.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Email { get; set; }

        public int? RecoveryCode { get; set; }

        [Required]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }
    }
}
