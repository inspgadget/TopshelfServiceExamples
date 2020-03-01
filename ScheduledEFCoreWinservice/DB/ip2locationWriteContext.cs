using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace ScheduledEFCoreWinservice.DB
{
    public partial class ip2locationWriteContext : ip2locationContext
    {
        public ip2locationWriteContext(IConfiguration configuration) : base(configuration)
        {
        }

        public ip2locationWriteContext(DbContextOptions<ip2locationContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("CTX_WRITE"));
            }
        }
    }
}
