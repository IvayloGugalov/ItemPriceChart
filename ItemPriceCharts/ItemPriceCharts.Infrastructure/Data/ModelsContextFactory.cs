using System;
using System.IO;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

using ItemPriceCharts.InfraStructure.Constants;

namespace ItemPriceCharts.Infrastructure.Data
{
    public class ModelsContextFactory : IDisposable
    {
        private StreamWriter logStream;

        public ModelsContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ModelsContext>();

            this.logStream = new StreamWriter(
                path: string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DatabaseKeyWordConstants.DATABASE_LOG_PATH),
                append: true);

            options.UseSqlite(DatabaseKeyWordConstants.CONNECTION_STRING)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
                .LogTo(this.logStream.WriteLine,
                    Microsoft.Extensions.Logging.LogLevel.Information,
                    DbContextLoggerOptions.DefaultWithUtcTime);

            return new ModelsContext(options.Options);
        }

        public void Dispose()
        {
            this.logStream?.Dispose();
        }
    }
}
