using System.Security.Policy;
using System.Xml;
using SQLite;
using System.Collections.Generic;
using ApplicationInventaire.Core;
using System.IO;

using System;

using ApplicationInventaire.Core.DatabaseManagement;
using ApplicationInventaire.Core.ExcelManagement;
using ApplicationInventaire.Core.GlobalPages;
using ApplicationInventaire.Core.PieceSections;
using System.Runtime.CompilerServices;
using System.Linq;
using NPOI.OpenXmlFormats.Dml.Diagram;
using NPOI.SS.UserModel;
using static SQLite.SQLite3;
using ApplicationInventaire.Core.ProjectDataSet;

/// <summary>
/// This file contains Class ImageInfos, TemplateInfos, and ProjectData.
/// </summary>
namespace ApplicationInventaire.Core.ProjectDataSet
{




    /// <summary>
    /// used to contain image path and image name
    /// </summary>
    public class ImageInfos
    {
        #region variables
        public string Name { set; get; }
        public string Path { set; get; }
        #endregion
        #region constructor
        public ImageInfos(string name, string path)
        {
            this.Name = name;
            this.Path = path;
        }


        #endregion



        public void DispImageInfo()
        {
            Console.WriteLine("name: " + this.Name);
            Console.WriteLine("Path: " + this.Path);
        }
    }


    /// <summary>
    /// Used to store Path of a template data as well as other infomations such as name, description, author, creation Date,last edit Date.
    /// </summary>

    public class TemplateInfos
    {

        #region variables

        [PrimaryKey, AutoIncrement]
        int id { get; set; }

        #endregion

