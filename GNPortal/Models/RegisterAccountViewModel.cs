using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GenomeNext.Portal.Models
{
    public class RegisterAccountViewModel
    {
        //Organization Fields
        [Required]
        [Display(Name = "Invite Code")]
        public string InviteCode { get; set; }

        [Display(Name = "Time Zone")]
        public string UTCOffsetDescription { get; set; }

        //Organization Fields
        [Required]
        [Display(Name="Organization Name")]
        public string OrgName { get; set; }

        //Org Main Contact Fields
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        
        public string Title { get; set; }
        
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Fax { get; set; }

        [DataType(DataType.Url)]
        public string Website { get; set; }
        
        [Required]
        [Display(Name = "Street Address 1")]
        public string StreetAddress1 { get; set; }

        [Display(Name = "Street Address 2")]
        public string StreetAddress2 { get; set; }
        
        [Required]
        public string City { get; set; }
        
        [Required]
        public string State { get; set; }
        
        [Required]
        [DataType(DataType.PostalCode)]
        public string Zip { get; set; }

        //User Identity Fields
        [Display(Name = "Is Existing User")]
        public bool IsExistingUser { get; set; }
        
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }

        [Required]
        [Display(Name = "Terms & Conditions Accepted?")]
        public bool TermsAndConditionsAccepted { get; set; }

        [Required]
        [Display(Name = "Privacy Policy Accepted?")]
        public bool PrivacyPolicyAccepted { get; set; }

        [Display(Name = "Sign up for news and new product announcements from GenomeNext")]
        public bool SignUpForNewsAndProducts { get; set; }
    }
}