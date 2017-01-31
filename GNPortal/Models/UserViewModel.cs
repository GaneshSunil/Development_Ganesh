using System.ComponentModel.DataAnnotations;

namespace GenomeNext.Portal.Models
{
    public class UserViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        //User Identity Fields
        [Display(Name = "Is Email Confirmed?")]
        public bool EmailConfirmed { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string PasswordConfirm { get; set; }


        //User Identity Fields
        [Display(Name = "Is Admin")]
        public bool IsAdmin { get; set; }

        public string teamId { get; set; }

    }

}
