using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace PartAnalysis.Models
{
    public class exportToEPPExcel
    {
        public string ExportToEPPExcel(DataSet ds, string fileName)
        {
            try
            {


                ExcelPackage ep = new ExcelPackage();
                ExcelWorksheet ewsSummary;
                ExcelWorksheet ewsDetailed;
                
                DataTable dtSummary;
                DataTable dtDetailed;
                

                string pathToWriteFile = ConfigurationManager.AppSettings["fileExportPath"].ToString();

                if (ds.Tables.Contains("dtSummary") && ds.Tables["dtSummary"] !=null&& ds.Tables["dtSummary"].Rows.Count>0)
                {
                    dtSummary = new DataTable();
                    dtSummary = ds.Tables["dtSummary"];
                   
                        if (dtSummary != null && dtSummary.Rows.Count > 0)
                        {

                        dtSummary.Columns[0].ColumnName = "Material Number";
                        dtSummary.Columns[1].ColumnName = "Plant Code";
                        dtSummary.Columns[2].ColumnName = "Parent Supplier";
                        dtSummary.Columns[3].ColumnName = "Supplier Site ";
                        dtSummary.Columns[4].ColumnName = "Receipt Qty";
                        dtSummary.Columns[5].ColumnName = "Spend";

                        ewsSummary = ep.Workbook.Worksheets.Add("Summary Receipts");

                            int colcnt = dtSummary.Columns.Count;
                            int rowcnt = dtSummary.Rows.Count;


                        using (ExcelRange col = ewsSummary.Cells[2, 5, rowcnt + 1, 5])
                        {
                            col.Style.Numberformat.Format = "0.00";
                            col.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        }
                        using (ExcelRange col = ewsSummary.Cells[2, 6, rowcnt + 1,6])
                        {
                            col.Style.Numberformat.Format = "$#,##0.00";
                            col.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        }
                       
                        string colname = "";

                        ewsSummary.Cells["A1"].LoadFromDataTable(dtSummary, true, OfficeOpenXml.Table.TableStyles.None);

                        ewsSummary.Cells[1, 1, 1, colcnt].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                           ewsSummary.Cells[1, 1, 1, colcnt].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
                           ewsSummary.Cells[1, 1, 1, colcnt].Style.Font.Color.SetColor(Color.White);
                           ewsSummary.Cells[1, 1, 1, colcnt].Style.Font.Bold = true;

                        ewsSummary.Cells[1, 1, 1, colcnt].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.General;
                            ewsSummary.Column(1).Width = 18;
                            ewsSummary.Column(2).Width = 18;
                            ewsSummary.Column(3).Width = 18;
                            ewsSummary.Column(4).Width = 18;
                            ewsSummary.Column(5).Width = 18;
                        

                    }
                    
                }

                if (ds.Tables.Contains("dtDetailed") && ds.Tables["dtDetailed"] != null && ds.Tables["dtDetailed"].Rows.Count > 0)
                {
                    dtDetailed = new DataTable();
                    dtDetailed = ds.Tables["dtDetailed"];
                    if (dtDetailed != null && dtDetailed.Rows.Count > 0)
                    {
                        dtDetailed.Columns[0].ColumnName = "Material Number";
                        dtDetailed.Columns[1].ColumnName = "Received In";
                        dtDetailed.Columns[2].ColumnName = "Plant Code";
                       dtDetailed.Columns[3].ColumnName = "Receipt Date";
                       dtDetailed.Columns[4].ColumnName = "PO Number";
                        dtDetailed.Columns[5].ColumnName = "PO Line Number";
                        dtDetailed.Columns[6].ColumnName = "Receipt Qty";
                       dtDetailed.Columns[7].ColumnName = "Unit Price";
                       dtDetailed.Columns[8].ColumnName = "Spend";
                       dtDetailed.Columns[9].ColumnName = "Planner Code";
                       dtDetailed.Columns[10].ColumnName = "Planner";
                       dtDetailed.Columns[11].ColumnName = "Parent Supplier";
                       dtDetailed.Columns[12].ColumnName = "Supplier Site ";

                        ewsDetailed = ep.Workbook.Worksheets.Add("Detailed Receipts");

                        int colcnt = dtDetailed.Columns.Count;
                        int rowcnt = dtDetailed.Rows.Count;

                        string colname = "";

                        ewsDetailed.Cells["A1"].LoadFromDataTable(dtDetailed, true, OfficeOpenXml.Table.TableStyles.None);

                        ewsDetailed.Cells[1, 1, 1, colcnt].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ewsDetailed.Cells[1, 1, 1, colcnt].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
                        ewsDetailed.Cells[1, 1, 1, colcnt].Style.Font.Color.SetColor(Color.White);
                        ewsDetailed.Cells[1, 1, 1, colcnt].Style.Font.Bold = true;
                        ewsDetailed.Cells[1, 1, 1, colcnt].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.General;

                        using (ExcelRange col = ewsDetailed.Cells[2, 4, rowcnt+1, 4])
                        {
                            col.Style.Numberformat.Format = "MM/dd/yyyy";
                            col.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        }
                        using (ExcelRange col = ewsDetailed.Cells[2, 7, rowcnt + 1, 7])
                        {
                            col.Style.Numberformat.Format = "0.00";
                            col.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        }
                        using (ExcelRange col = ewsDetailed.Cells[2, 8, rowcnt + 1, 8])
                        {
                            col.Style.Numberformat.Format = "$#,##0.00";
                            col.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        }
                        using (ExcelRange col = ewsDetailed.Cells[2, 9, rowcnt + 1, 9])
                        {
                            col.Style.Numberformat.Format = "$#,##0.00";
                            col.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        }

                        ewsDetailed.Column(1).Width = 20;
                      ewsDetailed.Column(2).Width = 20;
                      ewsDetailed.Column(3).Width = 20;
                      ewsDetailed.Column(4).Width = 20;
                      ewsDetailed.Column(5).Width = 20;
                      ewsDetailed.Column(6).Width = 20;
                      ewsDetailed.Column(7).Width = 20;
                      ewsDetailed.Column(8).Width = 20;
                      ewsDetailed.Column(9).Width = 20;
                        ewsDetailed.Column(10).Width = 20;
                        ewsDetailed.Column(11).Width = 20;
                        ewsDetailed.Column(12).Width = 30;
                        ewsDetailed.Column(13).Width = 40;

                    }
                }
               
                FileInfo fi = new FileInfo(pathToWriteFile + @"\" + fileName + ".xlsx");

                if (fi.Exists)
                {
                    fi.Delete();
                }
                //ep.SaveAs(fi);
                if (ep.Workbook.Worksheets.Count > 0)
                {
                    using (FileStream aFile = new FileStream(pathToWriteFile + @"\" + fileName + ".xlsx", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {

                        aFile.Seek(0, SeekOrigin.Begin);
                        ep.SaveAs(aFile);
                        aFile.Close();
                    }
                }

         
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return fileName + ".xlsx";
        }
    }
}