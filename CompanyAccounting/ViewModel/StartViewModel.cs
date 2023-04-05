using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ResourceStrings = CompanyAccounting.Strings.Properties.Resources;

namespace CompanyAccounting.ViewModel
{
    public class StartViewModel : ViewModelBase
    {
        public StartViewModel() 
        {
            Task.Factory.StartNew(LoadApplication);
        }

        public string ProductName => Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>()?.Title;

        public string ProductVersion => string.Format(ResourceStrings.VersionTemplate,
                                                      Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version);

        public bool LoadingInProgress
        {
            get => _loadingInProgress;
            private set
            {
                _loadingInProgress = value;
                RaisePropertyChanged(() => LoadingInProgress);
            }
        }

        public string StatusText
        {
            get => _statusText;
            private set 
            { 
                if(_statusText == value)
                    return;
                _statusText = value;
                RaisePropertyChanged(() => StatusText);
            }
        }

        private void LoadApplication()
        {
            for (int i = 0; i < CountTestIteration; i++)
            {
                StatusText = string.Format(ResourceStrings.IterationLoadingTemplate, i+1, CountTestIteration, ResourceStrings.MainLoadingIterationName);
                System.Threading.Thread.Sleep(700);
            }
            LoadingInProgress = false;
        }
        private const int CountTestIteration = 5;

        private string _statusText;
        private bool _loadingInProgress = true;
    }
}
