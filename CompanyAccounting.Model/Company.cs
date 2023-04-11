using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyAccounting.Model
{
    public class Company : BaseTableObject
    {
        public Company() 
        {
            Departments = new ObservableCollection<Department>();
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

        public DateTime DateCreation
        {
            get => _dateCreation;
            set
            {
                if (_dateCreation == value)
                    return;
                _dateCreation = value;
                RaisePropertyChanged(nameof(DateCreation));
            }
        }

        public readonly ObservableCollection<Department> Departments;

        internal void SetDepartments(IEnumerable<Department> departments)
        {
            Departments.Clear();
            if (departments == null)
                return;

            foreach (var department in departments)
                Departments.Add(department);
        }

        private string _name;
        private DateTime _dateCreation;
    }
}
