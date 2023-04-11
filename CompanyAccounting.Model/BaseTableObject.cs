using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CompanyAccounting.Model
{
    public class BaseTableObject : BaseObject
    {
        public int ID
        {
            get => _id;
            set
            {
                if (_id == value)
                    return;
                _id = value;
                RaisePropertyChanged(nameof(ID));
            }
        }

        private int _id;
    }
}
