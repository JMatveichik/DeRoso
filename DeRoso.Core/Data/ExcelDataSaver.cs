using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DeRoso.Core.Health;
using Excel = Microsoft.Office.Interop.Excel;

namespace DeRoso.Core.Data
{
    public class ExcelResultsSaver : FileOutputResultSaver
    {
        /// <summary>
        /// Конструктор класса сохранения в Файл Excel
        /// </summary>
        /// <param name="targetPath">Путь к файлу сохранения</param>
        public ExcelResultsSaver(string targetPath) : base(targetPath)
        {

        }

        /// <summary>
        /// Сохранение результатов 
        /// </summary>
        /// <param name="results"></param>
        public override bool Save(HealthTestReport report, bool showResults)
        {
            
            try {
                Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();

                //Отобразить Excel
                ExcelApp.Visible = false;

                //Количество листов в рабочей книге
                ExcelApp.SheetsInNewWorkbook = 1;

                //Добавить рабочую книгу
                Excel.Workbook workBook = ExcelApp.Workbooks.Add(Type.Missing);

                //Отключить отображение окон с сообщениями
                ExcelApp.DisplayAlerts = false;

                //Получаем первый лист документа (счет начинается с 1)
                Excel.Worksheet sheet = (Excel.Worksheet)ExcelApp.Worksheets.get_Item(1);
                sheet.Columns[1].ColumnWidth = 80;
                sheet.Columns[2].ColumnWidth = 20;
                sheet.Columns[3].ColumnWidth = 20;

                //Название листа (вкладки снизу)
                string pacientName = (report.Patient == null) ? "Новый пциент" : report.Patient.ShortName;
                sheet.Name = string.Format("{0}-{1}-{2}:{3}", pacientName, report.ReportDate.ToShortDateString(), report.ReportDate.Hour, report.ReportDate.Minute);

                //ДАТА
                Excel.Range rng = (Excel.Range)sheet.get_Range("A1").Cells;
                FormatRange(rng, Excel.XlHAlign.xlHAlignLeft, Excel.XlVAlign.xlVAlignCenter, 16, true);
                rng.Value = string.Format("ДАТА");

                rng = (Excel.Range)sheet.get_Range("B1").Cells;
                FormatRange(rng, Excel.XlHAlign.xlHAlignLeft, Excel.XlVAlign.xlVAlignCenter, 16, false);
                rng.Value = report.ReportDate.ToShortDateString();

                ///ВРЕМЯ
                rng = (Excel.Range)sheet.get_Range("A2").Cells;
                FormatRange(rng, Excel.XlHAlign.xlHAlignLeft, Excel.XlVAlign.xlVAlignCenter, 16, true);
                rng.Value = string.Format("ВРЕМЯ");

                rng = (Excel.Range)sheet.get_Range("B2").Cells;
                FormatRange(rng, Excel.XlHAlign.xlHAlignLeft, Excel.XlVAlign.xlVAlignCenter, 16, false);
                rng.Value = report.ReportDate.ToShortTimeString();

                ///ПАЦИЕНТ
                rng = (Excel.Range)sheet.get_Range("A3").Cells;
                FormatRange(rng, Excel.XlHAlign.xlHAlignLeft, Excel.XlVAlign.xlVAlignCenter, 16, true);
                rng.Value = string.Format("ПАЦИЕНТ");

                rng = (Excel.Range)sheet.get_Range("B3").Cells;
                FormatRange(rng, Excel.XlHAlign.xlHAlignLeft, Excel.XlVAlign.xlVAlignCenter, 16, false);
                rng.Value = report.Patient.ShortName;

                //СЕКЦИЯ 
                var res = report.Results.FirstOrDefault();
                string section = (res == null) ? "ОШИБКА ПОЛУЧЕНИЯ СЕКЦИИ" : res.Test.Group.Section.Title;

                rng = (Excel.Range)sheet.get_Range("A6").Cells;
                FormatRange(rng, Excel.XlHAlign.xlHAlignLeft, Excel.XlVAlign.xlVAlignCenter, 22, true);
                rng.Value = section;

            
                //начальная строка вывода.
                int startRow = 8;
                foreach (HealthTestResult r in report.Results)
                {
                    startRow = SaveTestResult(startRow, r, sheet);

                    foreach(HealthTestDrugResult d in r.Meassurments)
                    {
                        startRow = SaveDrugResult(startRow, d, sheet);
                    }
                }


                workBook.SaveAs(TargetFilePath);

                if (showResults)
                    //Отобразить Excel
                    ExcelApp.Visible = true;
                else
                    ExcelApp.Quit();
            }
            catch(Exception e)
            {
                string message = string.Format(" Ошибка при опытке сохранения данных в Microsoft Excel. \n Возможно программа не установлена либо не активирована.\n\t{0}", e.Message);
                MessageBox.Show(message, "Ошибка Excel", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
            
        }


        private int SaveTestResult(int startrow, HealthTestResult testres, Excel.Worksheet sheet)
        {
            HealthTest test = testres.Test;

            //Заголовок теста
            string strAddress = string.Format("A{0}", startrow);
            Excel.Range currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            currentOutRange.Value = test.Title;
            currentOutRange.RowHeight = 35;
            FormatRange(currentOutRange, Excel.XlHAlign.xlHAlignLeft, Excel.XlVAlign.xlVAlignCenter, 20, true);
            
            //ШКАЛА
            strAddress = string.Format("B{0}", startrow);
            currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            currentOutRange.Value2 = testres.Scale;
            FormatRange(currentOutRange, Excel.XlHAlign.xlHAlignCenter, Excel.XlVAlign.xlVAlignCenter, 18, true);
            if (test.Reciepts.Count > 1)
            {
                currentOutRange.NumberFormat = "#";
            }
            else
            {
                currentOutRange.NumberFormat = "##.##";
            }                

            //ТИП РАСЧЕТА
            strAddress = string.Format("C{0}", startrow);
            currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            currentOutRange.Value = EnumHelper.GetDescription( testres.Test.CalculationType );
            FormatRange(currentOutRange, Excel.XlHAlign.xlHAlignCenter, Excel.XlVAlign.xlVAlignCenter, 16, false);

            strAddress = string.Format("A{0}:C{0}", startrow);
            currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            currentOutRange.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = 1;
            currentOutRange.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
            return startrow + 1;
        }

        private void FormatRange(Excel.Range rng, Excel.XlHAlign halign, Excel.XlVAlign valign, int  fontsize, bool bold)
        {
            rng.VerticalAlignment = valign;
            rng.HorizontalAlignment = halign;
            rng.Font.Name = bold  ? "Roboto Condensed" : "Roboto Condensed Light";
            rng.Font.Size = fontsize;
            //rng.Font.Bold = bold;
        }

        
        private int  SaveDrugResult(int startrow, HealthTestDrugResult drugres, Excel.Worksheet sheet )
        {
            HealthTestDrug drug = drugres.Drug;

            //Заголовок
            string strAddress = string.Format("A{0}", startrow);
            Excel.Range currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            string drugString = string.Format("{0}:{1} : {2}", drug.Address, drug.Cell, drug.Title);
            currentOutRange.Value = drugString;
            currentOutRange.IndentLevel = 1;
            FormatRange(currentOutRange, Excel.XlHAlign.xlHAlignLeft, Excel.XlVAlign.xlVAlignCenter, 18, false);            


            //Измерение  после
            strAddress = string.Format("B{0}", startrow);
            currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            currentOutRange.Value2 = drugres.MeassurmentBefore;
            currentOutRange.NumberFormat = "0.00";
            FormatRange(currentOutRange, Excel.XlHAlign.xlHAlignCenter, Excel.XlVAlign.xlVAlignCenter, 18, false);

            //Измерение  после
            strAddress = string.Format("C{0}", startrow);
            currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            currentOutRange.Value2 = drugres.MeassurmentAfter;
            currentOutRange.NumberFormat = "0.00";
            FormatRange(currentOutRange, Excel.XlHAlign.xlHAlignCenter, Excel.XlVAlign.xlVAlignCenter, 18, false);

            if (drugres.IsOptimal)
            {
                strAddress = string.Format("A{0}:C{0}", startrow);
                currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
                currentOutRange.Font.Color = Excel.XlRgbColor.rgbForestGreen;
            }
                
            

            /*
            //Адрес препарата
            strAddress = string.Format("D{0}", startrow);
            currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            currentOutRange.Value = .Address;
            FormatRange(currentOutRange, Excel.XlHAlign.xlHAlignCenter, Excel.XlVAlign.xlVAlignCenter, 18, false);

            //Ячейка препарата
            strAddress = string.Format("E{0}", startrow);
            currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            currentOutRange.Merge(Type.Missing);
            currentOutRange.Value = drugres.Drug.Cell;
            FormatRange(currentOutRange, Excel.XlHAlign.xlHAlignCenter, Excel.XlVAlign.xlVAlignCenter, 16, false);

            //Название препарата
            strAddress = string.Format("F{0}:H{0}", startrow);
            currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            currentOutRange.Merge(Type.Missing);
            currentOutRange.Value = drugres.Drug.Title;
            FormatRange(currentOutRange, Excel.XlHAlign.xlHAlignCenter, Excel.XlVAlign.xlVAlignCenter, 16, false);

            //Описание препарата
            strAddress = string.Format("D{0}:H{0}", startrow + 1);
            currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            currentOutRange.Merge(Type.Missing);
            currentOutRange.Value = drugres.Drug.Description;
            FormatRange(currentOutRange, Excel.XlHAlign.xlHAlignCenter, Excel.XlVAlign.xlVAlignCenter, 12, false);

            if (drugres.IsOptimal)
            {
                strAddress = string.Format("B{0}:I{1}", startrow, startrow + 1);
                currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
                currentOutRange.Interior.Color = Excel.XlRgbColor.rgbAquamarine;
            }
            */

            return startrow + 1;
        }
        
    }
}
