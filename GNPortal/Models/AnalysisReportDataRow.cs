using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GenomeNext.Portal.Models
{
    public class AnalysisReportDataRow
    {
        public string ANALYSIS_NAME { get; set; }
        public string STATUS { get; set; }
        public string START_DATE_TIME { get; set; }
        public string END_DATE_TIME { get; set; }
        public string TOTAL_TIME { get; set; }
        public string RESULT_FILE_BUCKET { get; set; }
        public string RESULT_FILE_KEY { get; set; }
        public string RESULT_FILE_SIZE { get; set; }
    }
}