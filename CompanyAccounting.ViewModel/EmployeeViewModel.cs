using CompanyAccounting.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

        #region Информация о должности

        private WorkbookEntry WorkbookEntry => Parent?.WorkbookEntries?.Where(w => w.EmployeeID == ID)?.OrderBy(w => w.DateEmployment)?.FirstOrDefault();
        private JobInformation JobInfo => WorkbookEntry != null ? Model.GetJobInformation(WorkbookEntry.JobInformationID) : null;
        public string PositionName
        {
            get => IsEnabledEditJob ? _positionName : JobInfo?.PositionName;
            set
            {
                _positionName = value;
                RaisePropertyChanged(nameof(PositionName));
            }
        }
        public int SalarySumm
        {
            get => IsEnabledEditJob ? _salarySumm : (JobInfo != null ? JobInfo.SalaryDollars : 0);
            set
            { 
                _salarySumm = value;
                RaisePropertyChanged(nameof(SalarySumm));
            }
        }
        public bool IsDisabledEditJob => !IsEnabledEditJob;
        public bool IsEnabledEditJob
        {
            get => _isEnabledEditJob;
            set 
            {
                if (_isEnabledEditJob == value)
                    return;
                _isEnabledEditJob = value;
                RaisePropertyChanged(nameof(IsEnabledEditJob));
                RaisePropertyChanged(nameof(IsDisabledEditJob));
            }
        }

        public ICommand StartChangeJobCommand => _startChangeJobCommand ?? (_startChangeJobCommand = new RelayCommand(StartChangeJob));
        public ICommand CancelEditJobCommand => _cancelEditJobCommand ?? (_cancelEditJobCommand = new RelayCommand(CancelEditJob));
        public ICommand ApplyJobChangesCommand => _applyJobChangesCommand ?? (_applyJobChangesCommand = new RelayCommand(ApplyJobChanges, CanApplyJobChanges));

        private void StartChangeJob()
        {
            _positionName = PositionName;
            _salarySumm = SalarySumm;
            IsEnabledEditJob = true;
        }

        private void CancelEditJob()
        { 
            IsEnabledEditJob = false;
        }

        private void ApplyJobChanges()
        {
            if(JobInfo == null || WorkbookEntry == null) 
                return;

            var existJobInfo = Model.GetExistJobInformation(PositionName, SalarySumm);
            if(existJobInfo == null)
                existJobInfo = Model.AddJobInformation(PositionName, SalarySumm);
            WorkbookEntry.JobInformationID = existJobInfo.ID;
            Model.UpdateData(WorkbookEntry);
            IsEnabledEditJob = false;
        }

        private bool CanApplyJobChanges()
        {
            return !string.IsNullOrWhiteSpace(PositionName) && SalarySumm > 0;
        }
        #endregion

        internal void RefreshAttributes()
        {
            RaisePropertyChanged(nameof(IsSupervisor));
        }

        private ModelAssistant Model => ViewModelLocator.Instance.IoC.GetInstance<ModelAssistant>();

        public readonly Department Parent;
        private readonly Employee _employee;
        private string _positionName;
        private int _salarySumm;
        private bool _isEnabledEditJob;

        private ICommand _startChangeJobCommand;
        private ICommand _cancelEditJobCommand;
        private ICommand _applyJobChangesCommand;
    }
}
