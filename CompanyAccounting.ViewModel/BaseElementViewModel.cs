using CompanyAccounting.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyAccounting.ViewModel
{
    public class BaseElementViewModel : ViewModelBase
    {
        public BaseElementViewModel(BaseTableObject tableObject)         
        {
            Base = tableObject;
        }
        public int ID => Base.ID;

        public BaseTableObject Base { get; private set; }
    }
}
