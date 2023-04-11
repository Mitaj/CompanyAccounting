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
    public class CompaniesViewModel : ViewModelBase
    {
        public CompaniesViewModel(string title)
        { 
            Title = title;
            Companies = new ObservableCollection<CompanyViewModel>();
            LoadCompanies();
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

        private void RefreshSelectedObject()
        {
            if (_selectedItem is CompanyViewModel)
            { 
            
            }
        }

        public void LoadCompanies()
        {
            Companies.Clear();
            var loadedCompanyModels = ViewModelLocator.Instance.IoC.GetInstance<ModelAssistant>().Companies;
            if (loadedCompanyModels == null || loadedCompanyModels.Count == 0)
            {
                RaisePropertyChanged(nameof(Companies));
                return;
            }

            foreach ( var company in loadedCompanyModels) 
            {
                var companyVM = new CompanyViewModel(company);
                companyVM.Departments.Clear();
                foreach (var department in company.Departments)
                    companyVM.Departments.Add(new DepartmentViewModel(company, department));

                Companies.Add(companyVM);
            }

            RaisePropertyChanged(nameof(Companies));
        }

        public readonly ObservableCollection<CompanyViewModel> Companies;
        private string _title;
        private ViewModelBase _selectedItem;

    }
}
