using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyAccounting.Model
{
    public class Company : BaseObject
    {
        public Company() { }

        public int ID
        {
            get => _id;
            set
            {
                if (_id == value)
                    return;
                _id = value;
                RaisePropertyChanged("ID");
            }
        }
        public string Name
        {
            get => _name;
            set 
            {
                if (_name == value)
                    return;
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        public DateTime DateCreation 
        { 
            get => _dateCreation;
            set 
            {
                if (_dateCreation == value)
                    return;
                _dateCreation = value;
                RaisePropertyChanged("DateCreation");
            }   
        }

        private int _id;
        private string _name;
        private DateTime _dateCreation;
    }
}
