using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CompanyAccounting.Model
{
    public class Department : BaseTableObject
    {
        public Department() 
        {
            _workbookEntries = new ObservableCollection<WorkbookEntry>();
        }

        public int CompanyID
        {
            get => _company_id;
            set
            {
                if (_company_id == value)
                    return;
                _company_id = value;
                RaisePropertyChanged(nameof(CompanyID));
            }
        }

        public int SupervisorID
        {
            get => _supervisor_id;
            set
            {
                if (_supervisor_id == value)
                    return;
                _supervisor_id = value;
                RaisePropertyChanged(nameof(SupervisorID));
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
                RaisePropertyChanged(nameof(Name));
            }
        }

        public ObservableCollection<WorkbookEntry> WorkbookEntries => _workbookEntries;

        internal void SetWorkbookEntries(IEnumerable<WorkbookEntry> entries)
        {
            _workbookEntries.Clear();
            if (_workbookEntries == null)
                return;

            foreach (var entry in entries)
                _workbookEntries.Add(entry);
            RaisePropertyChanged(nameof(WorkbookEntries));
        }

        public readonly ObservableCollection<WorkbookEntry> _workbookEntries;
        private int _company_id;
        private int _supervisor_id;
        private string _name;
    }
}
