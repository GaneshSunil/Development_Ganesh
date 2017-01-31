using GenomeNext.Data.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNAnalysisRequestMetadata))]
    public partial class GNAnalysisRequest : GenomeNext.Data.Metadata.Audit.AuditModel
    {
        public bool CanUpdateStatus { get; set; }
        public bool IsValidSampleSetPairEndings { get; set; }
        public bool IsValidSampleNumberOfFiles { get; set; }
        public bool AllSamplesAreReady { get; set; }
        public bool IfTumorNormalIsReady { get; set; }  //If the analysis is TUMORNORMAL, the flag indicates if it's ready
        public bool IfRnaIsReady { get; set; }  //If the analysis is RNA, the flag indicates if it's ready

        public bool CanStartAnalysis { get; set; }
        public bool CanReStartAnalysis { get; set; }
        public bool AutoStart { get; set; }
        
        public bool IsFailedRequest
        {
            get
            {
                return (this.AnalysisResult != null && !this.AnalysisResult.Success);
            }
        }

        public string CurrentStatusLong { 
            get 
            {             
                var currentStatus = this.AnalysisStatus
                    .Where(s => s.PercentComplete == this.AnalysisStatus.Where(b => b.Id == this.AnalysisStatus.Max(sm => sm.Id)).FirstOrDefault().PercentComplete)
                    .OrderByDescending(r => r.CreateDateTime)
                    .LastOrDefault();
                string currentStatusMsg = "Not Submitted";
                
                if (currentStatus != null)
                {
                    currentStatusMsg = currentStatus.Status + " : " + currentStatus.Message;
                }

                if (this.IsFailedRequest)
                {
                    currentStatusMsg = "ERROR" + (currentStatus != null ? " : " + currentStatusMsg : "");
                }

                return currentStatusMsg;
            } 
        }
        
        public string CurrentStatusShort
        {
            get
            {
                var currentStatus = this.AnalysisStatus
                    .Where(s => s.PercentComplete == this.AnalysisStatus.Where(b => b.Id == this.AnalysisStatus.Max(sm => sm.Id)).FirstOrDefault().PercentComplete)
                    .OrderBy(r => r.CreateDateTime)
                    .LastOrDefault();
                string currentStatusMsg = "Not Submitted";

                if (currentStatus != null)
                {
                    currentStatusMsg = currentStatus.Status;
                }

                if(this.IsFailedRequest)
                {
                    currentStatusMsg = "ERROR";
                }

                return currentStatusMsg;
            }
        }

        private TimeSpan getTimeLapse()
        {
            //Take out the milliseconds so the result is truncated
            DateTime EndDateTime = AnalysisResult.AnalysisEndDateTime.AddMilliseconds(-AnalysisResult.AnalysisEndDateTime.Millisecond);
            DateTime StartDateTime = AnalysisResult.AnalysisStartDateTime.AddMilliseconds(-AnalysisResult.AnalysisStartDateTime.Millisecond);
            return (EndDateTime - StartDateTime);
        }

        //Format: hh:mm:ss
        public string TotalTimeLapseShort
        {
            get
            {
                if (this.AnalysisResult != null && this.AnalysisResult.AnalysisEndDateTime != null)
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
                if (this.AnalysisResult != null && this.AnalysisResult.AnalysisEndDateTime != null)
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

        public int CurrentProgress
        {
            get
            {
                var currentStatus = this.AnalysisStatus
                    .Where(s=>s.PercentComplete == this.AnalysisStatus.Where(b => b.Id == this.AnalysisStatus.Max(sm=>sm.Id)).FirstOrDefault().PercentComplete)
                    .OrderByDescending(r => r.CreateDateTime)
                    .LastOrDefault();
                int currentStatusPercentComplete = 0;
                if (currentStatus != null)
                {
                    currentStatusPercentComplete = currentStatus.PercentComplete;
                }
                return currentStatusPercentComplete;
            }
        }

        public string AnalysisTypeId { get; set; }
    }

    public partial class GNAnalysisRequestMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        [Display(Name = "Analysis ID")]
        public System.Guid Id { get; set; }
        [Required]
        public string Description { get; set; }
        [Display(Name = "Request Progress")]
        public Nullable<double> RequestProgress { get; set; }
        [Display(Name = "Request Date/Time")]
        public Nullable<System.DateTime> RequestDateTime { get; set; }
        [Display(Name = "Project")]
        public System.Guid GNProjectId { get; set; }
        [Display(Name = "Region")]
        public string AWSRegionSystemName { get; set; }

        [Display(Name = "Sample(s) Type")]
        public virtual GNAnalysisType AnalysisType { get; set; }

        [Display(Name = "Analysis Type")]
        public virtual string GNAnalysisRequestTypeCode { get; set; }
   
        public virtual GNProject Project { get; set; }
        [Display(Name = "Analysis Result")]
        public virtual GNAnalysisResult AnalysisResult { get; set; }

        [Display(Name = "Samples")]
        public virtual ICollection<GNAnalysisRequestGNSample> GNAnalysisRequestGNSamples { get; set; }
        [Display(Name = "Region")]
        public virtual AWSRegion AWSRegion { get; set; }
        [Display(Name = "Analysis Status")]
        public virtual ICollection<GNAnalysisStatus> AnalysisStatus { get; set; }
        [Display(Name = "Adaptor")]
        public string GNAnalysisAdaptor { get; set; }
    }
}
