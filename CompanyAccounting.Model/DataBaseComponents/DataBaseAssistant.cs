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
        public DataBaseAssistant(string connectionString) 
        { 
            ConnectionString = connectionString;
        }
        public string ConnectionString { get; private set; }

        public DbSet<Company> Companies { get; set; } = null;

        internal void SetConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConnectionString);
        }
    }
}
