using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public override bool Save(HealthTestReport report)
        {
            return false;

            /*
            Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();


            //Отобразить Excel
            ExcelApp.Visible = true;

            //Количество листов в рабочей книге
            ExcelApp.SheetsInNewWorkbook = 1;

            //Добавить рабочую книгу
            Excel.Workbook workBook = ExcelApp.Workbooks.Add(Type.Missing);

            //Отключить отображение окон с сообщениями
            ExcelApp.DisplayAlerts = false;

            //Получаем первый лист документа (счет начинается с 1)
            Excel.Worksheet sheet = (Excel.Worksheet)ExcelApp.Worksheets.get_Item(1);

            //Название листа (вкладки снизу)
            sheet.Name = string.Format("Отчет за {0}", DateTime.Now.ToShortDateString());

            //Заголовок
            Excel.Range rangeHeader = (Excel.Range)sheet.get_Range("A1", "I1").Cells;
            rangeHeader.Merge(Type.Missing);
            rangeHeader.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            rangeHeader.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

            rangeHeader.Value = string.Format("Отчет за {0} - {1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortDateString());


            //СЕКЦИЯ 
            string section = results.FirstOrDefault().Test.Group.Section.Title;

            Excel.Range rangeSection = (Excel.Range)sheet.get_Range("A2", "I2").Cells;
            rangeSection.Merge(Type.Missing);
            rangeHeader.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            rangeHeader.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            rangeSection.Value = section;


            /////////////////////////////////////////////////////////////////
           
            //начальная строка вывода.
            int startRow = 4;
            foreach (HealthTestResult r in results)
            {
                startRow = SaveTestResult(startRow, r, sheet);

                foreach(HealthTestDrugResult d in r.Meassurments)
                {
                    startRow = SaveDrugResult(startRow, d, sheet);
                }
            }

            return true;
            */
        }

        private int SaveTestResult(int startrow, HealthTestResult testres, Excel.Worksheet sheet)
        {
            //Заголовок теста
            string strAddress = string.Format("A{0}:B{1}", startrow, startrow + 1);
            Excel.Range currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            currentOutRange.Merge(Type.Missing);
            currentOutRange.Value = testres.Test.Title;

            //измерение до
            strAddress = string.Format("C{0}:E{0}", startrow);
            currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            currentOutRange.Merge(Type.Missing);
            currentOutRange.Value = "ИЗМЕРЕНИЕ ДО";

            strAddress = string.Format("C{0}:E{0}", startrow + 1);
            currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            currentOutRange.Merge(Type.Missing);
            currentOutRange.Value = "0.0";

            //измерение после
            strAddress = string.Format("F{0}:H{0}", startrow);
            currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            currentOutRange.Merge(Type.Missing);
            currentOutRange.Value = "ИЗМЕРЕНИЕ ДО";

            strAddress = string.Format("F{0}:H{0}", startrow + 1);
            currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            currentOutRange.Merge(Type.Missing);
            currentOutRange.Value = "100.0";

            //Тип вычислений
            strAddress = string.Format("I{0}", startrow);
            currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            currentOutRange.Value = "ТИП ВЫЧИСЛЕНИЙ";

            strAddress = string.Format("I{0}", startrow + 1);
            currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            currentOutRange.Value = testres.Test.CalculationType.ToString();

            return startrow + 2;
        }

        private void FormatRange(Excel.Range rng, Excel.XlHAlign halign, Excel.XlVAlign valign, int  fontsize, bool bold)
        {
            rng.VerticalAlignment = valign;
            rng.HorizontalAlignment = halign;
            rng.Font.Name = "Roboto Condenced Light";
            rng.Font.Size = fontsize;
            rng.Font.Bold = bold;
        }
        
        private int  SaveDrugResult(int startrow, HealthTestDrugResult drugres, Excel.Worksheet sheet )
        {
            //Измерение до
            string strAddress = string.Format("B{0}:C{1}", startrow, startrow + 1);
            Excel.Range currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            currentOutRange.Merge(Type.Missing);
            currentOutRange.Value = drugres.MeassurmentBefore;
            FormatRange(currentOutRange, Excel.XlHAlign.xlHAlignCenter, Excel.XlVAlign.xlVAlignCenter, 20, true);            


            //Измерение после
            strAddress = string.Format("I{0}:I{1}", startrow, startrow + 1);
            currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            currentOutRange.Merge(Type.Missing);
            currentOutRange.Value = drugres.MeassurmentAfter;
            FormatRange(currentOutRange, Excel.XlHAlign.xlHAlignCenter, Excel.XlVAlign.xlVAlignCenter, 20, true);

            //Адрес препарата
            strAddress = string.Format("D{0}", startrow);
            currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            currentOutRange.Merge(Type.Missing);
            currentOutRange.Value = drugres.Drug.Address;
            FormatRange(currentOutRange, Excel.XlHAlign.xlHAlignCenter, Excel.XlVAlign.xlVAlignCenter, 16, false);

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
            

            return startrow + 2;
        }
        
    }
}
