using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GenomeNext.Portal.Models
{
    public class BulkImportViewModel
    {
        [Required]
        public HttpPostedFileBase ImportFile { get; set; }
    }
}