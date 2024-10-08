using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System;


using ApplicationInventaire.Core.DatabaseManagement;
using ApplicationInventaire.Core.ExcelManagement;
using ApplicationInventaire.Core.GlobalPages;
using ApplicationInventaire.Core.PieceSections;
using ApplicationInventaire.Core.ProjectDataSet;
using System.IO.Pipes;
using System.Security.Policy;
using NPOI.SS.Formula.Functions;
using NPOI.HSSF.Util;
using static SQLite.TableMapping;
using System.Reflection.Metadata;


/// <summary>
/// This file contains function used to read and write infos in a given excel file.
/// </summary>
namespace ApplicationInventaire.Core.ExcelManagement
{

    /// <summary>
    /// Used to store information about an excel cell.
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

    /// <summary>
    /// Class to operate on an excel file.
    /// INDEXES START AT ZERO
    /// </summary>
    public class ExcelFile
    {
       

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
        /// Return the color of a given cell presents on a given sheet, at a given row and column
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns> byte[3] array, containing RGB values</returns>
        public byte[] GetCellColor(string sheetName, int row, int column)
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
                            ICellStyle cellStyle = cell.CellStyle;
                            XSSFColor color = cell.CellStyle.FillForegroundColorColor as XSSFColor;
                            byte[] rgb = color.RGB;

                            return rgb;


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
                            ICellStyle cellStyle = cell.CellStyle;
                            IFont font = cellStyle.GetFont(workbook);


                            HSSFColor color = cell.CellStyle.FillForegroundColorColor as HSSFColor;
                            byte[] rgb = color.RGB;
                            return rgb;
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

            return null;
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
                        if(excelRow==null)
                        {
                            excelRow = sheet.CreateRow(row);
                        }
                        ICell cell = excelRow.GetCell(column) ?? excelRow.CreateCell(column);
                        if(content.Equals("1"))
                        {
                            cell.SetCellValue(1);

                        }
                       else if (content.Equals("0"))
                        {
                            cell.SetCellValue(0);

                        }
                        else 
                        {
                            cell.SetCellValue(content); //there is no recurence, fonction from NPOI library


                        }
                        var formulaEvaluator = workbook.GetCreationHelper().CreateFormulaEvaluator();

                        // Evaluate the formulas
                        formulaEvaluator.EvaluateAll();

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
                        if(row==0) { 
                            return ; 
                        }
                        IRow excelRow = sheet.GetRow(row);
                        if (excelRow == null)
                        {
                            excelRow = sheet.CreateRow(row);
                        }
                        ICell cell = excelRow.GetCell(column) ?? excelRow.CreateCell(column);
                        if (content.Equals("1"))
                        {
                            cell.SetCellValue(1);

                        }
                        else if (content.Equals("0"))
                        {
                            cell.SetCellValue(0);

                        }
                        else
                        {
                            cell.SetCellValue(content);


                        }

                        //var formulaEvaluator = workbook.GetCreationHelper().CreateFormulaEvaluator();

