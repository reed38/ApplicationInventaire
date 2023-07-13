using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System;


using ApplicationInventaire.Core.Config;
using ApplicationInventaire.Core.DatabaseManagement;
using ApplicationInventaire.Core.ExcelManagement;
using ApplicationInventaire.Core.GlobalPages;
using ApplicationInventaire.Core.PieceSections;
using ApplicationInventaire.Core.ProjectDataSet;
using System.IO.Pipes;
using System.Security.Policy;

namespace ApplicationInventaire.Core.ExcelManagement
{

    /// <summary>
    /// used later in method FindCell to return multiple variables
    /// </summary>
    public class CellInfo
    {


        public int Row { get; set; }
        public int Column { get; set; }
        public string Sheet { get; set; }
        public string Content { set; get; }

        public CellInfo(int Row, int Column, string Sheet, string Content)
        {
            this.Row = Row;
            this.Sheet = Sheet;
            this.Column = Column;
            this.Content = Content;
        }
        public CellInfo() { }




    }

    public class CoupleCellInfo
    {
        public CellInfo tag { set; get; }
        public CellInfo Present { set; get; }

        public CoupleCellInfo(CellInfo tag, CellInfo present)
        {
            this.tag = tag;
            this.Present = present;
        }

    }
    /// <summary>
    /// used to operate on excel sheet
    /// INDEXES START AT ZERO
    /// </summary>
    public class ExcelFile
    {
        #region variables


        #endregion


        #region set_get
        public string FilePath { get; set; }
        #endregion

        #region constructor
        public ExcelFile(string filename)
        {
            FilePath = filename;
        }
        #endregion

