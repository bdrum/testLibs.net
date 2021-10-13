using System;
using System.Collections.Generic;
using System.Linq;
using Regata.Core.DataBase;
using Excel = Microsoft.Office.Interop.Excel;

namespace excel
{
    class Program
    {

        static void Main(string[] args)
        {
            var excelApp = new Excel.Application();
            // Make the object visible.
            excelApp.Visible = true;

            // Create a new, empty workbook and add it to the collection returned
            // by property Workbooks. The new workbook becomes the active workbook.
            // Add has an optional parameter for specifying a praticular template.
            // Because no argument is sent in this example, Add creates a new workbook.
            excelApp.Workbooks.Add();

            // This example uses a single workSheet. The explicit type casting is
            // removed in a later procedure.
            Excel._Worksheet workSheet = (Excel.Worksheet)excelApp.ActiveSheet;
            workSheet.Cells.NumberFormat = "@";
            float width = 2;
            Action<string> set_width = (string col) =>
            {
                Excel.Range exCol = workSheet.get_Range($"{col}:{col}", Type.Missing);
                exCol.EntireColumn.ColumnWidth = width;
            };

            (int col1, int col2) cols = (3, 47);
            Action<string> set_borders = (string col) =>
            {
                Excel.Range cell = workSheet.get_Range($"{col}{cols.col1}:{col}{cols.col2}", Type.Missing);
                Excel.Borders border = cell.Borders;
                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                border.Weight = 2d;
                border[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
            };


            Action<string> set_bottom_borders = (string col) =>
            {
                Excel.Range cell = workSheet.get_Range($"{col}{cols.col1}:{col}{cols.col2}", Type.Missing);
                Excel.Borders border = cell.Borders;
                border[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                border[Excel.XlBordersIndex.xlEdgeBottom].Weight = 4d;
            };

            new List<string> { "A", "D", "G", "K", "J" }.ForEach(set_width);

            using (var rc = new RegataContext())
            {
                var irrs = rc.Irradiations.Where(ir => ir.LoadNumber == 258).OrderBy(ir => ir.Container).ThenBy(ir => ir.Position).ToArray();

                var detCont = new Dictionary<string, int[]>
                {
                    { "D1", new int[] { 1,2 } }, 
                    { "D2", new int[] { 3,4 } }, 
                    { "D3", new int[] { 5,6 } }, 
                    { "D4", new int[] { 7 } },
                };

                var detColsExcel = new Dictionary<string, (string Column1, string Column2)>
                {
                    { "D1",  (Column1:"B", Column2: "C")},
                    { "D2",  (Column1:"E", Column2: "F")},
                    { "D3",  (Column1:"H", Column2: "I")},
                    { "D4",  (Column1:"K", Column2: "L")}
                };


                Func<short, short> max_cont_position = (short cont) =>
                {
                    var m = irrs.Where(irr => irr.Container.Value == cont).Select(ir => ir.Position).Max();
                    return m.HasValue ? m.Value : (short)0;
                };

                width = 5;
                detColsExcel.Values.Select(d => d.Column1).ToList().ForEach(set_width);
                width = 14;
                detColsExcel.Values.Select(d => d.Column2).ToList().ForEach(set_width);

                detColsExcel.Values.Select(d => d.Column1).ToList().ForEach(set_borders);
                detColsExcel.Values.Select(d => d.Column2).ToList().ForEach(set_borders);

                foreach (var det in detCont.Keys)
                {
                    workSheet.Cells[2, detColsExcel[det].Column1] = string.Join("-", detCont[det]);
                    var oRng = workSheet.get_Range($"{detColsExcel[det].Column1}2", $"{detColsExcel[det].Column2}2");
                    oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    oRng.Merge(null);
                    var bold_pos = 2;
                    foreach (var cont in detCont[det])
                    {
                        bold_pos += max_cont_position((short)cont);
                        cols = (bold_pos, bold_pos);
                        set_bottom_borders(detColsExcel[det].Column1);
                        set_bottom_borders(detColsExcel[det].Column2);
                    }

                    var irForDet = irrs.Where(irr => detCont[det].Contains((int)irr.Container)).ToArray();
                    for (var i = 0; i < 45; ++i)
                    {
                        workSheet.Cells[i+3, detColsExcel[det].Column1] = i+1;
                        if (irForDet.Length <= i)
                        {
                            workSheet.Cells[i + 3, detColsExcel[det].Column2] = string.Empty;
                        }
                        else
                        {
                            if (irForDet[i].Year == "s" || irForDet[i].Year == "m")
                                workSheet.Cells[i+3, detColsExcel[det].Column2] = irForDet[i].SetKey;
                            else 
                                workSheet.Cells[i+3, detColsExcel[det].Column2] = irForDet[i].SampleKey;
                            
                        }
                    }

                }


                var info_col = "N";
                var info_num = 2;
                workSheet.Cells[info_num, info_col] = irrs[0].LoadNumber;
                width = 15;
                set_width(info_col);
                foreach (var set_key in irrs.Where(ir => ir.Year != "s").Select(ir => ir.SetKey).Distinct())
                {
                    info_num++;
                    workSheet.Cells[info_num, info_col] = set_key;
                }
                workSheet.Cells[47, info_col] = irrs.Length;

            }
            
        }
    }
}
