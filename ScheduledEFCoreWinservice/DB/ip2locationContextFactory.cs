using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ScheduledEFCoreWinservice.DB
{
    public class ip2locationContextFactory : IDesignTimeDbContextFactory<ip2locationContext>
    {
        public ip2locationContext CreateDbContext(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .Build();

            var builder = new DbContextOptionsBuilder<ip2locationContext>();
            var connectionString = configuration.GetConnectionString("GMGTwitterServiceContextEditor");
            builder.UseSqlServer(connectionString);
            return new ip2locationContext(builder.Options);
        }
    }
}
