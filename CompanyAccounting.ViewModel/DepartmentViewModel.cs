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
    public class DepartmentViewModel : BaseElementViewModel
    {
        public DepartmentViewModel(Company parent, Department department)
            : base(department)
        {
            _department = department;
            Parent = parent;
            _isExpanded = false;
            _employees = new ObservableCollection<EmployeeViewModel>();
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

        public ObservableCollection<EmployeeViewModel> Employees => _employees;

        public readonly Company Parent;
        internal void LoadEmployees()
        {
            _employees.Clear();
            var loadedEmployees = ViewModelLocator.Instance.IoC.GetInstance<ModelAssistant>().Employees;
            foreach (var workbookEntry in _department.WorkbookEntries)
            {
                var employee = loadedEmployees.FirstOrDefault(x => x.ID == workbookEntry.EmployeeID);
                if (employee != null)
                    _employees.Add(new EmployeeViewModel(_department, employee));
            }
            RaisePropertyChanged(() => Employees);
        }

        internal void Remove(EmployeeViewModel employee)
        { 
            Employees?.Remove(employee);
        }

        private void LoadIfNeedEmployees()
        {
            ViewModelLocator.Instance.IoC.GetInstance<ModelAssistant>().LoadEmployees(_department);
            LoadEmployees();
        }

        private readonly ObservableCollection<EmployeeViewModel> _employees;
        private readonly Department _department;
        private bool _isExpanded;
    }
}
