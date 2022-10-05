﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Background_Processor
{
    public class BackgroundProcessorDbContextFactory : IDesignTimeDbContextFactory<BackgroundProcessorDbContext>
    {
        public BackgroundProcessorDbContext CreateDbContext(string[] args)
        {
            var configurations = new ConfigurationBuilder()
                    .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                    .AddJsonFile("local.settings.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

            var optionsBuilder = new DbContextOptionsBuilder<BackgroundProcessorDbContext>();
            optionsBuilder.UseSqlServer(configurations.GetConnectionString("SQL"));

            return new BackgroundProcessorDbContext(optionsBuilder.Options);
        }
    }
}
