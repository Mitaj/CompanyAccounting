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
            company.Name = name;
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

        public void SetConnectionString(string connectionString)
        {
            DataBaseAssistant.SetConnectionString(connectionString);
        }

        public ObservableCollection<Company> Companies { get; private set; }
        public ObservableCollection<Employee> Employees { get; private set; }

        private void TableItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!(sender is BaseTableObject tableObject))
                return;

            UpdateData(tableObject);
        }

        private void UpdateData(BaseTableObject tableItem)
        {
            var task = Task.Run(() => SaveChangesAsyncTask(tableItem));
            task.Wait();
            RaisePropertyChanged(nameof(Companies));
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

                await context.JobInformations.LoadAsync();
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
    }
}
