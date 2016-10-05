using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsCommon
{
    public class ContosoAdsContext : DbContext
    {
        public ContosoAdsContext() : base("ContosoAdsContext")
        {
        }

        public ContosoAdsContext(string connectionString) : base(connectionString)
        {
        }

        // DataSet
        public DbSet<Ad> Ads { get; set; }
    }
}
