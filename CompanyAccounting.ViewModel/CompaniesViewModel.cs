﻿using CompanyAccounting.Model;
using CompanyAccounting.Model.RegistryData;
using CompanyAccounting.ViewModel.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RelayCommand = GalaSoft.MvvmLight.CommandWpf.RelayCommand;

namespace CompanyAccounting.ViewModel
{
    public class CompaniesViewModel : ViewModelBase
    {
        public CompaniesViewModel(string title)
        { 
            Title = title;
            _companies = new ObservableCollection<CompanyViewModel>();
            _reportBuilder = new ReportBuilder();
            LoadCompanies();
            AddCompanyPanelVisible = false;
            _experienceYears = new HashSet<byte>();
            _ages = new HashSet<byte>();
            _yearsOfBirth = new HashSet<ushort>();
            for (byte i = 0; i <= CountExperienceYears; i++)
                _experienceYears.Add(i);
            for(byte i = StartWorkAge; i <= EndWorkAge; i++)
                _ages.Add(i);
            var todayYear = DateTime.Now.Year;
            var startYearBirthEmployee = (ushort)(todayYear - EndWorkAge);
            var endYearBirthEmployee = (ushort)(todayYear - StartWorkAge);
            for(var i = startYearBirthEmployee; i <= endYearBirthEmployee; i++)
                _yearsOfBirth.Add(i);
            IsFilterTypeAge = true;
            IsFilterTypeYearOfBirth = false;
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged(nameof(Title));
            }
        }

