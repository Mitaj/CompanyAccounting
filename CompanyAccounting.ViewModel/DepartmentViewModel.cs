using CompanyAccounting.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyAccounting.ViewModel
{
    public class DepartmentViewModel : ViewModelBase
    {
        public DepartmentViewModel(Company parent, Department department) 
        {
            _department = department;
            Parent = parent;
        }

        public string Name
        {
            get => _department.Name;
            set
            {
                if (_department.Name == value)
                    return;

                _department.Name = value;
                RaisePropertyChanged("Name");
            }
        }

        public readonly ObservableCollection<EmployeeViewModel> Employees;

        public readonly Company Parent;
        internal void LoadEmployees()
        {
            Employees.Clear();
            foreach (var employee in _department.Employees)
                Employees.Add(new EmployeeViewModel(_department, employee));
            RaisePropertyChanged(() => Employees);
        }
        private readonly Department _department;
    }
}
