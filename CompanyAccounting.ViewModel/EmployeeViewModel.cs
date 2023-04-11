using CompanyAccounting.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyAccounting.ViewModel
{
    public class EmployeeViewModel : ViewModelBase
    {
        public EmployeeViewModel(Department parent, Employee employee)
        {
            Parent = parent;
            _employee = employee;
        }

        public string Name => $"{_employee.FirstName} {_employee.LastName} {_employee.SecondName}";

        public readonly Department Parent;
        private readonly Employee _employee;
    }
}