        #region methods
        /// <summary>
        /// Used to return the string contained in the cell in a given row column and sheet
        /// </summary>
        /// <param name="sheetName"> name of the sheet targeted</param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns> the content of the cell converted to string</returns>
        public string GetCellValue(string sheetName, int row, int column)
        {
            string cellValue = string.Empty;
            try
            {

                if (FilePath.IndexOf(".xlsx") > 0)
                {
                    using (var file = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                    {
                        XSSFWorkbook workbook = new XSSFWorkbook(file);
                        ISheet sheet = workbook.GetSheet(sheetName);
                        IRow excelRow = sheet.GetRow(row);
                        ICell cell = excelRow.GetCell(column);
                        if (cell != null)
                        {
                            cellValue = cell.ToString();
                        }
                        file.Close();
                    }


                }
                else if (FilePath.IndexOf(".xls") > 0)
                {
                    using (var file = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                    {
                        HSSFWorkbook workbook = new HSSFWorkbook(file);
                        ISheet sheet = workbook.GetSheet(sheetName);
                        IRow excelRow = sheet.GetRow(row);
                        ICell cell = excelRow.GetCell(column);
                        if (cell != null)
                        {
                            cellValue = cell.ToString();
                        }
                        file.Close();

                    }


                }



            }
            catch (IOException ex)
            {
                // Handle file-related errors
                Console.WriteLine("Une erreur s'est produite lors de la lecture du fichier Excel: " + ex.Message);
            }

            return cellValue;
        }

        /// <summary>
        /// set the cell in a given row column and sheet to a given string
        /// </summary>
        /// <param name="sheetName">name of the targeted sheet</param>
        /// <param name="content"> string which will be put in the cell</param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        public void SetCellValue(string sheetName, string content, int column, int row)
        {

            try
            {
                if (FilePath.IndexOf(".xlsx") > 0)
                {
                    using (var file = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                    {
                        XSSFWorkbook workbook = new XSSFWorkbook(file);
                        ISheet sheet = workbook.GetSheet(sheetName);
                        IRow excelRow = sheet.GetRow(row);
                        ICell cell = excelRow.GetCell(column) ?? excelRow.CreateCell(column);
                        cell.SetCellValue(content);
                        file.Close();

                        using (var outputFile = new FileStream(FilePath, FileMode.Create, FileAccess.Write))
                        {
                            workbook.Write(outputFile);
                            outputFile.Close();
                        }
                    }


                }
                else if (FilePath.IndexOf(".xls") > 0)
                {
                    using (var file = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                    {
                        HSSFWorkbook workbook = new HSSFWorkbook(file);
                        ISheet sheet = workbook.GetSheet(sheetName);
                        IRow excelRow = sheet.GetRow(row);
                        ICell cell = excelRow.GetCell(column) ?? excelRow.CreateCell(column);
                        cell.SetCellValue(content);
                        file.Close();

                        using (var outputFile = new FileStream(FilePath, FileMode.Create, FileAccess.Write))
                        {
                            workbook.Write(outputFile);
                            outputFile.Close();
                        }
                    }
                }



            }
            catch (IOException ex)
            {
                // Handle file-related errors
                Console.WriteLine("Une erreur s'est produite lors de la modification du fichier Excel: " + ex.Message);
            }

        }

        /// <summary>
        /// find the first cell in a given file (.xls or .xlsx) containing a given string
        /// 
        /// </summary>
        /// <param name="content">the string that must be found </param>
        /// <returns> the method return a CellInfo struct {int row, int colums, int sheetNumber} .
        /// if nothing is found the struct returned has the sheet field set to -1</returns>  
        public CellInfo FindValue(string content)
        {
            CellInfo cellInfo = new CellInfo();
            if (FilePath.IndexOf(".xlsx") > 0)
            {
                using (var file = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                {
                    XSSFWorkbook workbook = new XSSFWorkbook(file);

                    // Loop through each sheet in the workbook
                    for (int sheetIndex = 0; sheetIndex < workbook.NumberOfSheets; sheetIndex++)
                    {
                        ISheet sheet = workbook.GetSheetAt(sheetIndex);

                        // Loop through each row in the sheet
                        for (int rowIndex = 0; rowIndex <= sheet.LastRowNum; rowIndex++)
                        {
                            IRow row = sheet.GetRow(rowIndex);

                            // Loop through each cell in the row
                            if (row != null)
                            {
                                for (int columnIndex = 0; columnIndex < row.LastCellNum; columnIndex++)
                                {
                                    ICell cell = row.GetCell(columnIndex);

                                    if (cell != null && cell.CellType == CellType.String && cell.StringCellValue == content)
                                    {
                                        // Cell with matching content found, populate the CellInfo struct
                                        cellInfo.Row = rowIndex;
                                        cellInfo.Column = columnIndex;

                                        cellInfo.Sheet = sheet.SheetName;
                                        return cellInfo;
                                    }
                                }
                            }
                        }
                        file.Close();
                    }
                }
            }



            else if (FilePath.IndexOf(".xls") > 0)
            {


                using (var file = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                {
                    HSSFWorkbook workbook = new HSSFWorkbook(file);


                    // Loop through each sheet in the workbook
                    for (int sheetIndex = 0; sheetIndex < workbook.NumberOfSheets; sheetIndex++)
                    {
                        ISheet sheet = workbook.GetSheetAt(sheetIndex);

                        // Loop through each row in the sheet
                        for (int rowIndex = 0; rowIndex <= sheet.LastRowNum; rowIndex++)
                        {
                            IRow row = sheet.GetRow(rowIndex);

                            // Loop through each cell in the row
                            if (row != null)
                            {
                                for (int columnIndex = 0; columnIndex < row.LastCellNum; columnIndex++)
                                {
                                    ICell cell = row.GetCell(columnIndex);

                                    if (cell != null && cell.CellType == CellType.String && cell.StringCellValue == content)
                                    {
                                        // Cell with matching content found, populate the CellInfo struct
                                        cellInfo.Row = rowIndex;
                                        cellInfo.Column = columnIndex;
                                        cellInfo.Sheet = sheet.SheetName;
                                        return cellInfo;
                                    }
                                }
                            }
                        }
                    }
                    file.Close();
                }

            }



            // Cell with matching content not found
            cellInfo.Sheet = null; // return -1 for sheet by default if nothing is found
            return cellInfo;
        }
        #endregion


        #region debugging
        public void DispDataExcel()
        {
            Console.WriteLine(" Excel FilePath: " + FilePath);

        }


        #endregion

    }

}





