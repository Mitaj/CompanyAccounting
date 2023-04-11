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
            //DBAssistant = new DataBaseAssistant();
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
                var loadDepartmentsTask = Task.Run(() => LoadDepartmentsAsyncTask(company));
                loadDepartmentsTask.Wait();
            }
        }

        public void LoadEmployees(Company company)
        {
            if (company == null || company.Departments.Count == 0)
                return;

            foreach (var department in company.Departments)
            {
                var loadEmployeesTask = Task.Run(() => LoadEmployeesAsyncTask(department));
                loadEmployeesTask.Wait();
            }      
        }

        private static async Task LoadCompaniesAsyncTask(ModelAssistant assistant)
        {
            if(assistant.Companies == null)
                assistant.Companies = new ObservableCollection<Company>();

            using (var context = new DataBaseAssistant())
            {
                assistant.Companies.Clear();
                await context.Companies.LoadAsync();
                foreach (var company in context.Companies.AsEnumerable())
                {
                    company.PropertyChanged += assistant.Company_PropertyChanged;
                    assistant.Companies.Add(company);
                }
            }
        }

        private static async Task LoadDepartmentsAsyncTask(Company company)
        {
            using (var context = new DataBaseAssistant())
            {
                company.Departments.Clear();
                var loadedDepartments = await context.Departments
                                       .Where(d => d.CompanyID == company.ID)
                                       .ToListAsync();

                company.SetDepartments(loadedDepartments);
            }
        }

        private static async Task LoadEmployeesAsyncTask(Department department)
        {
            using (var context = new DataBaseAssistant())
            {
                department.Employees.Clear();
                var loadedWorkBookEntries = await context.WorkbookEntries
                                            .Where(w => w.DepartmentID == department.ID)
                                            .ToListAsync();

                if (loadedWorkBookEntries == null)
                    return;
                foreach (var entry in loadedWorkBookEntries)
                {
                    var loadedEmployees = await context.Employees
                                          .Where(e => e.ID == department.ID)
                                          .ToListAsync();
                    if (loadedEmployees == null)
                        continue;

                    department.SetEmployees(loadedEmployees);
                }

                foreach (var employee in department.Employees)
                {
                    var loadedWorkbookEntries = await context.WorkbookEntries
                                                .Where(w => w.EmployeeID == employee.ID)
                                                .OrderBy(w => w.DateEmployment)
                                                .ToListAsync();

                    if (loadedWorkbookEntries == null)
                        continue;

                    employee.SetWorkbookEntries(loadedWorkbookEntries);
                }

                await context.JobInformations.LoadAsync();
            }
        }

        private static async Task<Company> SaveChangesAsyncTask(Company company)
        {
            using (var context = new DataBaseAssistant())
            {
                context.Entry(company).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return company;
            }
        }

        public void SetConnectionString(string connectionString)
        {
            DataBaseAssistant.SetConnectionString(connectionString);
        }

        public ObservableCollection<Company> Companies { get; private set; }
        

        private void Company_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!(sender is Company company))
                return;

            UpdateData(company);
        }

        private void UpdateData(Company company)
        {
            var task = Task.Run(() => SaveChangesAsyncTask(company));
            task.Wait();
            RaisePropertyChanged(nameof(Companies));
        }
    }
}
