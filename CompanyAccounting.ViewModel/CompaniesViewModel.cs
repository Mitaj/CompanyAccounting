using CompanyAccounting.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CompanyAccounting.ViewModel
{
    public class CompaniesViewModel : ViewModelBase
    {
        public CompaniesViewModel(string title)
        { 
            Title = title;
            _companies = new ObservableCollection<CompanyViewModel>();
            LoadCompanies();
            AddCompanyPanelVisible = false;
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

        public ViewModelBase SelectedItem
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
            var loadedCompanyModels = ViewModelLocator.Instance.IoC.GetInstance<ModelAssistant>().Companies;
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

        public bool ControlPanelVisible => !AddCompanyPanelVisible;
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


        public ICommand AddCompanyCommand => _addCompanyCommand ?? (_addCompanyCommand = new RelayCommand(AddCompany));
        public ICommand ApplyAddCompanyCommand => _applyAddCompanyCommand ?? (_applyAddCompanyCommand = new RelayCommand(ApplyAddCompany));
        public ICommand CancelAddCompanyCommand => _cancelAddCompanyCommand ?? (_cancelAddCompanyCommand = new RelayCommand(CancelAddCompany));
        public ICommand AddItemCommand => _addItemCommand ?? (_addItemCommand = new RelayCommand(AddItem));
        public ICommand DeleteItemCommand => _deleteItemCommand ?? (_deleteItemCommand = new RelayCommand(DeleteItem));


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
            var addedCompany = ViewModelLocator.Instance.IoC.GetInstance<ModelAssistant>().AddCompany(AddCompanyName, AddCompanyDateCreation);
            LoadCompany(addedCompany);
            RaisePropertyChanged(nameof(Companies));
            AddCompanyPanelVisible = false;
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

        private void CancelAddCompany()
        {
            AddCompanyPanelVisible = false;
        }

        private void AddItem()
        { 
        
        }

        private void DeleteItem()
        {

        }

        private const string CompanyNameTemplate = "Компания {0}";

        private readonly ObservableCollection<CompanyViewModel> _companies;

        private string _title;
        private ViewModelBase _selectedItem;
        private bool _addCompanyPanelVisible;
        private string _addCompanyName;
        private DateTime _addCompanyDateCreation;

        private ICommand _addCompanyCommand;
        private ICommand _applyAddCompanyCommand;
        private ICommand _cancelAddCompanyCommand;
        private ICommand _addItemCommand;
        private ICommand _deleteItemCommand;
    }
}
