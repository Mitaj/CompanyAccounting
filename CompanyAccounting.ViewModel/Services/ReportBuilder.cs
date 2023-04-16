using CompanyAccounting.Model;
using GalaSoft.MvvmLight.Command;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace CompanyAccounting.ViewModel.Services
{
    public struct FilterListOfEmployees
    {
        public int CompanyID;
        public byte ExperienceInYears;
        public ushort YearOfBirth;
        public byte Age;
        public bool UseAgeType;
        public DateTime RequestDate;
    }
    public class ReportBuilder
    {
        public ReportBuilder() { }

        public ICommand BuildPayrollReportCommand => _buildPayrollReportCommand ?? (_buildPayrollReportCommand = new RelayCommand(BuildPayrollReport));
        public ICommand BuildListOfEmployeesReportCommand => _buildListOfEmployeesReportCommand ?? (_buildListOfEmployeesReportCommand = new RelayCommand<FilterListOfEmployees>(BuildListOfEmployeesReport));

        private void BuildPayrollCompanies(Worksheet mainSheet)
        {
            if (Model.Companies == null)
                return;

            Range sheetRange = mainSheet.UsedRange;
            Range findedMainCompanyTemplate = sheetRange.Find(TemplateCompanyCompanyName);
            if (findedMainCompanyTemplate == null)
                return;


            foreach (var company in Model.Companies)
            {
                if (company.Departments == null || company.Departments.Count == 0)
                    continue;

                var salarySumm = 0;
                foreach (var department in company.Departments)
                {
                    foreach (var entry in department.WorkbookEntries)
                    {
                        var employee = Model.Employees.FirstOrDefault(e => e.ID == entry.EmployeeID);
                        var jobInformation = Model.JobInformations.FirstOrDefault(j => j.ID == entry.JobInformationID);
                        if (employee == null || jobInformation == null)
                            continue;

                        salarySumm += jobInformation.SalaryDollars;
                    }
                }

                findedMainCompanyTemplate.EntireRow.Insert(XlInsertShiftDirection.xlShiftDown);
                var addedRow = mainSheet.Rows[findedMainCompanyTemplate.EntireRow.Row - 1];
                findedMainCompanyTemplate.EntireRow.Copy(addedRow.EntireRow);

                Range companyRange = addedRow.Find(TemplateCompanyCompanyName);
                if (companyRange != null)
                    companyRange.Value = company.Name;

                Range salary = addedRow.Find(TemplateCompanySalary);
                if (salary != null)
                    salary.Value = salarySumm;
            }

            do
            {
                if (findedMainCompanyTemplate != null)
                    findedMainCompanyTemplate.EntireRow.Delete();
                findedMainCompanyTemplate = sheetRange.Find(TemplateCompanyCompanyName);
            }
            while (findedMainCompanyTemplate != null);
        }

        private void BuildPayrollDepartments(Worksheet mainSheet) 
        {
            if (Model.Companies == null)
                return;

            Range sheetRange = mainSheet.UsedRange;
            Range findedMainCompanyTemplate = sheetRange.Find(TemplateDepartmentCompanyName);
            if (findedMainCompanyTemplate == null)
                return;


            foreach (var company in Model.Companies)
            {
                if (company.Departments == null)
                    continue;

                foreach (var department in company.Departments)
                {
                    var salarySumm = 0;
                    foreach (var entry in department.WorkbookEntries)
                    {
                        var employee = Model.Employees.FirstOrDefault(e => e.ID == entry.EmployeeID);
                        var jobInformation = Model.JobInformations.FirstOrDefault(j => j.ID == entry.JobInformationID);
                        if (employee == null || jobInformation == null)
                            continue;

                        salarySumm += jobInformation.SalaryDollars;
                    }
                    findedMainCompanyTemplate.EntireRow.Insert(XlInsertShiftDirection.xlShiftDown);
                    var addedRow = mainSheet.Rows[findedMainCompanyTemplate.EntireRow.Row - 1];
                    findedMainCompanyTemplate.EntireRow.Copy(addedRow.EntireRow);

                    Range companyRange = addedRow.Find(TemplateDepartmentCompanyName);
                    if (companyRange != null)
                        companyRange.Value = company.Name;

                    Range departmentRange = addedRow.Find(TemplateDepartmentDepartmentName);
                    if (departmentRange != null)
                        departmentRange.Value = department.Name;

                    Range salary = addedRow.Find(TemplateDepartmentSalary);
                    if (salary != null)
                        salary.Value = salarySumm;
                }
            }

            do
            {
                if (findedMainCompanyTemplate != null)
                    findedMainCompanyTemplate.EntireRow.Delete();
                findedMainCompanyTemplate = sheetRange.Find(TemplateDepartmentCompanyName);
            }
            while (findedMainCompanyTemplate != null);
        }

        private void BuildPayrollEmployees(Worksheet mainSheet) 
        {
            if (Model.Companies == null)
                    return;

            Range sheetRange = mainSheet.UsedRange;
            Range findedMainCompanyTemplate = sheetRange.Find(TemplateMainCompanyName);
            if (findedMainCompanyTemplate == null)
                return;

            
            foreach (var company in Model.Companies)
            {
                if (company.Departments == null)
                    continue;

                foreach (var department in company.Departments)
                {
                    foreach (var entry in department.WorkbookEntries)
                    {
                        var employee = Model.Employees.FirstOrDefault(e => e.ID == entry.EmployeeID);
                        var jobInformation = Model.JobInformations.FirstOrDefault(j => j.ID == entry.JobInformationID);
                        if (employee == null || jobInformation == null)
                            continue;

                        findedMainCompanyTemplate.EntireRow.Insert(XlInsertShiftDirection.xlShiftDown);
                        var addedRow = mainSheet.Rows[findedMainCompanyTemplate.EntireRow.Row - 1];
                        findedMainCompanyTemplate.EntireRow.Copy(addedRow.EntireRow);

                        Range companyRange = addedRow.Find(TemplateMainCompanyName);
                        if (companyRange != null)
                            companyRange.Value = company.Name;

                        Range employeeRange = addedRow.Find(TemplateMainEmployeeName);
                        if (employeeRange != null)
                            employeeRange.Value = $"{employee.LastName} {employee.FirstName} {employee.SecondName}";

                        Range departmentRange = addedRow.Find(TemplateMainDepartmentName);
                        if (departmentRange != null)
                            departmentRange.Value = department.Name;

                        Range salary = addedRow.Find(TemplateMainSalary);
                        if(salary != null)
                            salary.Value = jobInformation.SalaryDollars;
                    }
                }
            }

            do
            {
                if (findedMainCompanyTemplate != null)
                    findedMainCompanyTemplate.EntireRow.Delete();
                findedMainCompanyTemplate = sheetRange.Find(TemplateMainCompanyName);
            }
            while (findedMainCompanyTemplate != null);
        }

        private void BuildPayrollReport()
        {
            var templateFilePath = Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath),
                                                PayrollTemplateFilePath);

            if (!File.Exists(templateFilePath))
                return;

            var reportFilePath = GetFilePathReport();
            if (string.IsNullOrWhiteSpace(reportFilePath))
                return;

            var Excel = new Microsoft.Office.Interop.Excel.Application();
            if (Excel == null)
                return;
            try
            {
                var reportWorkbook = Excel.Application.Workbooks.Open(templateFilePath);
                if (reportWorkbook.Sheets.Count == 0)
                    return;
                Worksheet workSheet = reportWorkbook.Sheets[1];

                BuildPayrollEmployees(workSheet);
                BuildPayrollDepartments(workSheet);
                BuildPayrollCompanies(workSheet);

                reportWorkbook.SaveAs(reportFilePath);
                reportWorkbook.Close();
            }
            finally { Excel.Quit(); }
        }

        private void BuildListOfEmployees(Worksheet mainSheet, FilterListOfEmployees filter)
        {
            Range sheetRange = mainSheet.UsedRange;
            Range findedDateCreationTemplate = sheetRange.Find(TemplateDateCreation);
            if (findedDateCreationTemplate != null)
                findedDateCreationTemplate.Value = filter.RequestDate.ToString("dd.MM.yyyy");

            Range findedMainCompanyTemplate = sheetRange.Find(TemplateMainCompanyName);
            if (findedMainCompanyTemplate == null)
                return;

            try
            {
                if (Model.Companies == null)
                    return;

                var companyModel = Model.Companies.FirstOrDefault(c => c.ID == filter.CompanyID);
                if (companyModel == null)
                    return;

                if (companyModel.Departments == null)
                    return;

                foreach (var department in companyModel.Departments)
                {
                    foreach (var entry in department.WorkbookEntries)
                    {
                        var employee = Model.Employees.FirstOrDefault(e => e.ID == entry.EmployeeID);
                        var jobInformation = Model.JobInformations.FirstOrDefault(j => j.ID == entry.JobInformationID);

                        if (employee == null || jobInformation == null)
                            continue;

                        var totalExperienceYears = (byte)((filter.RequestDate - entry.DateEmployment).TotalDays / CountDaysInYear);
                        var ageInYears = (byte)((filter.RequestDate - employee.Birthday).TotalDays / CountDaysInYear);
                        if (totalExperienceYears != filter.ExperienceInYears || 
                            filter.UseAgeType && ageInYears != filter.Age ||
                            !filter.UseAgeType && employee.Birthday.Year != filter.YearOfBirth)
                            continue;

                        findedMainCompanyTemplate.EntireRow.Insert(XlInsertShiftDirection.xlShiftDown);
                        var addedRow = mainSheet.Rows[findedMainCompanyTemplate.EntireRow.Row - 1];
                        findedMainCompanyTemplate.EntireRow.Copy(addedRow.EntireRow);

                        Range companyRange = addedRow.Find(TemplateMainCompanyName);
                        if (companyRange != null)
                            companyRange.Value = companyModel.Name;

                        Range employeeRange = addedRow.Find(TemplateMainEmployeeName);
                        if (employeeRange != null)
                            employeeRange.Value = $"{employee.LastName} {employee.FirstName} {employee.SecondName}";

                        Range departmentRange = addedRow.Find(TemplateMainDepartmentName);
                        if (departmentRange != null)
                            departmentRange.Value = department.Name;

                        Range experienceRange = addedRow.Find(TemplateMainExperience);
                        if (experienceRange != null)
                            experienceRange.Value = $"{totalExperienceYears} лет";

                        
                        Range ageRange = addedRow.Find(TemplateMainAge);
                        if (ageRange != null)
                            ageRange.Value = $"{ageInYears} лет";
                    }
                }
            }
            finally
            {
                do
                {
                    if (findedMainCompanyTemplate != null)
                        findedMainCompanyTemplate.EntireRow.Delete();
                    findedMainCompanyTemplate = sheetRange.Find(TemplateMainCompanyName);
                }
                while (findedMainCompanyTemplate != null);
            }
        }

        private void BuildListOfEmployeesReport(FilterListOfEmployees filter)
        {
            var templateFilePath = Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath),
                                                ListOfEmployeesTemplateFilePath);

            if (!File.Exists(templateFilePath))
                return;

            var reportFilePath = GetFilePathReport();
            if (string.IsNullOrWhiteSpace(reportFilePath))
                return;

            var Excel = new Microsoft.Office.Interop.Excel.Application();
            if (Excel == null)
                return;

            try
            {
                var reportWorkbook = Excel.Application.Workbooks.Open(templateFilePath);
                if (reportWorkbook.Sheets.Count == 0)
                    return;
                Worksheet workSheet = reportWorkbook.Sheets[1];

                BuildListOfEmployees(workSheet, filter);

                reportWorkbook.SaveAs(reportFilePath);
                reportWorkbook.Close();
            }
            finally { Excel.Quit(); }

        }

        private ModelAssistant Model => ViewModelLocator.Instance.IoC.GetInstance<ModelAssistant>();

        private static string GetFilePathReport()
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Отчеты (*.xlsx)|*.xlsx";
            saveFileDialog.FilterIndex = 1;
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return string.Empty;

            return saveFileDialog.FileName;
        }

        private ICommand _buildPayrollReportCommand;
        private ICommand _buildListOfEmployeesReportCommand;
        private const string PayrollTemplateFilePath = "ReportTemplates/PayRollReportTemplate.xlsx";
        private const string ListOfEmployeesTemplateFilePath = "ReportTemplates/ListOfEmployeesTemplate.xlsx";
        private const string TemplateMainCompanyName = "#Main#CompanyName";
        private const string TemplateMainEmployeeName = "#Main#EmployeeName";
        private const string TemplateMainDepartmentName = "#Main#DepartmentName";
        private const string TemplateMainSalary = "#Main#Salary";
        private const string TemplateMainExperience = "#Main#Experience";
        private const string TemplateMainAge = "#Main#Age";
        private const string TemplateDepartmentCompanyName = "#Department#CompanyName";
        private const string TemplateDepartmentDepartmentName = "#Department#DepartmentName";
        private const string TemplateDepartmentSalary = "#Department#Salary";
        private const string TemplateCompanyCompanyName = "#Company#CompanyName";
        private const string TemplateCompanyDepartmentName = "#Company#DepartmentName";
        private const string TemplateCompanySalary = "#Company#Salary";
        private const string TemplateDateCreation = "#DateCreation";
        private const int CountDaysInYear = 365;
    }
}
