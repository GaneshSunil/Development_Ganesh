using GenomeNext.Data.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNTeamMetadata))]
    public partial class GNTeam : GenomeNext.Data.Metadata.Audit.AuditModel
    {
    }

    public class GNTeamMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        public System.Guid Id { get; set; }

        [Required]
        [Display(Name = "Team Name")]
        public string Name { get; set; }
        
        [Display(Name="Organization")]
        public System.Guid OrganizationId { get; set; }
        
        [Display(Name = "Team Lead")]
        public System.Guid GNContactId { get; set; }

        [Display(Name = "Team Lead")]
        public virtual GNContact TeamLead { get; set; }

        [Display(Name = "Team Members")]
        public virtual ICollection<GNTeamMember> TeamMembers { get; set; }
        
        [Display(Name = "Organization")]
        public virtual GNOrganization Organization { get; set; }
        
        public virtual ICollection<GNProject> Projects { get; set; }
    }
}
