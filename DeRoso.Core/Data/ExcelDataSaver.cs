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
        public override bool Save(IEnumerable<HealthTestResult> results)
        {
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
                    SaveDrugResult(startRow, d, sheet);
                }
            }

            return true;
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
            currentOutRange.Value = "ИЗМЕРЕНИЕ ДО";

            strAddress = string.Format("C{0}:E{0}", startrow + 1);
            currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            currentOutRange.Value = "0.0";

            //измерение после
            strAddress = string.Format("F{0}:H{0}", startrow);
            currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            currentOutRange.Value = "ИЗМЕРЕНИЕ ДО";

            strAddress = string.Format("F{0}:H{0}", startrow + 1);
            currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            currentOutRange.Value = "100.0";

            //измерение после
            strAddress = string.Format("I{0}", startrow);
            currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            currentOutRange.Value = "ТИП ВЫЧИСЛЕНИЙ";

            strAddress = string.Format("I{0}", startrow + 1);
            currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            currentOutRange.Value = testres.Test.CalculationType.ToString();

            return startrow + 2;
        }

        private int  SaveDrugResult(int startrow, HealthTestDrugResult drugres, Excel.Worksheet sheet )
        {
            //Измерение до
            string strAddress = string.Format("B{0}:C{1}", startrow, startrow + 1);
            Excel.Range currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            currentOutRange.Merge(Type.Missing);
            currentOutRange.Value = drugres.MeassurmentBefore;

            //Измерение после
            strAddress = string.Format("I{0}:I{1}", startrow, startrow + 1);
            currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            currentOutRange.Merge(Type.Missing);
            currentOutRange.Value = drugres.MeassurmentAfter;

            //Адрес препарата
            strAddress = string.Format("С{0}", startrow);
            currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            currentOutRange.Merge(Type.Missing);
            currentOutRange.Value = drugres.Drug.Address;

            //Ячейка препарата
            strAddress = string.Format("D{0}", startrow);
            currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            currentOutRange.Merge(Type.Missing);
            currentOutRange.Value = drugres.Drug.Cell;

            //Название препарата
            strAddress = string.Format("E{0}:H{0}", startrow);
            currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            currentOutRange.Merge(Type.Missing);
            currentOutRange.Value = drugres.Drug.Title;
            
            //Описание препарата
            strAddress = string.Format("С{0}:H{0}", startrow + 1);
            currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            currentOutRange.Merge(Type.Missing);
            currentOutRange.Value = drugres.Drug.Title;

            return startrow + 2;
        }
        
    }
}
