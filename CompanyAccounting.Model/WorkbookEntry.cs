using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CompanyAccounting.Model
{
    public class WorkbookEntry : BaseTableObject
    {
        public int EmployeeID
        {
            get => _employeeId;
            set
            {
                if (_employeeId == value)
                    return;
                _employeeId = value;
                RaisePropertyChanged(nameof(EmployeeID));
            }
        }

        public int DepartmentID
        {
            get => _departmentId;
            set
            {
                if (_departmentId == value)
                    return;
                _departmentId = value;
                RaisePropertyChanged(nameof(DepartmentID));
            }
        }

        public int JobInformationID
        {
            get => _jobInformationId;
            set
            {
                if (_jobInformationId == value)
                    return;
                _jobInformationId = value;
                RaisePropertyChanged(nameof(JobInformationID));
            }
        }

        public DateTime DateEmployment
        {
            get => _dateEmployment;
            set
            {
                if (_dateEmployment == value)
                    return;
                _dateEmployment = value;
                RaisePropertyChanged(nameof(DateEmployment));
            }
        }

        private int _employeeId;
        private int _departmentId;
        private int _jobInformationId;
        private DateTime _dateEmployment;
    }
}
