using GenomeNext.Data.EntityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GenomeNext.Portal.Models
{
    public class MyInvoicePrintModel
    {
        public string TeamName { get; set; }
        public int TotalTransactions { get; set; }

        [DataType(DataType.Currency)]
        public double TotalPrice { get; set; }

    }
}