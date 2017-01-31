using GenomeNext.Data.IdentityModel;
using GenomeNext.Data.Metadata.Audit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.IdentityModel
{
    [MetadataType(typeof(AspNetUserMetadata))]
    public partial class AspNetUser : AuditModel
    {
        public IdentityModelContainer dbContext { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }

        public List<string> RoleIds { get; set; }

        public bool IsAdmin { get; set; }

        public int ContactCount { get; set; }

        public IList<AspNetRole> Roles
        {
            get
            {
                IList<AspNetRole> roles = null;
                if (this.AspNetUserRoles != null && this.AspNetUserRoles.Count != 0 && dbContext != null)
                {
                    roles = new List<AspNetRole>();

                    foreach (var userRole in this.AspNetUserRoles)
                    {
                        roles.Add(dbContext.AspNetRoles.Find(userRole.RoleId));
                    }
                }

                return roles;
            }
        }
    }

    public class AspNetUserMetadata : AuditModelMetadata
    {
        public string Id { get; set; }
        public string Email { get; set; }
        [Display(Name="Email Confirmed")]
        public bool EmailConfirmed { get; set; }
        [Display(Name = "Password Hash")]
        public string PasswordHash { get; set; }
        [Display(Name = "Security Stamp")]
        public string SecurityStamp { get; set; }
        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Phone Confirmed")]
        public bool PhoneNumberConfirmed { get; set; }
        [Display(Name = "MFA Enabled")]
        public bool TwoFactorEnabled { get; set; }
        [Display(Name = "Lockout End Date")]
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        [Display(Name = "Lockout Enabled")]
        public bool LockoutEnabled { get; set; }
        [Display(Name = "Access Failed Count")]
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }

        [Display(Name = "Is Admin?")]
        public bool IsAdmin { get; set; }

        [Display(Name = "# Org Contacts")]
        public int ContactCount { get; set; }

        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual ICollection<AspNetUserRoles> AspNetUserRoles { get; set; }
    }
}
