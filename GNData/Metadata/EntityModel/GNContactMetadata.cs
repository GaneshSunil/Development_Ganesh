using GenomeNext.Data.IdentityModel;
using GenomeNext.Data.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNContactMetadata))]
    public partial class GNContact : GenomeNext.Data.Metadata.Audit.AuditModel
    {
        public string FullName
        {
            get
            {
                return this.FirstName + " " + this.LastName;
            }
        }
        public string FullNameWithEmail
        {
            get
            {
                return this.FirstName + " " + this.LastName + " (" + this.Email + ")";
            }
        }

        public AspNetUser User { get; set; }
        
        public string IsSubscribedForNewslettersText
        {
            get
            {
                string text = "No";
                switch (this.IsSubscribedForNewsletters)
                {
                    case true: text = "Yes"; break;
                    case false: text = "No"; break;
                    default: text = "No"; break;
                }

                return text;
            }
        }

        public string IsInviteAcceptedText
        {
            get
            {
                string text = "Not set";
                switch (this.IsInviteAccepted)
                {
                    case true: text = "Yes"; break;
                    case false: text = "No"; break;
                    default: text = "Not set"; break;
                }

                return text;
            }
        }
        
        public bool IsInRole(string role)
        {
            bool isInRole = false;

            if(!string.IsNullOrEmpty(role) && GNContactRoles != null)
            {
                foreach (var contactRole in GNContactRoles)
                {
                    if(contactRole.AspNetRole == null)
                    {
                        contactRole.AspNetRole = new IdentityModelContainer().AspNetRoles.Find(contactRole.AspNetRoleId);
                    }

                    if(contactRole.AspNetRole != null && contactRole.AspNetRole.Name == role)
                    {
                        isInRole = true;
                        break;
                    }
                }
            }

            if (!isInRole && User != null && User.AspNetUserRoles != null)
            {
                foreach (var userRole in User.AspNetUserRoles)
                {
                    if (userRole.AspNetRole != null && userRole.AspNetRole.Name == role)
                    {
                        isInRole = true;
                        break;
                    }
                }
            }

            return isInRole;
        }

        public int GetUserMinHierarchyOrder()
        {
            int minHierarchyOrder = 99;

            if(IsInRole("GN_ADMIN"))
            {
                minHierarchyOrder = 1;
            }
            else
            {
                minHierarchyOrder = GNContactRoles.Min(a => a.AspNetRole.HierarchyOrder).Value;
            }
            
            return minHierarchyOrder;
        }
    }

    public class GNContactMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        public System.Guid Id { get; set; }

        [Required]
        [Display(Name="First Name")]
        public string FirstName { get; set; }
        
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        
        public string Title { get; set; }
        
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        
        [DataType(DataType.PhoneNumber)]
        public string Fax { get; set; }
        
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [DataType(DataType.Url)]
        public string Website { get; set; }
        
        [Display(Name = "Street Address 1")]
        public string StreetAddress1 { get; set; }
        
        [Display(Name = "Street Address 2")]
        public string StreetAddress2 { get; set; }
        
        public string City { get; set; }
        
        public string State { get; set; }
        
        [DataType(DataType.PostalCode)]
        public string Zip { get; set; }
        
        [Display(Name = "Invite Accepted?")]
        public Nullable<bool> IsInviteAccepted { get; set; }

        [Display(Name = "Subscribed for News?")]
        public Nullable<bool> IsSubscribedForNewsletters { get; set; }

        [Display(Name = "Terms & Conditions Acceptance Date/Time")]
        [DataType(DataType.DateTime)]
        public Nullable<System.DateTime> TermsAcceptDateTime { get; set; }

        [Display(Name = "Privacy Policy Acceptance Date/Time")]
        [DataType(DataType.DateTime)]
        public Nullable<System.DateTime> PrivacyPolicyAcceptDateTime { get; set; }

        [Display(Name = "User Login")]
        public string AspNetUserId { get; set; }
        
        [Display(Name = "Organization Code")]
        public System.Guid GNOrganizationId { get; set; }

        [Display(Name = "Organization")]
        public virtual GNOrganization GNOrganization { get; set; }

        [Display(Name = "Roles")]
        public virtual ICollection<GNContactRole> GNContactRoles { get; set; }
    }

    public class GNContactEqualityComparer : EqualityComparer<GNContact>
    {

        public override bool Equals(GNContact c1, GNContact c2)
        {
            if (c1.Id == c2.Id)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public override int GetHashCode(GNContact c)
        {
            int hCode = c.Id.ToByteArray().Length ^ new Random().Next();
            return hCode.GetHashCode();
        }

    }
}
