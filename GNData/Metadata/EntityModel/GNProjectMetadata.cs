using GenomeNext.Data.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNProjectMetadata))]
    public partial class GNProject : GenomeNext.Data.Metadata.Audit.AuditModel
    {
        public string NameWithTeam
        {
            get
            {
                return this.Teams.FirstOrDefault().Name + " : " + this.Name;
            }
        }
        public string ProjectLeadId { get; set; }
        public string TeamId { get; set; }
    }

    public class GNProjectMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        public System.Guid Id { get; set; }

        [Required]
        [Display(Name="Project Name")]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:d}",ApplyFormatInEditMode=true)]
        [DataType(DataType.Date)]
        public System.DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public System.DateTime EndDate { get; set; }

        [Display(Name = "Project Lead")]
        public virtual GNContact ProjectLead { get; set; }

        [Display(Name = "Team")]
        public virtual ICollection<GNTeam> Teams { get; set; }

        [Display(Name = "Analyses")]
        public virtual ICollection<GNAnalysisRequest> AnalysisRequests { get; set; }
    }
}
