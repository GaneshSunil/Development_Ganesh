using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.Security
{
    public abstract class SecureModel
    {
        public Boolean CanView { get; set; }
        public Boolean CanCreate { get; set; }
        public Boolean CanEdit { get; set; }
        public Boolean CanDelete { get; set; }
    }
}