                        //// Evaluate the formulas
                        //formulaEvaluator.EvaluateAll();

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
        /// This function is called when the user click "enregistrer et quitter" in the inventory update.
        /// It will add the current date end the user name (name of the session) at the end of the document in the two colums "DATE de MISE A JOUR" and "NOM + SIGNATURE"
        /// </summary>
        public void UpdateSignature()
        {
            string user = Environment.UserName;
            System.DateTime date = DateTime.Now;
            string dateStr=date.Day.ToString()+"/"+date.Month.ToString()+"/"+date.Year.ToString();
            CellInfo SignatureCell = FindValue("NOM + SIGNATURE",-1);
            CellInfo DateCell = FindValue("DATE de MISE A JOUR",-1);

            int i = SignatureCell.Row;
            while(!GetCellValue(SignatureCell.Sheet,i,SignatureCell.Column).Equals(""))
            {
                i++;

            }
            this.SetCellValue(SignatureCell.Sheet, user, SignatureCell.Column,i) ;

            i = SignatureCell.Row;
            while (!GetCellValue(DateCell.Sheet, i, DateCell.Column).Equals(""))
            {
                i++;

            }
            this.SetCellValue(DateCell.Sheet, dateStr, DateCell.Column, i);


        }
        /// <summary>
        /// Go through all excel file and update the  cell according to their value. This is used because just changing the value with the function SetCellValue doesn't do anything.
        /// </summary>
        public void UpdateExcelFormula()
        {

            try
            {
                if (FilePath.IndexOf(".xlsx") > 0)
                {
                    using (var file = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                    {
                        XSSFWorkbook workbook = new XSSFWorkbook(file);
                        
                        var formulaEvaluator = workbook.GetCreationHelper().CreateFormulaEvaluator();

                        // Evaluate the formulas
                        try
                        {
                            formulaEvaluator.EvaluateAll();

                        }
                        catch(Exception ex) { }

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


                        var formulaEvaluator = workbook.GetCreationHelper().CreateFormulaEvaluator();

                        // Evaluate the formulas
                        try 
                        {
                            formulaEvaluator.EvaluateAll();

                        }
                        catch (Exception ex) { }

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
        public CellInfo FindValue(string content, int columnIndex)
        {
            if (columnIndex < 0) //if we don't know in which colum search
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
                                for (int columnIndexIndice = 0; columnIndexIndice < row.LastCellNum; columnIndexIndice++)
                                {
                                    ICell cell = row.GetCell(columnIndexIndice);

                                    if (cell != null && cell.CellType == CellType.String && cell.StringCellValue.Equals(content))
                                    {
                                        // Cell with matching content found, populate the CellInfo struct
                                        cellInfo.Row = rowIndex;
                                        cellInfo.Column = columnIndexIndice;

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

            else if (FilePath.IndexOf(".xls") > 0)
            {


                using (var file = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                {
                    HSSFWorkbook workbook;
                    try
                    {
                        workbook = new HSSFWorkbook(file);
                    }
                    catch(Exception e) //problem with file format, abort da mission
                    {
                        return null;
                    } 


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
                                for (int columnIndexIndice = 0; columnIndexIndice < row.LastCellNum; columnIndexIndice++)
                                {
                                    ICell cell = row.GetCell(columnIndexIndice);

                                    if (cell != null && cell.CellType == CellType.String && cell.StringCellValue == content)
                                    {
                                        // Cell with matching content found, populate the CellInfo struct
                                        cellInfo.Row = rowIndex;
                                        cellInfo.Column = columnIndexIndice;
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

            else //we know in which colmn to search
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
                                  
                                        ICell cell = row.GetCell(columnIndex);

                                        if (cell != null && cell.CellType == CellType.String && cell.StringCellValue.Equals(content))
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
                



                else if (FilePath.IndexOf(".xls") > 0)
                {


                    using (var file = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                    {
                        HSSFWorkbook workbook;
                        try
                        {
                            workbook = new HSSFWorkbook(file);
                        }
                        catch (Exception e) //problem with file format, abort da mission
                        {
                            return null;
                        }


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

                                    ICell cell = row.GetCell(columnIndex);

                                    if (cell != null && cell.CellType == CellType.String && cell.StringCellValue.Equals(content))
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



                // Cell with matching content not found
                cellInfo.Sheet = null; // return -1 for sheet by default if nothing is found
                return cellInfo;

            }
        }



        /// <summary>
        /// find the first cell in a given file (.xls or .xlsx) containing a given string
        /// 
        /// </summary>
        /// <param name="content">the string that must be found </param>
        /// <returns> the method return a CellInfo struct {int row, int colums, int sheetNumber} .
        /// if nothing is found the struct returned has the sheet field set to -1</returns>  
        public CellInfo FindValue2(string content)
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

                                    if (cell != null && cell.CellType == CellType.String && cell.StringCellValue.Equals(content))
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



            else if (FilePath.IndexOf(".xls") > 0)
            {


                using (var file = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                {
                    HSSFWorkbook workbook;
                    try
                    {
                        workbook = new HSSFWorkbook(file);
                    }
                    catch (Exception e) //problem with file format, abort da mission
                    {
                        return null;
                    }


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





