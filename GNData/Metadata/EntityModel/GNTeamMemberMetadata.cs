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
    [MetadataType(typeof(GNTeamMemberMetadata))]
    public partial class GNTeamMember : GenomeNext.Data.Metadata.Audit.AuditModel
    {       
    }

    public class GNTeamMemberMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        [Display(Name="Team")]
        public System.Guid GNTeamId { get; set; }

        [Display(Name = "Team Member")]
        public System.Guid GNContactId { get; set; }
        
        [Display(Name = "Date Assigned")]
        [DataType(DataType.Date)]
        public System.DateTime AssignDate { get; set; }

        [Display(Name = "Team")]
        public virtual GNTeam Team { get; set; }
        
        [Display(Name = "Team Member")]
        public virtual GNContact Contact { get; set; }
    }
}
