﻿using CompanyAccounting.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyAccounting.ViewModel
{
    public class CompanyViewModel : BaseElementViewModel
    {
        public CompanyViewModel(Company company)
            : base(company)
        {
            _company = company;
            _departments = new ObservableCollection<DepartmentViewModel>();
            Init();
        }

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

        public string LegalAddress
        {
            get => _company.LegalAddress;
            set
            {
                if (_company.LegalAddress == value)
                    return;

                _company.LegalAddress = value;
                RaisePropertyChanged(nameof(LegalAddress));
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

        public ObservableCollection<DepartmentViewModel> Departments => _departments;

        internal void Remove(DepartmentViewModel department)
        { 
          Departments?.Remove(department);
        }

        private void Init()
        {
            _loaded = false;
            _isSelected = false;
            _isExpanded = false;
            foreach (var department in _company.Departments)
                Departments.Add(new DepartmentViewModel(_company, department));
            RaisePropertyChanged(() => Departments);
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
            _loaded = true;
        }

        public readonly ObservableCollection<DepartmentViewModel> _departments;
        private readonly Company _company;
        private bool _isSelected;
        private bool _isExpanded;
        private bool _loaded;
    }
}
