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
    public class CompanyViewModel : ViewModelBase
    {
        public CompanyViewModel(Company company)   
        {
            _company = company;
            Departments = new ObservableCollection<DepartmentViewModel>();
            Init();
        }

        public int ID => _company.ID;

        public string Name
        {
            get => _company.Name;
            set
            { 
                if(_company.Name == value)
                    return;

                _company.Name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        public DateTime DateCreation
        { 
            get => _company.DateCreation;
            set
            {
                if (_company.DateCreation == value)
                    return;

                _company.DateCreation = value;
                RaisePropertyChanged(nameof(DateCreation));
            }
        }

        public bool IsExpanded 
        {
            get => _isExpanded;
            set 
            {
                if (_isExpanded == value)
                    return;

                _isExpanded = value;
                LoadIfNeedEmployees();
                RaisePropertyChanged(nameof(IsExpanded));
            }
        }        
        
        public bool IsSelected 
        {
            get => _isSelected;
            set 
            {
                if (_isSelected == value)
                    return;

                _isSelected = value;
                LoadIfNeedEmployees();
                RaisePropertyChanged(nameof(IsSelected));
            }
        }

        public readonly ObservableCollection<DepartmentViewModel> Departments;

        private void Init()
        {
            _loaded = false;
            foreach (var department in _company.Departments)
                Departments.Add(new DepartmentViewModel(_company, department));
            RaisePropertyChanged(() => Departments);

            LoadIfNeedEmployees();
        }

        private void LoadIfNeedEmployees()
        {
            if (_loaded)
                return;
            ViewModelLocator.Instance.IoC.GetInstance<ModelAssistant>().LoadEmployees(_company);
            foreach(var department in Departments)
            {
                department.LoadEmployees();
            }
        }

        private readonly Company _company;

        private bool _isExpanded;
        private bool _isSelected;
        private bool _loaded;
    }
}
