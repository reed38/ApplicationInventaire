using System.Security.Policy;
using System.Xml;
using SQLite;
using System.Collections.Generic;
using ApplicationInventaire.Core;
using System.IO;

using System;

using ApplicationInventaire.Core.Config;
using ApplicationInventaire.Core.DatabaseManagement;
using ApplicationInventaire.Core.ExcelManagement;
using ApplicationInventaire.Core.GlobalPages;
using ApplicationInventaire.Core.PieceSections;
using ApplicationInventaire.Core.ProjectDataSet;
using ApplicationInventaire.Core.GlobalProjectData;







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
    /// used to contain image path and image name
    /// </summary>

    public class ProjectInfos
    {

        #region variables

        [PrimaryKey, AutoIncrement]
        int id { get; set; }

        #endregion

        #region set_get
        public string DatabasePath { set; get; }
        public string ExcelPath { set; get; }
        public string AppPath { set; get; } = AppDomain.CurrentDomain.BaseDirectory;
        public string TmpPath { set; get; } 
        public string TmpExcelPath { set; get; }

        public string ProjectPath { set; get; }
        public string ImagePath { set; get; }
        public string ImageSectionPath { set; get; }
        public string ImageRelevePath { set; get; }

        public string ProjectName { set; get; }
        public string Responsable { set; get; }
        public string Description { set; get; }


        #endregion

        #region constructor
        public ProjectInfos(string ProjectName)
        {
            this.ProjectName = ProjectName;
            this.ProjectPath = Path.Combine(AppPath, "UserData/" + ProjectName);
            this.ImagePath = Path.Combine(this.ProjectPath, "Image");
            this.ImageSectionPath = Path.Combine(this.ImagePath, "ImageSection");
            this.ImageRelevePath = Path.Combine(this.ImagePath, "ImageReleve");
            this.DatabasePath = Path.Combine(this.ProjectPath, "Database.db");
            this.ExcelPath = Path.Combine(this.ProjectPath, "Excel_" + this.ProjectName + ".xls");
            this.TmpPath= Path.Combine(this.ProjectPath, "Tmp");
            this.TmpExcelPath = Path.Combine(TmpPath, this.ProjectName + "tmp.xls");
            this.InitializeFileTree();



        }

        public ProjectInfos() { }

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
            CreateFile(this.DatabasePath);


        }
        private void CreateFile(string FilePath)
        {
            if (FilePath != null && !File.Exists(FilePath))
            {
                FileStream fileStream = File.Create(FilePath);
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
        public void DispDataProjectInfos()
        {
            Console.WriteLine("ProjectName: " + this.ProjectName);
            Console.WriteLine("Description: " + this.Description);
            Console.WriteLine("Responsable: " + Responsable);

            Console.WriteLine("database path: " + this.DatabasePath);
            Console.WriteLine("ExcelPath: " + this.ExcelPath);
            Console.WriteLine("ProjectPath: " + this.ProjectPath);
            Console.WriteLine("ImagePath: " + this.ImagePath);
            Console.WriteLine("ImageSectionPath: " + this.ImageSectionPath);
            Console.WriteLine("ImageRelevePath: " + this.ImageRelevePath);



        }

        #endregion






    }


    public class ProjectData
    {


        #region variables
        public ProjectInfos myProjectInfos;
        public List<Section> mySections;
        public ExcelFile myExcelFile;
        public ExcelFile myTmpExcelFile;
        public Database myDatabase;

        #endregion


        #region set_get
        #endregion

        #region constructor
        public ProjectData(ProjectInfos project, List<Section> mySections)
        {
             File.Copy(project.ExcelPath, project.ExcelPath, true);

            this.mySections = mySections;
            this.myExcelFile = new ExcelFile(myProjectInfos.ExcelPath);
            this.myTmpExcelFile = new ExcelFile(myProjectInfos.TmpExcelPath);
            this.myDatabase = new Database(myProjectInfos.DatabasePath);


        }

        public ProjectData(ProjectInfos project)
        {
            try
            {
                File.Copy(project.TmpExcelPath, project.ExcelPath, true);

            }
            catch (Exception ex) { }

            Database database = new Database(project.DatabasePath);
            if (database.IsDatabaseInitialized())
            {

                this.myProjectInfos = database.ReadProjectInfos();
                this.mySections = database.GetAllSections();
                this.myDatabase = database;
            }
            else
            {
                this.myProjectInfos = project;

                myDatabase = new Database(myProjectInfos.DatabasePath);
                mySections = new List<Section>();
                this.myProjectInfos = project;

            }
            myExcelFile = new ExcelFile(project.ExcelPath);
            this.myTmpExcelFile = new ExcelFile(project.TmpExcelPath);


        }


        #endregion



        #region methods


        /// <summary>
        /// Used to save to database the ProjecData object
        /// </summary>
        public void Save()
        {
            myDatabase.InsertAllSections(mySections);
            myDatabase.WriteProjectInfos(myProjectInfos);

        }

        /// <summary>
        /// Load Section and Piece information to the ProjectData object from the database file
        /// </summary>
        public void Load()
        {
            mySections = myDatabase.GetAllSections();
            myProjectInfos = myDatabase.ReadProjectInfos();
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

        public void InitializePieceFromExcel()
        {
            CellInfo PresentCell = this.myExcelFile.FindValue("Pr�sent");
            foreach (Section i in this.mySections)
            {
                foreach (Piece j in i.PiecesList)
                {
                    CellInfo cellInfo = myTmpExcelFile.FindValue(j.PieceName);
                    if (cellInfo.Sheet != null)
                    {
                        j.ExcelColumn = cellInfo.Column;
                        j.ExcelRow = cellInfo.Row;
                        j.SheetName = cellInfo.Sheet;
                        string cellPresentValue = this.myTmpExcelFile.GetCellValue(cellInfo.Sheet, cellInfo.Row, PresentCell.Column);
                        if (cellPresentValue.IndexOf("1") > 0)
                        {
                            j.IsPresent = 1;

                        }
                        else
                        {
                            j.IsPresent = 0;

                        }
                    }
                }
            }

        }


        #endregion

        #region debuging
        public void DispDataProjectData()
        {
            myProjectInfos.DispDataProjectInfos();
            foreach (Section tmp in mySections)
            {
                tmp.DispDataSection();
            }

        }

        #endregion















    }
}




