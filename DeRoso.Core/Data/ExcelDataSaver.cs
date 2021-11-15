using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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


        protected virtual Excel.Workbook PrepareWorkBook(Excel.Application excelApp)
        {
            //Отобразить Excel
            excelApp.Visible = false;

            //Количество листов в рабочей книге
            excelApp.SheetsInNewWorkbook = 1;

            //Отключить отображение окон с сообщениями
            excelApp.DisplayAlerts = false;

            //Добавить рабочую книгу
            Excel.Workbook workBook = excelApp.Workbooks.Add(Type.Missing);

            return workBook;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="excelApp"></param>
        /// <returns></returns>
        protected virtual Excel.Worksheet PrepareWorkSheet(HealthTestReport report, Excel.Application excelApp)
        {
            //Получаем первый лист документа (счет начинается с 1)
            Excel.Worksheet sheet = (Excel.Worksheet)excelApp.Worksheets.Item[1];

            //Название листа (вкладки снизу)
            //string pacientName = (report.Patient == null) ? "Новый пциент" : report.Patient.ShortName;
            string sheetName =
                $"{report.ReportDate.ToShortDateString()}-{report.ReportDate.Hour}-{report.ReportDate.Minute}";

            sheet.Name = sheetName;

            sheet.Columns[1].ColumnWidth = 100;
            sheet.Columns[2].ColumnWidth = 20;
            sheet.Columns[3].ColumnWidth = 20;

            return sheet;
        }

        private void SaveSectionAsSheetDiagramm(Excel.Workbook workBook, HealthTestReport report,  HealthTestSection section)
        {
            Excel.Worksheet sheet = workBook.Worksheets.Add();
            sheet.Name = section.Title;
            int startRow = 1;

            sheet.Columns[1].ColumnWidth = 80;
            sheet.Columns[2].ColumnWidth = 10;
            sheet.Columns[3].ColumnWidth = 10;
            sheet.Columns[4].ColumnWidth = 10;
            sheet.Columns[5].ColumnWidth = 10;
            sheet.Columns[6].ColumnWidth = 10;
            sheet.Columns[7].ColumnWidth = 10;
            sheet.Columns[8].ColumnWidth = 10;
            sheet.Columns[9].ColumnWidth = 10;

            SaveSection(startRow, report, section, sheet, true);
        }

        private List<HealthTestSection> EnumSections(HealthTestReport report)
        {
            var tests = report.Results.Select(r => r.Test).ToList();
            var groups = tests.Select(t => t.Group).ToList();
            var sections = groups.Select(s => s.Section).Distinct().ToList();

            return sections;
        }


        /// <summary>
        /// Сохранение результатов 
        /// </summary>
        /// <param name="report"> Отчет о тестах</param>
        /// <param name="showResults"> Отобразить отчет после формирования</param>
        public override bool Save(HealthTestReport report, bool showResults)
        {

            Excel.Application excelApp = new Excel.Application();

            try
            {
                //
                Excel.Workbook workBook = PrepareWorkBook(excelApp);

                //Получаем первый лист документа (счет начинается с 1)
                Excel.Worksheet sheet = PrepareWorkSheet(report, excelApp);

                //Сохраняем заголовок теста
                //начальная строка вывода результатов тестов
                int startRow = SaveHeader(report, sheet);


                //получаем все тестируемые разделы и сохраняем каждый раздел 
                List<HealthTestSection> sec = EnumSections(report);

                try
                {
                    foreach (var s in sec)
                    {
                        //если секция входила в тестирование сохраняем
                        if ( IsSectionTested(report, s) )
                            startRow = SaveSection(startRow, report, s, sheet);
                    }
                        
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    throw;
                }


                foreach (var s in sec)
                {
                    //если секция входила в тестирование сохраняем
                    if (IsSectionTested(report, s))
                        SaveSectionAsSheetDiagramm(workBook, report, s);
                }


                //сохраняем каждый тест из  отчета
                /*foreach (HealthTestResult r in report.Results)
                {
                    startRow = SaveTestResult(startRow, r, sheet);

                    //сохраняем каждый препарат теста
                    foreach(HealthTestDrugResult d in r.Meassurments)
                    {
                        startRow = SaveDrugResult(startRow, d, sheet);
                    }
                }

                */

                workBook.SaveAs(TargetFilePath);

                if (showResults)
                    //Отобразить Excel
                    excelApp.Visible = true;
            }
            catch (Exception e)
            {
                string message =
                    $" Ошибка при опытке сохранения данных в Microsoft Excel. \n Возможно программа не установлена либо не активирована.\n{e.Message}";
                MessageBox.Show(message, "Ошибка Excel", MessageBoxButton.OK, MessageBoxImage.Error);

                excelApp.Quit();
                return false;
            }
            finally
            {
                excelApp.ActiveWorkbook.SaveAs(TargetFilePath);
            }

            return true;
            
        }

        protected virtual int SaveHeader(HealthTestReport report, Excel.Worksheet sheet)
        {
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
            /*
            var res = report.Results.FirstOrDefault();
            string section = (res == null) ? "ОШИБКА ПОЛУЧЕНИЯ СЕКЦИИ" : res.Test.Group.Section.Title;

            rng = (Excel.Range)sheet.get_Range("A6").Cells;
            FormatRange(rng, Excel.XlHAlign.xlHAlignLeft, Excel.XlVAlign.xlVAlignCenter, 22, true);
            rng.Value = section;
            */

            return 8;
        }

        protected virtual int SaveSection(int startRow, HealthTestReport report, HealthTestSection section, Excel.Worksheet sheet, bool asDiagramm = false)
        {
            //Заголовок секции
            string strAddress = $"A{startRow}";
            Excel.Range currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            currentOutRange.Value = section.Title.ToUpper();
            currentOutRange.RowHeight = 35;
            FormatRange(currentOutRange, Excel.XlHAlign.xlHAlignLeft, Excel.XlVAlign.xlVAlignCenter, 20, true);

            int colCount = 7;
            currentOutRange = sheet.Range[
                sheet.Cells[startRow, 1],
                sheet.Cells[startRow, 1 + colCount]
            ];

            currentOutRange.Interior.Color = ColorTranslator.ToOle(Color.White);

            //пропускаем 2 строки
            startRow += 1;

            try
            {
                foreach (var gr in section.Groups)
                {
                    if (IsGroupTested(report, gr))
                        startRow = SaveGroup(startRow, report, gr, sheet, asDiagramm);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
            


            return startRow;
        }


        protected virtual int SaveGroup(int startRow, HealthTestReport report, HealthTestGroup group, Excel.Worksheet sheet, bool asDiagramm = false)
        {
            //Заголовок группы
            string strAddress = $"A{startRow}";
            Excel.Range currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            currentOutRange.Value = group.Title;
            currentOutRange.RowHeight = 30;
            FormatRange(currentOutRange, Excel.XlHAlign.xlHAlignLeft, Excel.XlVAlign.xlVAlignCenter, 18, true, 1);

            int colCount = 7;
            currentOutRange = sheet.Range[
                sheet.Cells[startRow, 1],
                sheet.Cells[startRow, 1 + colCount]
            ];
            currentOutRange.Interior.Color = ColorTranslator.ToOle(Color.White);

            //пропускаем 2 строки
            startRow += 1;

            try
            {
                foreach (var test in group.Tests)
                {
                    if (IsTestTested(report, test))
                        startRow = SaveTest(startRow, report, test, sheet, asDiagramm);
                }
                   
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
           
            

            return startRow;
        }

        protected virtual int SaveTest(int startRow, HealthTestReport report, HealthTest test, Excel.Worksheet sheet, bool asDiagramm = false)
        {

            var testRes = report.Results.Select(r => r).FirstOrDefault(r => r.Test.Id == test.Id);

            if (testRes == null)
                return startRow;

            //Заголовок теста
            string strAddress = $"A{startRow}";
            Excel.Range currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            currentOutRange.Value = test.Title;
            currentOutRange.RowHeight = 20;
            FormatRange(currentOutRange, Excel.XlHAlign.xlHAlignLeft, Excel.XlVAlign.xlVAlignCenter, 16, true, 2);

            int colCount = 7;
            currentOutRange = sheet.Range[
                sheet.Cells[startRow, 1],
                sheet.Cells[startRow, 1 + colCount]
            ];
            currentOutRange.Interior.Color = ColorTranslator.ToOle(Color.White);

            //если сохраняем как простая таблица
            if (!asDiagramm)
            {
                //ШКАЛА
                strAddress = $"B{startRow}";
                currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
                currentOutRange.Value2 = testRes?.Scale ?? (dynamic)"ОШИБКА";

                FormatRange(currentOutRange, Excel.XlHAlign.xlHAlignCenter, Excel.XlVAlign.xlVAlignCenter, 16, true);
                if (test.Reciepts.Count > 1)
                {
                    currentOutRange.NumberFormat = "#";
                }
                else
                {
                    currentOutRange.NumberFormat = "##.##";
                }

                //ТИП РАСЧЕТА
                strAddress = $"C{startRow}";
                currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
                currentOutRange.Value = EnumHelper.GetDescription(testRes.Test.CalculationType);
                FormatRange(currentOutRange, Excel.XlHAlign.xlHAlignCenter, Excel.XlVAlign.xlVAlignCenter, 16, false);

                strAddress = string.Format("A{0}:C{0}", startRow);
                currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
                currentOutRange.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = 1;
                currentOutRange.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;

                //добавляем строку
                startRow += 1;

                try
                {
                    //сохраняем результаты препаратов
                    foreach (var m in testRes.Meassurments)
                        startRow = SaveDrugResult(startRow, m, sheet);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    throw;
                }

            }
            else
            {
                //выводим шкалу
                startRow = DrawScale(startRow, testRes, sheet);
            }

            return startRow;
        }

        private int DrawScale(int startRow, HealthTestResult res, Excel.Worksheet sheet)
        {
            Excel.Range colorOutRange = null;
            Excel.Range strokeOutRange = null;

            float percent = 0.0f;
            int size = res.Meassurments.Count;

            int colCount = 7;
            Excel.Range currentOutRange = sheet.Range[
                sheet.Cells[startRow, 1],
                sheet.Cells[startRow + 1, 1 + colCount]
            ];
            currentOutRange.Interior.Color = ColorTranslator.ToOle(Color.White);

            //однопрепаратный тест
            if (size == 1)
            {
                percent = res.Scale / 100.0f;
                colorOutRange = (Excel.Range)sheet.Range[$"B{startRow}"];
                strokeOutRange = (Excel.Range)sheet.Range[$"B{startRow}"];
                colorOutRange.Value = res.Scale;
                colorOutRange.NumberFormat = "##.##";
                FormatRange(strokeOutRange, Excel.XlHAlign.xlHAlignCenter, Excel.XlVAlign.xlVAlignCenter, 18, true, 0);
            }
            else
            {
                percent = res.Scale / res.Meassurments.Count;
                strokeOutRange = sheet.Range[
                    sheet.Cells[startRow, 2], 
                    sheet.Cells[startRow, size + 1 ]
                ];

                colorOutRange = sheet.Range[
                    sheet.Cells[startRow, 2],
                    sheet.Cells[startRow, (int)res.Scale + 1]
                ];

                ///вывод значения шкалы
                for (int i = 1; i < size + 1; i++)
                {
                    currentOutRange = sheet.Cells[ startRow + 1, i + 1];

                    FormatRange(currentOutRange, Excel.XlHAlign.xlHAlignCenter, Excel.XlVAlign.xlVAlignCenter, 8, false, 0);
                    currentOutRange.RowHeight = 10;
                    currentOutRange.Value = i;
                }

            }

            Color scaleColor = GetScaleColor(percent, res.Test.IsReversedResult);

            strokeOutRange.BorderAround( Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThin );
            colorOutRange.Interior.Color = ColorTranslator.ToOle(scaleColor); 

            return startRow + 2;
        }

        private Color GetScaleColor(float percent, bool inverce)
        {
            //Зелёный - 169, 208, 142
            //Жёлтый - 255, 217, 102
            //Красный - 236, 77, 60
            if (inverce)
            {
                //если обратный расчет
                //case [0-0.4]: цвет ячейки зелёный
                if (percent < 0.4f)
                    return Color.FromArgb(255, 169, 208, 142);

                //case (0.4-0.7]: цвет ячейки жёлтый
                if (percent < 0.7f)
                    return Color.FromArgb(255, 255, 217, 102);

                //case (0.7-1]: цвет ячейки красный
                return Color.FromArgb(255, 236, 77, 60);
            }
            else
            {
                //прямой метод расчета

                //case (0.7-1]: цвет ячейки зелёный
                if (percent > 0.7f)
                    return Color.FromArgb(255, 169, 208, 142);

                //case (0.4-0.7]: цвет ячейки жёлтый
                if (percent > 0.4)
                    return Color.FromArgb(255, 255, 217, 102);

                //case [0-0.4]: цвет ячейки красный
                return Color.FromArgb(255, 236, 77, 60);

            }
        }

        protected bool IsSectionTested(HealthTestReport report, HealthTestSection section)
        {
            //Все пройденные тесты 
            var tests = report.Results.Select(r => r.Test).ToList();

            //набор тестов пустой
            if (tests.Count == 0)
                return false;

            //все группы входящие в тестирование
            var groups = tests.Select(t => t.Group).Distinct().ToList();

            //проверить входит ли группа  
            var sec = groups.Select(s => s.Section).FirstOrDefault(s => s.Id == section.Id);

            if (sec == null)
                return false;

            return true;
        }

        protected bool IsGroupTested(HealthTestReport report, HealthTestGroup group)
        {
            //Все пройденные тесты 
            var tests = report.Results.Select(r => r.Test).ToList();

            //набор тестов пустой
            if (tests.Count == 0)
                return false;

            //группа входит в результаты тестов
            var gr = tests.Select(t => t.Group).FirstOrDefault(t => t.Id == group.Id);
            
            if (gr == null)
                return false;

            return true;
        }

        protected bool IsTestTested(HealthTestReport report, HealthTest test)
        {
            //найти тест в результатах 
            var tst = report.Results.Select(r => r.Test).FirstOrDefault(r => r.Id == test.Id);

            //тест не найден
            if (tst == null)
                return false;

            return true;
        }

        protected void FormatRange(Excel.Range rng, Excel.XlHAlign halign, Excel.XlVAlign valign, int  fontsize, bool bold, int indent = 0)
        {
            rng.VerticalAlignment = valign;
            rng.HorizontalAlignment = halign;
            rng.Font.Name = bold  ? "Roboto Condensed" : "Roboto Condensed Light";
            rng.Font.Size = fontsize;

            rng.IndentLevel = indent;
            //rng.Font.Bold = bold;
        }

        /// <summary>
        /// Сохранение результатов отдельного препарата
        /// </summary>
        /// <param name="startrow">Номер строки для сохранения </param>
        /// <param name="drugres">Результат  теста конкретного препарата из теста</param>
        /// <param name="sheet">Лист на котором сохраняются результаты</param>
        /// <returns></returns>
        protected virtual int  SaveDrugResult(int startRow, HealthTestDrugResult drugres, Excel.Worksheet sheet )
        {
            HealthTestDrug drug = drugres.Drug;

            //Заголовок
            string strAddress = $"A{startRow}";
            Excel.Range currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            string drugString = $"{drug.Address}:{drug.Cell} : {drug.Title}";
            currentOutRange.Value = drugString;
            currentOutRange.IndentLevel = 1;
            FormatRange(currentOutRange, Excel.XlHAlign.xlHAlignLeft, Excel.XlVAlign.xlVAlignCenter, 18, false, 4);

            int colCount = 7;
            currentOutRange = sheet.Range[
                sheet.Cells[startRow, 1],
                sheet.Cells[startRow + 1, 1 + colCount]
            ];
            currentOutRange.Interior.Color = ColorTranslator.ToOle(Color.White);

            //Измерение  после
            strAddress = $"B{startRow}";
            currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            currentOutRange.Value2 = drugres.MeassurmentBefore;
            currentOutRange.NumberFormat = "0.00";
            FormatRange(currentOutRange, Excel.XlHAlign.xlHAlignCenter, Excel.XlVAlign.xlVAlignCenter, 18, false);

            //Измерение  после
            strAddress = $"C{startRow}";
            currentOutRange = (Excel.Range)sheet.get_Range(strAddress).Cells;
            currentOutRange.Value2 = drugres.MeassurmentAfter;
            currentOutRange.NumberFormat = "0.00";
            FormatRange(currentOutRange, Excel.XlHAlign.xlHAlignCenter, Excel.XlVAlign.xlVAlignCenter, 18, false);

            if (drugres.IsOptimal)
            {
                strAddress = string.Format("A{0}:C{0}", startRow);
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

            return startRow + 1;
        }
        
    }
}
