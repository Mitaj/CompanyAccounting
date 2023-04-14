using CompanyAccounting.Model.DataBaseComponents;
using CompanyAccounting.Strings.Properties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CompanyAccounting.Model
{
    public class ModelAssistant : BaseObject
    {
        public ModelAssistant()
        {
            Companies = new ObservableCollection<Company>();
            Employees = new ObservableCollection<Employee>();
            JobInformations = new ObservableCollection<JobInformation>();
        }

        public void LoadCompanies()
        {
            var loadCompaniesTask = Task.Run(() => LoadCompaniesAsyncTask(this));
            loadCompaniesTask.Wait();
            RaisePropertyChanged(nameof(Companies));
        }

        public void LoadDepartments()
        {
            if (Companies == null || Companies.Count == 0)
                return;

            foreach (var company in Companies)
            {
                var loadDepartmentsTask = Task.Run(() => LoadDepartmentsAsyncTask(this, company));
                loadDepartmentsTask.Wait();
            }
        }

        public void LoadEmployees(Company company)
        {
            if (company == null || company.Departments.Count == 0)
                return;

            foreach (var department in company.Departments)
                LoadEmployees(department);
        }

        public void LoadEmployees(Department department)
        {
            if (department == null)
                return;

            var loadEmployeesTask = Task.Run(() => LoadEmployeesAsyncTask(this, department));
            loadEmployeesTask.Wait();
        }

        public Company AddCompany(string name, DateTime dateCreation)
        {
            var company = new Company();
            company.Name = name?.Trim();
            company.DateCreation = dateCreation;
            try
            {
                var task = Task.Run(() => AddTableItemAsyncTask(this, company));
                task.Wait();
            }
            catch (ArgumentNullException)
            {
                return null;
            }
            catch (AggregateException)
            {
                return null;
            }
            catch (ObjectDisposedException)
            {
                return null;
            }
            return company;
        }

        public Department AddDepartment(string name, Company company)
        { 
            if(company == null)
                return null;
            var department = new Department();
            department.Name = name?.Trim();
            department.SupervisorID = -1;
            department.CompanyID = company.ID;
            company.Departments.Add(department);
            try
            {
                var task = Task.Run(() => AddTableItemAsyncTask(this, department));
                task.Wait();
            }
            catch (ArgumentNullException)
            {
                return null;
            }
            catch (AggregateException)
            {
                return null;
            }
            catch (ObjectDisposedException)
            {
                return null;
            }
            return department;
        }
        public Employee AddEmployee(string firstName, string secondName, string lastName, DateTime birthday)
        { 
            var employee = new Employee();
            employee.FirstName = firstName?.Trim();
            employee.SecondName = secondName?.Trim();
            employee.LastName = lastName?.Trim();
            employee.Birthday = birthday;

            try
            {
                var task = Task.Run(() => AddTableItemAsyncTask(this, employee));
                task.Wait();
            }
            catch (ArgumentNullException)
            {
                return null;
            }
            catch (AggregateException)
            {
                return null;
            }
            catch (ObjectDisposedException)
            {
                return null;
            }
            return employee;
        }

        public JobInformation GetJobInformation(int id)
        {
            return JobInformations?.FirstOrDefault(j => j.ID == id);
        }

        public JobInformation GetExistJobInformation(string positionName, int salary)
        {
            try
            {
                var task = Task.Run(() => GetExistJobInformationAsyncTask(positionName, salary));
                task.Wait();
            }
            catch (ArgumentNullException)
            {
                return null;
            }
            catch (AggregateException)
            {
                return null;
            }
            catch (ObjectDisposedException)
            {
                return null;
            }
            return null;
        }
        
        public JobInformation AddJobInformation(string positionName, int salary) 
        {
            var jobInformation = new JobInformation();
            jobInformation.PositionName = positionName;
            jobInformation.SalaryDollars = salary;
            JobInformations.Add(jobInformation);
            try
            {
                var task = Task.Run(() => AddTableItemAsyncTask(this, jobInformation));
                task.Wait();
            }
            catch (ArgumentNullException)
            {
                return null;
            }
            catch (AggregateException)
            {
                return null;
            }
            catch (ObjectDisposedException)
            {
                return null;
            }
            return jobInformation;
        }
        
        public WorkbookEntry AddWorkbookEntry(Employee employee, DateTime dateEmployment, Department department, JobInformation jobInformation)
        {
            if (employee == null || department == null || jobInformation == null)
                return null;
            var entry = new WorkbookEntry();
            entry.DateEmployment = dateEmployment;
            entry.EmployeeID = employee.ID;
            entry.DepartmentID = department.ID;
            entry.JobInformationID = jobInformation.ID;
            department.AddWorkbookEntry(entry);

            try
            {
                var task = Task.Run(() => AddTableItemAsyncTask(this, entry));
                task.Wait();
            }
            catch (ArgumentNullException)
            {
                return null;
            }
            catch (AggregateException)
            {
                return null;
            }
            catch (ObjectDisposedException)
            {
                return null;
            }
            return entry;
        }



        public void DeleteTableItem(BaseTableObject tableObject)
        {
            try
            {
                var task = Task.Run(() => DeleteTableItemAsyncTask(tableObject));
                task.Wait();
            }
            catch (ArgumentNullException) { }
            catch (AggregateException) { }
            catch (ObjectDisposedException) { }
        }

        public void SetConnectionString(string connectionString)
        {
            DataBaseAssistant.SetConnectionString(connectionString);
        }


        public ObservableCollection<Company> Companies { get; private set; }
        public ObservableCollection<Employee> Employees { get; private set; }
        public ObservableCollection<JobInformation> JobInformations { get; private set; }

        public void UpdateData(BaseTableObject tableItem)
        {
            var task = Task.Run(() => SaveChangesAsyncTask(tableItem));
            task.Wait();
            RaisePropertyChanged(nameof(Companies));
        }

        private void TableItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!(sender is BaseTableObject tableObject))
                return;

            UpdateData(tableObject);
        }

        private static async Task LoadCompaniesAsyncTask(ModelAssistant assistant)
        {
            if (assistant.Companies == null)
                assistant.Companies = new ObservableCollection<Company>();

            using (var context = new DataBaseAssistant())
            {
                if(assistant.Companies != null)
                    foreach(var company in assistant.Companies)
                        company.PropertyChanged -= assistant.TableItem_PropertyChanged;
                assistant.Companies.Clear();
                await context.Companies.LoadAsync();
                foreach (var company in context.Companies.AsEnumerable())
                {
                    company.PropertyChanged += assistant.TableItem_PropertyChanged;
                    assistant.Companies.Add(company);
                }
            }
        }

        private static async Task LoadDepartmentsAsyncTask(ModelAssistant assistant, Company company)
        {
            using (var context = new DataBaseAssistant())
            {
                if(company.Departments != null)
                    foreach(var department in company.Departments)
                        department.PropertyChanged -= assistant.TableItem_PropertyChanged;
                company.Departments.Clear();

                var loadedDepartments = await context.Departments
                                       .Where(d => d.CompanyID == company.ID)
                                       .ToListAsync();
                if (loadedDepartments != null)
                    foreach (var department in loadedDepartments)
                    {
                        department.PropertyChanged += assistant.TableItem_PropertyChanged;
                    }
                company.SetDepartments(loadedDepartments);
            }
        }

        private static async Task LoadEmployeesAsyncTask(ModelAssistant assistant, Department department)
        {
            using (var context = new DataBaseAssistant())
            {
                if (department.WorkbookEntries != null)
                    foreach (var entry in department.WorkbookEntries)
                        entry.PropertyChanged -= assistant.TableItem_PropertyChanged;
                department.WorkbookEntries.Clear();
                var loadedWorkBookEntries = await context.WorkbookEntries
                                            .Where(w => w.DepartmentID == department.ID)
                                            .ToListAsync();

                if (loadedWorkBookEntries == null)
                    return;

                foreach(var entry in loadedWorkBookEntries)
                    entry.PropertyChanged += assistant.TableItem_PropertyChanged;
                department.SetWorkbookEntries(loadedWorkBookEntries);
                foreach (var entry in loadedWorkBookEntries)
                {
                    var employee = await context.Employees
                                   .Where(e => e.ID == entry.EmployeeID)
                                   .SingleOrDefaultAsync();
                    if (employee == null || assistant.Employees.Any(e => e.ID == employee.ID))
                        continue;

                    employee.PropertyChanged += assistant.TableItem_PropertyChanged;
                    assistant.Employees.Add(employee);
                }

                if (assistant.JobInformations != null && assistant.JobInformations.Count != 0)
                    return;

                var loadedJobInformations = await context.JobInformations.ToListAsync();
                if (loadedJobInformations == null)
                    return;

                foreach (var info in loadedJobInformations)
                    assistant.JobInformations.Add(info);
            }
        }

        private static async Task<BaseTableObject> SaveChangesAsyncTask(BaseTableObject tableItem)
        {
            using (var context = new DataBaseAssistant())
            {
                context.Entry(tableItem).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return tableItem;
            }
        }

        private static async Task<JobInformation> GetExistJobInformationAsyncTask(string positionName, int salary)
        { 
            using(var context = new DataBaseAssistant()) 
            {
                return await context.JobInformations.FirstOrDefaultAsync(i => i.SalaryDollars == salary && i.PositionName.ToUpper() == positionName.Trim().ToUpper());
            }
        }

        private static async Task<BaseTableObject> AddTableItemAsyncTask(ModelAssistant assistant, BaseTableObject tableItem)
        {
            using (var context = new DataBaseAssistant())
            {
                await context.AddRangeAsync(tableItem);
                await context.SaveChangesAsync();
                if (tableItem is Company company)
                {
                    company.PropertyChanged += assistant.TableItem_PropertyChanged;
                    assistant.Companies.Add(company);
                }
                return tableItem;
            }
        }

        private static async Task DeleteTableItemAsyncTask(BaseTableObject tableItem)
        {
            using (var context = new DataBaseAssistant())
            {
                context.Entry(tableItem).State = EntityState.Deleted;
                await context.SaveChangesAsync();
            }
        }
    }
}
