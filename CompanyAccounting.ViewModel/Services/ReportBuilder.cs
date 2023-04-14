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
    public class ReportBuilder
    {
        public ReportBuilder() { }

        public ICommand BuildPayrollReportCommand => _buildPayrollReportCommand ?? (_buildPayrollReportCommand = new RelayCommand(BuildPayrollReport));
        public ICommand BuildListOfEmployeesReportCommand => _buildListOfEmployeesReportCommand ?? (_buildListOfEmployeesReportCommand = new RelayCommand(BuildListOfEmployeesReport));

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
                Range sheetRange = workSheet.UsedRange;
                Range findedMainCompanyTemplate = sheetRange.Find(TemplateMainCompanyName);
                if (findedMainCompanyTemplate != null)
                {
                    var countEmployees = 0;
                    if (Model.Companies != null)
                        foreach(var company in Model.Companies)
                        {
                            if (company.Departments != null)
                                foreach (var department in company.Departments)
                                    countEmployees += department.WorkbookEntries.Count;
                        }
                    
                    for(var i = 0; i < countEmployees; i++)
                        findedMainCompanyTemplate.EntireRow.Insert(XlInsertShiftDirection.xlShiftDown);
                    var addedRow = workSheet.Rows[findedMainCompanyTemplate.EntireRow.Row - 1];
                    findedMainCompanyTemplate.EntireRow.Copy(addedRow);
                }

                reportWorkbook.SaveAs(reportFilePath);
                reportWorkbook.Close();
                //Excel.Application.Save(reportFilePath);
            }
            finally { Excel.Quit(); }
        }

        private void BuildListOfEmployeesReport()
        { 
        
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
        private const string ListOfEmployeesTemplateFilePath = "ReportTemplates/ListOfEmployees.xlsx";
        private const string TemplateMainCompanyName = "#Main#CompanyName";
    }
}
