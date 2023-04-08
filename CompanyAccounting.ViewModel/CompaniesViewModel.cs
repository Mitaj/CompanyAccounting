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
            _companies = new ObservableCollection<CompanyViewModel>();
            RefreshCompanies();
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

        public void RefreshCompanies()
        {
            _companies.Clear();
            var loadedCompanyModels = ViewModelLocator.Instance.IoC.GetInstance<ModelAssistant>().Companies;
            if (loadedCompanyModels == null || loadedCompanyModels.Count == 0)
            {
                RaisePropertyChanged(nameof(Companies));
                return;
            }
            foreach ( var company in loadedCompanyModels) 
            {
                _companies.Add(new CompanyViewModel(company));
            }

            RaisePropertyChanged(nameof(Companies));
        }

        public ObservableCollection<CompanyViewModel> Companies => _companies;

        private readonly ObservableCollection<CompanyViewModel> _companies;

        private string _title;
        private ViewModelBase _selectedItem;

    }
}
