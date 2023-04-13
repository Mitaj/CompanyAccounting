using CompanyAccounting.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyAccounting.ViewModel
{
    public class EmployeeViewModel : BaseElementViewModel
    {
        public EmployeeViewModel(Department parent, Employee employee) 
            : base(employee)
        {
            Parent = parent;
            _employee = employee;
        }

        public string FirstName
        {
            get => _employee.FirstName;
            set
            {
                if (_employee.FirstName == value)
                    return;

                _employee.FirstName = value;
                RaisePropertyChanged(nameof(FirstName));
                RaisePropertyChanged(nameof(Name));
            }
        }

        public string SecondName
        {
            get => _employee.SecondName;
            set
            {
                if (_employee.SecondName == value)
                    return;

                _employee.SecondName = value;
                RaisePropertyChanged(nameof(SecondName));
                RaisePropertyChanged(nameof(Name));
            }
        }

        public string LastName
        {
            get => _employee.LastName;
            set
            {
                if (_employee.LastName == value)
                    return;

                _employee.LastName = value;
                RaisePropertyChanged(nameof(LastName));
                RaisePropertyChanged(nameof(Name));
            }
        }

        public DateTime Birthday
        {
            get => _employee.Birthday;
            set
            {
                if (_employee.Birthday == value)
                    return;

                _employee.Birthday = value;
                RaisePropertyChanged(nameof(Birthday));
                RaisePropertyChanged(nameof(Name));
            }
        }

        public bool IsSupervisor => Parent != null && Parent.SupervisorID == _employee.ID;

        public string Name => $"{_employee.FirstName} {_employee.LastName} {_employee.SecondName}";

        public readonly Department Parent;
        private readonly Employee _employee;
    }
}
