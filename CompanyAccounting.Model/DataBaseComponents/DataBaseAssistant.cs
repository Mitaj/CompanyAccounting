using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyAccounting.Model.DataBaseComponents
{
    internal class DataBaseAssistant : DbContext
    {
        public DataBaseAssistant() 
        {
            ChangeTracker.AutoDetectChangesEnabled = false;
        }
        internal static string ConnectionString { get; private set; }

        public DbSet<Company> Companies { get; set; } = null;
        public DbSet<Department> Departments { get; set; } = null;
        public DbSet<WorkbookEntry> WorkbookEntries { get; set; } = null;
        public DbSet<Employee> Employees { get; set; } = null;
        public DbSet<Employee> JobInformations { get; set; } = null;

        internal static void SetConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConnectionString);
        }
    }
}
