using CompanyAccounting.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyAccounting.ViewModel
{
    public class CompanyViewModel : ViewModelBase
    {
        public CompanyViewModel(Company company)   
        { 
            _company = company;
        }

        public string Name
        {
            get => _company.Name;
            set
            { 
                if(_company.Name == value)
                    return;

                _company.Name = value;
                RaisePropertyChanged("Name");
            }
        }

        private Company _company;
    }
}