        #region set_get
        public string DatabasePath { set; get; }//Path to the Database used by the project
        public string ExcelPath { set; get; }
        public string AppPath { set; get; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserData");
        public string RessourcesPath { set; get; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
        public string TmpPath { set; get; }
        public string TmpExcelPath { set; get; }
        public string ProjectPath { set; get; }
        public string ImagePath { set; get; }
        public string ImageSectionPath { set; get; }//path to the folder where Section image are stored
        public string ImageRelevePath { set; get; } //path to the folder where Releve images are stored
        public string PlansPath { set; get; } //path to the folder where plans pdf are stored
        public string TemplateName { set; get; }
        public string Author { set; get; } //person who created the project
        public DateTime CreationDate { set; get; }
        public string LastEditor { set; get; }
        public DateTime LastEditionDate { set; get; }
        public string Description { set; get; }


        #endregion

        #region constructor
        public TemplateInfos(string ProjectName)
        {
            this.TemplateName = ProjectName;
            this.ProjectPath = Path.Combine(AppPath, ProjectName);
            this.ImagePath = Path.Combine(this.ProjectPath, "Image");
            this.ImageSectionPath = Path.Combine(this.ImagePath, "ImageSection");
            this.ImageRelevePath = Path.Combine(this.ImagePath, "ImageReleve");
            this.DatabasePath = Path.Combine(this.ProjectPath, "Database.db");
            this.ExcelPath = Path.Combine(this.ProjectPath, "Excel.xls");
            this.TmpPath = Path.Combine(this.ProjectPath, "Tmp");
            this.TmpExcelPath = Path.Combine(TmpPath, "tmp.xls");
            this.PlansPath = Path.Combine(ProjectPath, "Plans");
            this.InitializeFileTree();



        }

        public TemplateInfos() { }

        #endregion

        #region constructorMethod
        private void InitializeFileTree()
        {
            CreateDirectory(AppPath);
            CreateDirectory(this.ProjectPath);
            CreateDirectory(this.ImagePath);
            CreateDirectory(this.ImageSectionPath);
            CreateDirectory(this.ImageRelevePath);
            CreateDirectory(this.TmpPath);
            CreateDirectory(this.PlansPath);
            CreateFile(this.DatabasePath);
            CreateFile(this.ExcelPath);
            CreateFile(this.TmpExcelPath);


        }
        private void CreateFile(string FilePath)
        {
            if (FilePath != null && !File.Exists(FilePath))
            {
                FileStream fileStream = File.Create(FilePath);
                fileStream.Close();
            }
        }
        private void CreateDirectory(string DirectoryPath)
        {
            if (DirectoryPath != null && !Directory.Exists(DirectoryPath))
            {

                Directory.CreateDirectory(DirectoryPath);
            }
        }
        #endregion




        #region debugging
        public void DispDataTemplateInfos()
        {
            Console.WriteLine("ProjectName: " + this.TemplateName);
            Console.WriteLine("Description: " + this.Description);
            Console.WriteLine("Author: " + Author);

            Console.WriteLine("database path: " + this.DatabasePath);
            Console.WriteLine("ExcelPath: " + this.ExcelPath);
            Console.WriteLine("ProjectPath: " + this.ProjectPath);
            Console.WriteLine("ImagePath: " + this.ImagePath);
            Console.WriteLine("ImageSectionPath: " + this.ImageSectionPath);
            Console.WriteLine("ImageRelevePath: " + this.ImageRelevePath);



        }

        #endregion






    }

    /// <summary>
    /// Class xhich contains al the methods and data used to manage a given template.
    /// </summary>
    public class TemplateData
    {


        #region variables
        public TemplateInfos myTemplateInfos;
        public List<Section> mySections;
        public ExcelFile myExcelFile;
        public ExcelFile myTmpExcelFile;
        public Database myDatabase;
        public ImageInfos[] ImageSectionList;
        public ImageInfos[] ImageReleveList;

        #endregion


        #region set_get
        #endregion

        #region constructor

        public TemplateData(TemplateInfos template)
        {



            File.Copy(template.ExcelPath, template.TmpExcelPath, true);



            Database database = new Database(template.DatabasePath);
            if (database.IsDatabaseInitialized())
            {

                this.myTemplateInfos = database.ReadTemplateInfos();
                this.mySections = database.GetAllSections();
                this.myDatabase = database;
            }
            else
            {
                this.myTemplateInfos = template;
                this.myDatabase = new Database(myTemplateInfos.DatabasePath);
                mySections = new List<Section>();
                this.myTemplateInfos = template;
                this.myTemplateInfos.CreationDate = DateTime.Now;
                this.myTemplateInfos.Author= Environment.UserName;
                this.myTemplateInfos.LastEditor = Environment.UserName;
                this.myTemplateInfos.LastEditionDate = DateTime.Now;

            }
            myExcelFile = new ExcelFile(template.ExcelPath);
            this.myTmpExcelFile = new ExcelFile(template.TmpExcelPath);
            GetRelevesNames();
            GetSectionsNames();
           


        }


        #endregion



        #region methods

        /// <summary>
        /// used to get an array of ImageInfos object (containing file name ithout extension and file path) corresponding to a given oject
        /// </summary>
        /// <param name="project"> name of the project</param>
        /// <returns></returns>
        public void GetSectionsNames()
        {

            string ImageSectionPath = this.myTemplateInfos.ImageSectionPath;
            string[] path = Directory.GetFiles(ImageSectionPath);
            ImageInfos[] result = new ImageInfos[path.Length];

            for (int i = 0; i < path.Length; i++)
            {
                result[i] = new ImageInfos(Path.GetFileNameWithoutExtension(path[i]), path[i]);

            }

            this.ImageSectionList = result;

        }

        /// <summary>
        /// Used to Initialize ImageReleveList with the list of the image name and path used for releve
        /// </summary>
        public void GetRelevesNames()
        {

            string ImageRelevePath = this.myTemplateInfos.ImageRelevePath;
            string[] path = Directory.GetFiles(ImageRelevePath);
            ImageInfos[] result = new ImageInfos[path.Length];

            for (int i = 0; i < path.Length; i++)
            {
                result[i] = new ImageInfos(Path.GetFileNameWithoutExtension(path[i]), path[i]);
            }
            this.ImageReleveList = result;
        }




        /// <summary>
        /// Used to save to database the ProjecData object
        /// </summary>
        public void Save()
        {
            myDatabase.InsertAllSections(mySections);
            myDatabase.WriteTemplateInfos(myTemplateInfos);

        }

        /// <summary>
        /// Load Section and Piece information to the ProjectData object from the database file
        /// </summary>
        public void Load()
        {
            mySections = myDatabase.GetAllSections();
            myTemplateInfos = myDatabase.ReadTemplateInfos();
        }

        /// <summary>
        /// Initialize all of the pieces as non-present
        /// </summary>
        public void ResetPiecePresent()
        {
            foreach (Section i in this.mySections)
            {
                foreach (Piece j in i.PiecesList)
                {
                    j.IsPresent = 0;
                }
            }
        }



        /// <summary>
        /// Used to return the nameTag presents in the given excel file
        /// </summary>
        /// <returns>List<string> containing the nameTags</string></returns>
        public List<string> GetPieceNames()
        {
            CellInfo PIDtCell = this.myTmpExcelFile.FindValue("PID ");
            CellInfo AmountCell = this.myTmpExcelFile.FindValue("Besoin Qté Totale");
            CellInfo ConstructorCell = this.myTmpExcelFile.FindValue("FABRICANT");



            List<string> pieceNames = new List<string>();

            if (PIDtCell == null || AmountCell == null)
            {

                return null;
            }

            int n = AmountCell.Row + 1;
            string res;
           
            do
            {
                res = myTmpExcelFile.GetCellValue(PIDtCell.Sheet, n, AmountCell.Column);

                CellInfo TagCellTmp = new CellInfo(n, PIDtCell.Column, PIDtCell.Sheet, myTmpExcelFile.GetCellValue(PIDtCell.Sheet, n, PIDtCell.Column));
                if (!string.IsNullOrEmpty(TagCellTmp.Content) && TagCellTmp.Content.IndexOf(".M")==-1)
                {
                    pieceNames.Add((TagCellTmp.Content));
                }
                n++;


            } while (!res.Equals(""));



            return pieceNames;


        }

       











        /// <summary>
        /// Go through the excelfile and initialize the data structure using it. It is used to know which piece is already present and which one require its serial number to be be written down
        /// </summary>
        public void InitializePieceFromExcel()
        {
            CellInfo PresentCell = this.myTmpExcelFile.FindValue("Présent");
            CellInfo PIDtCell = this.myTmpExcelFile.FindValue("PID ");
            CellInfo AmountCell = this.myTmpExcelFile.FindValue("Besoin Qté Totale");
            CellInfo ConstructorCell = this.myTmpExcelFile.FindValue("FABRICANT");
            CellInfo DescriptionCell = this.myTmpExcelFile.FindValue("Désignation");

            List<Piece> Pieces= new List<Piece>();




            if (PresentCell == null || PIDtCell == null || AmountCell == null || ConstructorCell == null)
            {

                return;
            }

            int n = PresentCell.Row + 1;
            string res;
            do //getting all the pieces
            {
                Piece tmpPiece;
                res = myTmpExcelFile.GetCellValue(PIDtCell.Sheet, n, AmountCell.Column);

                CellInfo PID = new CellInfo(n, PIDtCell.Column, PIDtCell.Sheet, myTmpExcelFile.GetCellValue(PIDtCell.Sheet, n, PIDtCell.Column));

                CellInfo Present = new CellInfo(n, PresentCell.Column, PresentCell.Sheet, myTmpExcelFile.GetCellValue(PIDtCell.Sheet, n, PresentCell.Column));

                string description = myTmpExcelFile.GetCellValue(PIDtCell.Sheet, n, DescriptionCell.Column);

                ////IsReleveRequire, we check if the cell color is yellow
                int isYellow;
                byte[] color = myTmpExcelFile.GetCellColor(ConstructorCell.Sheet, n, ConstructorCell.Column);

                if (color != null && color[0] == 255 && color[1] == 255 && color[2] == 0) //we test if the color of the cell is yellow
                {
                    isYellow = 1;
                }
                else
                {
                    isYellow = 0;
                }

                if (!string.IsNullOrEmpty(PID.Content))
                {

                    int presentInt = (Present.Content.Equals("1")) ? 1 : 0;
                    tmpPiece = new Piece(PresentCell.Column, n, PID.Content, description, presentInt, isYellow);                            
                    Pieces.Add(tmpPiece);

                    

                }


                n++;


            } while (!res.Equals(""));

            foreach (Piece p1 in Pieces) //going through all Piece with a tag ending with ".M", and updating the piece with the same name
            {
                foreach (Piece p2 in Pieces)
                {
                    if (p2.PieceName.Equals(p1.PieceName + ".M"))
                    {
                        p1.HasMarking = 1;
                        break;
                    }
                }

            }

            for (int i = 0; i < this.mySections.Count; i++)
            {
                for (int j = 0; j < this.mySections[i].PiecesList.Count; j++)
                {
                    foreach (var k in Pieces)
                    {
                        Piece tmp = this.mySections[i].PiecesList[j];
                        if (k.PieceName.Equals(this.mySections[i].PiecesList[j].PieceName))
                        {
                            Piece MyPiece = this.mySections[i].PiecesList[j];
                            MyPiece.IsReleveRequired = k.IsReleveRequired;
                            MyPiece.HasMarking = k.HasMarking;
                            MyPiece.SheetName = PresentCell.Sheet;
                            MyPiece.ExcelColumn = PresentCell.Column;
                            MyPiece.ExcelRow = k.ExcelRow;
                            MyPiece.Description = k.Description;
                            MyPiece.IsPresent = k.IsPresent;


                            //we use the color of the cell to determine if Serial number is required



                        }

                    }
                }
            }
        }
















        /// <summary>
        /// This functions will go through sections and delete Section from mySection List iftheir name is not present in ImageSectionList.
        /// It is useful in edit mode when the user delete a section

        /// </summary>
        public void UpdateSection()
        {

            for (int i = 0; i < this.mySections.Count; i++)
            {
                bool found = false;
                for (int j = 0; j < this.ImageSectionList.Length; j++)
                {
                    if (mySections[i].SectionName.Equals(ImageSectionList[i].Name))
                    {
                        found = true;
                        break;
                    }
                    if (found == false && j == ImageSectionList.Length - 1)
                    {
                        this.mySections.Remove(mySections[i]);
                    }
                }

            }

        }


        #endregion

        #region debuging
        /// <summary>
        /// used fo debugging and displaying all data relative to a ProjectData object in the terminal
        /// </summary>
        public void DispDataProjectData()
        {
            myTemplateInfos.DispDataTemplateInfos();
            foreach (Section tmp in mySections)
            {
                tmp.DispDataSection();
            }

        }

        /// <summary>
        /// This function copy the base excel file to the tmp excel file. It is usefull when starting again an inventory
        /// </summary>
        public void ResetTmpExcel()
        {
            File.Copy(myTemplateInfos.ExcelPath, myTemplateInfos.TmpExcelPath, true);
        }
    }
    #endregion

public static class GlobalData
{


    /// <summary>
    /// Used to get an array of string containing the list of the name of the projects present in the UserData folder
    /// </summary>
    /// <returns> an array of string</returns>
    public static string[] GetProjectNames()
    {
        string UserDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserData");
        string[] result = Directory.GetDirectories(UserDataPath);
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = Path.GetFileNameWithoutExtension(result[i]);

        }

        return (result);



    }

    /// <summary>
    /// Go through the directory User Data to return the paths of the directories of the differents templates
    /// </summary>
    /// <returns> string [] containing the paths</returns>
    public static string[] GetTemplatePaths()
    {
        string UserDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserData");
        string[] result = Directory.GetDirectories(UserDataPath);
        return result;

    }
    /// <summary>
    /// Go through the directory User Data to returs the names of the directories of the differents templates
    /// </summary>
    /// <returns> string [] containing the names</returns>
    public static string[] GetPlansNames(string templateName)
    {
        TemplateInfos tmp = new(templateName);
        string PlansPath = tmp.PlansPath;
        string[] result = Directory.GetFiles(PlansPath);
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = Path.GetFileNameWithoutExtension(result[i]);

        }

        return (result);

    }
    /// <summary>
    /// Go through the directory Plans to returs the paths of the plans presents in the given template
    /// </summary>
    /// <returns> string [] containing the names</returns>
    public static string[] GetPlansPath(string templateName)
    {
        TemplateInfos tmp = new(templateName);
        string PlansPath = tmp.PlansPath;
        string[] result = Directory.GetFiles(PlansPath);

        return (result);

    }

}
}


















