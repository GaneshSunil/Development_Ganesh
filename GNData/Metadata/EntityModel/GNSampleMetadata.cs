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
    [MetadataType(typeof(GNSampleMetadata))]
    public partial class GNSample : GenomeNext.Data.Metadata.Audit.AuditModel
    {
        public string CurrentAnalysisRequestId { get; set; }
        public bool IsValidPairEnded { get; set; }
        public bool IsValidSingleEnded { get; set; }
        public string GenderDescription
        {
            get
            {
                string gender = "Undefined Gender";
                switch(this.Gender)
                {
                    case "F": gender = "Female"; break;
                    case "M": gender = "Male"; break;
                    default: gender = "Undefined Gender"; break;
                }
                return gender;
            }
        }

        public string ReadTypeDescription 
        {
            get
            {
                return (this.IsPairEnded ? "Paired-End" : "Single-End") ;
            }
        }

        public string IsReadyDescription
        {
            get
            {
                return (this.IsReady ? "Yes" : "No, Upload in progress");
            }
        }


        
        private TimeSpan getTimeLapse()
        {
            //Take out the milliseconds so the result is truncated
            DateTime EndDateTime1 = DateTime.Parse(GNNewSampleBatchSample.GNNewSampleBatch.GNNewSampleBatchStatus.FirstOrDefault().CreateDateTime.ToString());
            DateTime StartDateTime1 = DateTime.Parse(GNNewSampleBatchSample.GNNewSampleBatch.GNNewSampleBatchStatus.LastOrDefault().CreateDateTime.ToString());

            DateTime EndDateTime = EndDateTime1.AddMilliseconds(-EndDateTime1.Millisecond);
            DateTime StartDateTime = StartDateTime1.AddMilliseconds(-StartDateTime1.Millisecond);

            return (EndDateTime - StartDateTime);
        }
        
        //Format: hh:mm:ss
        public string TotalTimeLapseShort
        {
            get
            {
                if (this.GNNewSampleBatchSample != null)
                {
                    return this.getTimeLapse().ToString();
                }
                else
                {
                    return "na";
                }
            }
        }
        
        //Format: x days, y hours, w minutes, z seconds
        public string TotalTimeLapse
        {
            get
            {
                if (this.GNNewSampleBatchSample != null)
                {
                    TimeSpan timeLapse = this.getTimeLapse();
                    return String.Format("{0} days, {1} hours, {2} minutes, {3} seconds", timeLapse.Days, timeLapse.Hours, timeLapse.Minutes, timeLapse.Seconds);
                }
                else
                {
                    return "na";
                }
            }
        }



       // public GNReplicate GNReplicate { get; set; }
    }

    public partial class GNSampleMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Ready for Analysis")]
        public bool IsReady { get; set; }


        [Display(Name = "Ready for Analysis")]
        public string IsReadyDescription { get; set; }
       
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "Gender")]
        public string GenderDescription { get; set; }

        [Display(Name = "Read type")]
        public bool IsPairEnded { get; set; }

        [Display(Name = "Read type")]
        public string ReadTypeDescription { get; set; }

        [Display(Name = "Organization")]
        public System.Guid GNOrganizationId { get; set; }

        [Display(Name = "Sequencing Type")]
        public int GNSampleTypeId { get; set; }

        [Display(Name = "Sample Type")]
        public string GNSampleQualifierCode { get; set; }

        [Display(Name = "Replicates")]
        public string GNReplicateCode { get; set; }

        [Display(Name = "Files")]
        public virtual ICollection<GNCloudFile> CloudFiles { get; set; }
        [Display(Name = "Analysis Requests")]
        public virtual ICollection<GNAnalysisRequestGNSample> GNAnalysisRequestGNSamples { get; set; }
        public virtual GNOrganization Organization { get; set; }
        [Display(Name = "Sample Type")]
        public virtual GNSampleType SampleType { get; set; }

        [Display(Name = "Pedigree")]
        public virtual ICollection<GNSampleRelationship> GNSampleLeftRelationships { get; set; }
    }

}
