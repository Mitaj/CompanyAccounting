using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CompanyAccounting.Model
{
    public class Department : BaseTableObject
    {
        public Department() 
        {
            Employees = new ObservableCollection<Employee>();
        }

        public int CompanyID
        {
            get => _company_id;
            set
            {
                if (_company_id == value)
                    return;
                _company_id = value;
                RaisePropertyChanged(nameof(CompanyID));
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name == value)
                    return;
                _name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        public readonly ObservableCollection<Employee> Employees;

        internal void SetEmployees(IEnumerable<Employee> employees)
        {
            Employees.Clear();
            if (employees == null)
                return;

            foreach (var employee in employees)
                Employees.Add(employee);
        }

        private int _company_id;
        private string _name;
    }
}
