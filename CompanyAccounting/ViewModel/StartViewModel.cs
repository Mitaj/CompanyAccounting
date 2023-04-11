using CompanyAccounting.Model;
using CompanyAccounting.ViewModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ResourceStrings = CompanyAccounting.Strings.Properties.Resources;

namespace CompanyAccounting
{
    public class StartViewModel : ViewModelBase
    {
        public StartViewModel() 
        {
            LoadingOperations = new LoadingOperation[] { LoadLocator, LoadFirstData, LoadSecondData };
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

        public ICommand ShowCompaniesViewCommand => _showCompaniesViewCommand ?? (_showCompaniesViewCommand = new RelayCommand(ShowCompaniesAccounting));
 

        private View.CompaniesWindow CompaniesView => _companiesView ?? (_companiesView = LoadCompaniesAccounting());

        private void LoadApplication()
        {
            if (LoadingOperations == null || LoadingOperations.Length == 0)
            {
                Application.Current.Shutdown();
                return;
            }

            for (int i = 0; i < LoadingOperations.Length; i++)
            {
                System.Threading.Thread.Sleep(700);
                StatusText = string.Format(ResourceStrings.IterationLoadingTemplate, i + 1, LoadingOperations.Length, ResourceStrings.MainLoadingIterationName);
                if (LoadingOperations[i] != null)
                    LoadingOperations[i]();
            }

            LoadingInProgress = false;
        }

        private void LoadLocator()
        {
            var ioC = ViewModelLocator.Instance.IoC;
            ViewModelLocator.Instance.LoadInstances(ProductName);
        }

        private void LoadFirstData()
        {
            var modelAssistance = ViewModelLocator.Instance.IoC.GetInstance<ModelAssistant>();
            modelAssistance?.LoadCompanies();
        }

        private void LoadSecondData()
        {
            var modelAssistance = ViewModelLocator.Instance.IoC.GetInstance<ModelAssistant>();
            modelAssistance?.LoadDepartments();
        }

        private void ShowCompaniesAccounting()
        {
            try
            {
                CompaniesView.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private View.CompaniesWindow LoadCompaniesAccounting()
        {
            var viewModel = ViewModelLocator.Instance.Companies;
            var viewCompanies = new View.CompaniesWindow();
            viewCompanies.Width = MainView_Width;
            viewCompanies.Height = MainView_Height;
            viewCompanies. DataContext = viewModel;
            viewCompanies.Closed += CompaniesView_Closed;
            viewCompanies.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            return viewCompanies;
        }

        void CompaniesView_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private delegate void LoadingOperation();
        private LoadingOperation[] LoadingOperations;

        private ICommand _showCompaniesViewCommand;
        private View.CompaniesWindow _companiesView;
        private string _statusText;
        private bool _loadingInProgress = true;

        private const int MainView_Width = 1024;
        private const int MainView_Height = 768;
    }
}
