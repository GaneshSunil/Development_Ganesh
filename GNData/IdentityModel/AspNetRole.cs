//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GenomeNext.Data.IdentityModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class AspNetRole
    {
        public AspNetRole()
        {
            this.AspNetUserRoles = new HashSet<AspNetUserRoles>();
        }
    
        public string Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> HierarchyOrder { get; set; }
    
        public virtual ICollection<AspNetUserRoles> AspNetUserRoles { get; set; }
    }
}
