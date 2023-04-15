using CommonServiceLocator;
using CompanyAccounting.Model;
using CompanyAccounting.Model.RegistryData;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CompanyAccounting.ViewModel
{
    public sealed class ViewModelLocator
    {
        private static readonly Lazy<ViewModelLocator> lazy =
           new Lazy<ViewModelLocator>(() => new ViewModelLocator());

        public static ViewModelLocator Instance { get { return lazy.Value; } }

        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// Step 1
        /// </summary>
        public ViewModelLocator()
        {
            Init();
        }

        public CompaniesViewModel Companies => CurrentLocator.GetInstance<CompaniesViewModel>();

        public IServiceLocator CurrentLocator => ServiceLocator.Current;

        public static IMessenger MessengerInstance => Messenger.Default;
        
        public ISimpleIoc IoC => SimpleIoc.Default;

        public void LoadInstances(string productName)
        {
            var modelAssistance = new ModelAssistant();
            var savedConnectionString = RegistryAssistance.GetRegistryParamValue(RegistryParam.ConnectionString);
            modelAssistance.SetConnectionString(savedConnectionString);
            IoC.Register(() => modelAssistance);
            IoC.Register(() => new CompaniesViewModel(productName));
        }

        private void Init()
        {
            ServiceLocator.SetLocatorProvider(() => IoC);
        }

        private const string TestConnectionString = "Host=localhost;Port=5432;Database=company_accounting;Username=user_db;Password=123";
    }
}
