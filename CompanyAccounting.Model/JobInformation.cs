using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CompanyAccounting.Model
{
    public class JobInformation : BaseTableObject
    {
        public string PositionName
        {
            get => _positionName;
            set
            {
                if (_positionName == value)
                    return;
                _positionName = value;
                RaisePropertyChanged(nameof(PositionName));
            }
        }

        public int SalaryDollars
        {
            get => _salaryDollars;
            set
            {
                if (_salaryDollars == value)
                    return;
                _salaryDollars = value;
                RaisePropertyChanged(nameof(SalaryDollars));
            }
        }

        private string _positionName;
        private int _salaryDollars;
    }
}
