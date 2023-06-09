﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyAccounting.Model
{
    public class Employee : BaseTableObject
    {
        public Employee() 
        {
          
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                if (_firstName == value)
                    return;
                _firstName = value;
                RaisePropertyChanged(nameof(FirstName));
            }
        }

        public string SecondName
        {
            get => _secondName;
            set
            {
                if (_secondName == value)
                    return;
                _secondName = value;
                RaisePropertyChanged(nameof(SecondName));
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                if (_lastName == value)
                    return;
                _lastName = value;
                RaisePropertyChanged(nameof(LastName));
            }
        }

        public DateTime Birthday
        { 
            get => _birthday;
            set
            {
                if (_birthday == value)
                    return;
                _birthday = value;
                RaisePropertyChanged(nameof(Birthday));
            }
        }

        

        private string _firstName;
        private string _secondName;
        private string _lastName;
        private DateTime _birthday;
    }
}
