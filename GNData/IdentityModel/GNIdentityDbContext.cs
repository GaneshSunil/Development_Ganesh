using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.IdentityModel
{
    public class GNIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public GNIdentityDbContext()
            : base("GN_DB", throwIfV1Schema: false)
        {
        }

        public static GNIdentityDbContext Create()
        {
            return new GNIdentityDbContext();
        }
    }
}