        public BaseElementViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                if(_selectedItem == value)
                    return;
                _selectedItem = value;
                RaisePropertyChanged(nameof(SelectedItem));
            }
        }

        public void LoadCompanies()
        {
            _companies.Clear();
            var loadedCompanyModels = Model.Companies;
            if (loadedCompanyModels == null || loadedCompanyModels.Count == 0)
            {
                RaisePropertyChanged(nameof(Companies));
                return;
            }

            foreach (var company in loadedCompanyModels)
                LoadCompany(company);

            RaisePropertyChanged(nameof(Companies));
        }

        public ObservableCollection<CompanyViewModel> Companies => _companies;

        public bool IsDisabledConnectionDataEditing => !IsEnabledConnectionDataEditing;

        public bool IsEnabledConnectionDataEditing
        {
            get => _isEnabledConnectionDataEditing;
            set 
            {
                if (_isEnabledConnectionDataEditing == value)
                    return;
                _isEnabledConnectionDataEditing = value;
                RaisePropertyChanged(nameof(IsEnabledConnectionDataEditing));
                RaisePropertyChanged(nameof(IsDisabledConnectionDataEditing));
                RaisePropertyChanged(nameof(ConnectionString));
            }
        }

        public string ConnectionString
        {
            get => _isEnabledConnectionDataEditing ? _connectionString : Model.GetConnectionString();
            set 
            {
                if (_connectionString == value)
                    return;
                _connectionString = value;
                RaisePropertyChanged(nameof(ConnectionString));
            }
        }

        public bool ControlPanelVisible => !AddCompanyPanelVisible && !AddDepartmentPanelVisible && !AddEmployeePanelVisible;
        public bool AddCompanyPanelVisible 
        {
            get => _addCompanyPanelVisible;
            private set 
            {
                _addCompanyPanelVisible = value;
                RaisePropertyChanged(nameof(AddCompanyPanelVisible));
                RaisePropertyChanged(nameof(ControlPanelVisible));
            } 
        }

        public string AddCompanyName
        {
            get => _addCompanyName;
            set 
            {
                _addCompanyName = value;
                RaisePropertyChanged(nameof(AddCompanyName));
            }
        }

        public DateTime AddCompanyDateCreation
        {
            get => _addCompanyDateCreation;
            set
            {
                _addCompanyDateCreation = value;
                RaisePropertyChanged(nameof(AddCompanyDateCreation));
            }
        }

        public bool AddDepartmentPanelVisible
        {
            get => _addDepartmentPanelVisible;
            private set
            {
                _addDepartmentPanelVisible = value;
                RaisePropertyChanged(nameof(AddDepartmentPanelVisible));
                RaisePropertyChanged(nameof(ControlPanelVisible));
            }
        }

        public bool AddEmployeePanelVisible
        {
            get => _addEmployeePanelVisible;
            private set
            {
                _addEmployeePanelVisible = value;
                RaisePropertyChanged(nameof(AddEmployeePanelVisible));
                RaisePropertyChanged(nameof(ControlPanelVisible));
            }
        }

        public string AddDepartmentName
        {
            get => _addDepartmentName;
            set
            {
                _addDepartmentName = value;
                RaisePropertyChanged(nameof(AddDepartmentName));
            }
        }

        public string AddEmployeeFirstName
        {
            get => _addEmployeeFirstName;
            set
            {
                _addEmployeeFirstName = value;
                RaisePropertyChanged(nameof(AddEmployeeFirstName));
            }
        }

        public string AddEmployeeSecondName
        {
            get => _addEmployeeSecondName;
            set
            {
                _addEmployeeSecondName = value;
                RaisePropertyChanged(nameof(AddEmployeeSecondName));
            }
        }

        public string AddEmployeeLastName
        {
            get => _addEmployeeLastName;
            set
            {
                _addEmployeeLastName = value;
                RaisePropertyChanged(nameof(AddEmployeeLastName));
            }
        }

        public DateTime AddEmployeeBirthday
        {
            get => _addEmployeeBirthday;
            set
            {
                _addEmployeeBirthday = value;
                RaisePropertyChanged(nameof(AddEmployeeBirthday));
            }
        }

        public string AddPositionName
        {
            get => _addPositionName;
            set
            {
                _addPositionName = value;
                RaisePropertyChanged(nameof(AddPositionName));
            }
        }

        public int AddSalarySumm
        {
            get => _addSalarySumm;
            set
            {
                _addSalarySumm = value;
                RaisePropertyChanged(nameof(AddSalarySumm));
            }
        }

        public int FilterCompanyIDReport
        {
            get => _filterCompanyIDReport;
            set 
            {
                _filterCompanyIDReport = value;
                RaisePropertyChanged(nameof(FilterCompanyIDReport));
            }
        }

        public byte FilterExperienceInYear
        {
            get => _filterExperienceInYear;
            set 
            {
                _filterExperienceInYear = value;
                RaisePropertyChanged(nameof(FilterExperienceInYear));
            }
        }

        public byte FilterAge
        {
            get => _filterAge;
            set 
            {
                _filterAge = value;
                RaisePropertyChanged(nameof(FilterAge));
            }
        }
        public ushort FilterYearOfBirth
        {
            get => _filterYearOfBirth;
            set 
            {
                _filterYearOfBirth = value;
                RaisePropertyChanged(nameof(FilterYearOfBirth));
            }
        }

        public bool IsFilterTypeAge
        {
            get => _isFilterTypeAge;
            set
            {
                _isFilterTypeAge = value;
                RaisePropertyChanged(nameof(IsFilterTypeAge));
                RaisePropertyChanged(nameof(IsFilterTypeYearOfBirth));
            }
        }

        public bool IsFilterTypeYearOfBirth
        {
            get => _isFilterTypeYearOfBirth;
            set
            {
                _isFilterTypeYearOfBirth = value;
                RaisePropertyChanged(nameof(IsFilterTypeYearOfBirth));
            }
        }


        public ICommand AddCompanyCommand => _addCompanyCommand ?? (_addCompanyCommand = new RelayCommand(AddCompany));
        public ICommand ApplyAddCompanyCommand => _applyAddCompanyCommand ?? (_applyAddCompanyCommand = new RelayCommand(ApplyAddCompany, CanAddCompany));
        public ICommand ApplyAddDepartmentCommand => _applyAddDepartmentCommand ?? (_applyAddDepartmentCommand = new RelayCommand(ApplyAddDepartment, CanAddDepartment));
        public ICommand CancelAddCompanyCommand => _cancelAddCompanyCommand ?? (_cancelAddCompanyCommand = new RelayCommand(CancelAddCompany));
        public ICommand CancelAddDepartmentCommand => _cancelAddDepartmentCommand ?? (_cancelAddDepartmentCommand = new RelayCommand(CancelAddDepartment));
        public ICommand CancelAddEmployeeCommand => _cancelAddEmployeeCommand ?? (_cancelAddEmployeeCommand = new RelayCommand(CancelAddEmployee));
        public ICommand ApplyAddEmployeeCommand => _applyAddEmployeeCommand ?? (_applyAddEmployeeCommand = new RelayCommand(ApplyAddEmployee, CanAddEmployee));
        public ICommand AddItemCommand => _addItemCommand ?? (_addItemCommand = new RelayCommand(AddItem, CanAddItem));
        public ICommand DeleteItemCommand => _deleteItemCommand ?? (_deleteItemCommand = new RelayCommand(DeleteItem, CanDeleteItem));

        public ICommand BuildPayrollReportCommand => _buildPayrollReportCommand ?? (_buildPayrollReportCommand = new RelayCommand(BuildPayrollReport));
        public ICommand BuildListOfEmployeesReportCommand => _buildListOfEmployeesReportCommand ?? (_buildListOfEmployeesReportCommand = new RelayCommand(BuildListOfEmployeesReport));

        public ICommand StartEditConnectionDataCommand => _startEditConnectionDataCommand ?? (_startEditConnectionDataCommand = new RelayCommand(StartEditConnectionData));
        public ICommand CancelEditConnectionDataCommand => _cancelEditConnectionDataCommand ?? (_cancelEditConnectionDataCommand = new RelayCommand(CancelEditConnectionData));
        public ICommand ApplyConnectionDataCommand => _applyConnectionDataCommand ?? (_applyConnectionDataCommand = new RelayCommand(ApplyConnectionData));

        public HashSet<byte> ExperienceYears => _experienceYears;
        public HashSet<byte> Ages => _ages;
        public HashSet<ushort> YearsOfBirth => _yearsOfBirth;

        private void StartEditConnectionData()
        { 
            ConnectionString = Model.GetConnectionString();
            IsEnabledConnectionDataEditing = true;    
        }

        private void CancelEditConnectionData()
        {
            IsEnabledConnectionDataEditing = false;
        }

        private void ApplyConnectionData()
        { 
            Model.SetConnectionString(ConnectionString);
            IsEnabledConnectionDataEditing = false;
            RegistryAssistance.SetRegistryParamValue(RegistryParam.ConnectionString, ConnectionString);
            Model.LoadCompanies();
            Model.LoadDepartments();
            LoadCompanies();      
        }

        private void BuildPayrollReport()
        {
            if (_reportBuilder?.BuildPayrollReportCommand == null)
                return;
            foreach (var company in Companies)
                company.IsSelected = !company.IsSelected;
            if(_reportBuilder.BuildPayrollReportCommand.CanExecute(null))
                _reportBuilder.BuildPayrollReportCommand.Execute(null);
        }

        private void BuildListOfEmployeesReport()
        {
            if (_reportBuilder?.BuildListOfEmployeesReportCommand == null)
                    return;
            foreach (var company in Companies)
            {
                if (company.ID != FilterCompanyIDReport)
                    continue;
                company.IsSelected = !company.IsSelected;
            }

            var filter = new FilterListOfEmployees()
            {
                CompanyID = FilterCompanyIDReport,
                ExperienceInYears = FilterExperienceInYear,
                UseAgeType = IsFilterTypeAge,
                Age = FilterAge,
                YearOfBirth = FilterYearOfBirth,
                RequestDate = DateTime.Now,
            };

            if (_reportBuilder.BuildListOfEmployeesReportCommand.CanExecute(null))
                _reportBuilder.BuildListOfEmployeesReportCommand.Execute(filter);
        }

        private void AddCompany()
        {
            AddCompanyPanelVisible = true;
            AddCompanyName = string.Format(CompanyNameTemplate, Companies == null ? 1 : (Companies.Count + 1));
            AddCompanyDateCreation = DateTime.Now.Date;
        }

        private bool CanAddCompany()
        {
            return !string.IsNullOrWhiteSpace(AddCompanyName);
        }

        private void ApplyAddCompany()
        {
            var addedCompany = Model.AddCompany(AddCompanyName, AddCompanyDateCreation);
            LoadCompany(addedCompany);
            RaisePropertyChanged(nameof(Companies));
            AddCompanyPanelVisible = false;
        }

        private void ApplyAddDepartment()
        {
            if(!(SelectedItem is CompanyViewModel company))
                return;

            var addedDepartment = Model.AddDepartment(AddDepartmentName, company.Base as Company);
            LoadDepartment(company, addedDepartment);
            RaisePropertyChanged(nameof(Companies));
            AddDepartmentPanelVisible = false;
        }

        private bool CanAddDepartment()
        {
            return !string.IsNullOrWhiteSpace(AddDepartmentName);
        }

        private void ApplyAddEmployee()
        {
            if (!(SelectedItem is DepartmentViewModel departmentVM) ||
                !(departmentVM.Base is Department department))
                return;
            
            var addedEmployee = Model.AddEmployee(AddEmployeeFirstName, AddEmployeeSecondName, 
                                                  AddEmployeeLastName, AddEmployeeBirthday);
            var jobInformation = Model.GetExistJobInformation(AddPositionName, AddSalarySumm);
            if(jobInformation == null)
                jobInformation = Model.AddJobInformation(AddPositionName, AddSalarySumm);
            var addedWorkbookEntry = Model.AddWorkbookEntry(addedEmployee, DateTime.Now.Date, department, jobInformation);
            if (addedWorkbookEntry == null)
                return;
            
            LoadEmployee(departmentVM, addedEmployee);
            RaisePropertyChanged(nameof(Companies));
            AddEmployeePanelVisible = false; 
        }

        private bool CanAddEmployee()
        {
            return !string.IsNullOrWhiteSpace(AddEmployeeFirstName) && !string.IsNullOrWhiteSpace(AddEmployeeLastName) &&
                   !string.IsNullOrWhiteSpace(AddPositionName) && AddSalarySumm > 0;
        }

        private void LoadCompany(Company company)
        {
            if (company == null)
                return;

            var companyVM = new CompanyViewModel(company);
            companyVM.Departments.Clear();
            if(company.Departments != null)
                foreach (var department in company.Departments)
                    companyVM.Departments.Add(new DepartmentViewModel(company, department));

            _companies.Add(companyVM);
        }

        private void LoadDepartment(CompanyViewModel companyVM, Department department)
        {
            if (companyVM == null || !(companyVM.Base is Company company))
                return;

            companyVM.Departments.Add(new DepartmentViewModel(company, department));
        }

        private void LoadEmployee(DepartmentViewModel departmentVM, Employee employee)
        {
            if (departmentVM == null || !(departmentVM.Base is Department department))
                return;

            departmentVM.Employees.Add(new EmployeeViewModel(department, employee));
        }

        private void CancelAddCompany()
        {
            AddCompanyPanelVisible = false;
        }

        private void CancelAddDepartment()
        { 
            AddDepartmentPanelVisible = false;
        }        

        private void CancelAddEmployee()
        { 
            AddEmployeePanelVisible = false;
        }

        private void AddItem()
        {
            if (SelectedItem is CompanyViewModel)
                AddDepartmentPanelVisible = true;
            else if (SelectedItem is DepartmentViewModel)
            {
                AddEmployeePanelVisible = true;
                AddEmployeeBirthday = DateTime.Now.Date.AddYears(-18);
            }
        }

        private bool CanAddItem()
        {
            return (SelectedItem is CompanyViewModel) || (SelectedItem is DepartmentViewModel);
        }

        private void DeleteItem()
        {
            if (SelectedItem == null)
                return;

            Model.DeleteTableItem(SelectedItem.Base);
            if (SelectedItem is CompanyViewModel company)
            {
                Companies?.Remove(company);
            }
            else if (SelectedItem is DepartmentViewModel department)
            {
                if (Companies != null)
                    foreach(var companyItem in Companies)
                        companyItem?.Remove(department);  
            }
            else if (SelectedItem is EmployeeViewModel employee)
            {
                if (Companies != null)
                    foreach (var companyItem in Companies)
                        if (companyItem.Departments != null)
                            foreach (var departmentItem in companyItem.Departments)
                                departmentItem.Remove(employee);
            }
            
            RaisePropertyChanged(nameof(Companies));
        }

        private bool CanDeleteItem()
        {
            return SelectedItem != null;
        }

        private ModelAssistant Model => ViewModelLocator.Instance.IoC.GetInstance<ModelAssistant>();

        private const string CompanyNameTemplate = "Компания {0}";

        private readonly ObservableCollection<CompanyViewModel> _companies;
        private readonly ReportBuilder _reportBuilder;

        private string _title;
        private BaseElementViewModel _selectedItem;
        private bool _isEnabledConnectionDataEditing;
        private bool _addCompanyPanelVisible;
        private bool _addDepartmentPanelVisible;
        private bool _addEmployeePanelVisible;
        private string _addCompanyName;
        private string _addDepartmentName;
        private string _addEmployeeFirstName;
        private string _addEmployeeSecondName;
        private string _addEmployeeLastName;
        private string _addPositionName;
        private int _addSalarySumm;
        private DateTime _addCompanyDateCreation;
        private DateTime _addEmployeeBirthday;
        private ICommand _addCompanyCommand;
        private ICommand _applyAddCompanyCommand;
        private ICommand _applyAddDepartmentCommand;
        private ICommand _applyAddEmployeeCommand;
        private ICommand _cancelAddCompanyCommand;
        private ICommand _cancelAddDepartmentCommand;
        private ICommand _cancelAddEmployeeCommand;
        private ICommand _addItemCommand;
        private ICommand _deleteItemCommand;
        private ICommand _startEditConnectionDataCommand;
        private ICommand _cancelEditConnectionDataCommand;
        private ICommand _applyConnectionDataCommand;
        private string _connectionString;

        private ICommand _buildPayrollReportCommand;
        private ICommand _buildListOfEmployeesReportCommand;
        private int _filterCompanyIDReport;
        private byte _filterExperienceInYear;
        private byte _filterAge;
        private ushort _filterYearOfBirth;
        private bool _isFilterTypeAge;
        private bool _isFilterTypeYearOfBirth;
        private readonly HashSet<byte> _experienceYears;
        private readonly HashSet<byte> _ages;
        private readonly HashSet<ushort> _yearsOfBirth;

        private const byte CountExperienceYears = 30;
        private const byte StartWorkAge = 18;
        private const byte EndWorkAge = 100;


    }
}
