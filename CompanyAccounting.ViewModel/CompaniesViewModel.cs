using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
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
        }
        public CompaniesViewModel()
        {

        }

        public readonly string Title;
    }
}
