using CompanyAccounting.Model.DataBaseComponents;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace CompanyAccounting.Model
{
    public class ModelAssistant : BaseObject
    {
        public ModelAssistant() 
        { 
            Companies = new ObservableCollection<Company>();
            Employees = new ObservableCollection<Employee>();
            DBAssistant = new DataBaseAssistant(string.Empty);
        }

        public void LoadCompanies()
        {
            var companies = DBAssistant.Companies.ToList();
            Companies.Clear();
            foreach (var company in companies) 
            {
                Companies.Add(company);
            }

            RaisePropertyChanged("Companies");
        }

        public void SetConnectionString(string connectionString)
        {
            DBAssistant?.SetConnectionString(connectionString);
        }

        public bool Connected => DBConnection != null && DBConnection.State != System.Data.ConnectionState.Closed;

        public ObservableCollection<Company> Companies { get; private set; }
        public ObservableCollection<Employee> Employees { get; private set; }

        private DbConnection DBConnection => DBAssistant?.Database?.GetDbConnection();
        private DataBaseAssistant DBAssistant { get; set; }
    }
}
