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
using System.Runtime.CompilerServices;
using System.Linq;

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
        public string AppPath { set; get; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"UserData");
        public string TmpPath { set; get; }
        public string TmpExcelPath { set; get; }
        public string ProjectPath { set; get; }
        public string ImagePath { set; get; }
        public string ImageSectionPath { set; get; }
        public string ImageRelevePath { set; get; }
        public string PlansPath { set; get; }
        public string ProjectName { set; get; }
        public string Author { set; get; }
        public string Description { set; get; }


        #endregion

        #region constructor
        public ProjectInfos(string ProjectName)
        {
            this.ProjectName = ProjectName;
            this.ProjectPath = Path.Combine(AppPath, ProjectName);
            this.ImagePath = Path.Combine(this.ProjectPath, "Image");
            this.ImageSectionPath = Path.Combine(this.ImagePath, "ImageSection");
            this.ImageRelevePath = Path.Combine(this.ImagePath, "ImageReleve");
            this.DatabasePath = Path.Combine(this.ProjectPath, "Database.db");
            this.ExcelPath = Path.Combine(this.ProjectPath, "Excel_" + this.ProjectName + ".xls");
            this.TmpPath = Path.Combine(this.ProjectPath, "Tmp");
            this.TmpExcelPath = Path.Combine(TmpPath, this.ProjectName + "tmp.xls");
            this.PlansPath = Path.Combine(ProjectPath, "Plans");
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
        public void DispDataProjectInfos()
        {
            Console.WriteLine("ProjectName: " + this.ProjectName);
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


    public class ProjectData
    {


        #region variables
        public ProjectInfos myProjectInfos;
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


                File.Copy(project.ExcelPath, project.TmpExcelPath, true);
            }
            catch (Exception hehehaha) { }




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
            GetRelevesNames();
            GetSectionsNames();
            this.InitializePieceFromExcel();


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

            string ImageSectionPath = this.myProjectInfos.ImageSectionPath;
            string[] path = Directory.GetFiles(ImageSectionPath);
            ImageInfos[] result = new ImageInfos[path.Length];

            for (int i = 0; i < path.Length; i++)
            {
                result[i] = new ImageInfos(Path.GetFileNameWithoutExtension(path[i]), path[i]);

            }

            this.ImageSectionList = result;

            }
        public void GetRelevesNames()
        {

            string ImageRelevePath = this.myProjectInfos.ImageRelevePath;
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
            CellInfo PresentCell = this.myTmpExcelFile.FindValue("Présent");
            CellInfo PIDtCell = this.myTmpExcelFile.FindValue("PID ");
            CellInfo AmountCell=this.myTmpExcelFile.FindValue("Besoin Qté Totale");
            List<CoupleCellInfo> coupleCellInfo = new List<CoupleCellInfo>();

            List<string> PieceAcount=new List<string>();

            int n = PresentCell.Row + 1;
            string res;
            do
            {
                res = myTmpExcelFile.GetCellValue(PIDtCell.Sheet, n, AmountCell.Column);
                CellInfo TagCellTmp = new CellInfo(n, PIDtCell.Column, PIDtCell.Sheet, myTmpExcelFile.GetCellValue(PIDtCell.Sheet, n, PIDtCell.Column));
                CellInfo ContentCellTmp = new CellInfo(n, PresentCell.Column, PresentCell.Sheet, myTmpExcelFile.GetCellValue(PIDtCell.Sheet, n, PresentCell.Column));
                coupleCellInfo.Add(new CoupleCellInfo(TagCellTmp, ContentCellTmp));
                n++;

            } while (!res.Equals(""));


            for (int i = 0; i < this.mySections.Count; i++)
            {
                for (int j = 0; j < this.mySections[i].PiecesList.Count; j++)
                {
                    foreach (CoupleCellInfo k in coupleCellInfo)
                    {
                        Piece tmp = this.mySections[i].PiecesList[j];
                        if (k.tag.Content.Equals(this.mySections[i].PiecesList[j].PieceName))
                        {
                            if (k.Present.Content.Equals("1"))
                            {
                                this.mySections[i].PiecesList[j].IsPresent = 1;
                            }
                            else
                            {
                                this.mySections[i].PiecesList[j].IsPresent = 0;
                            }


                            this.mySections[i].PiecesList[j].SheetName = PresentCell.Sheet;
                            this.mySections[i].PiecesList[j].ExcelColumn = PresentCell.Column;
                            this.mySections[i].PiecesList[j].ExcelRow = k.Present.Row;
                            this.mySections[i].PiecesList[j].Amount=myTmpExcelFile.GetCellValue()


                        }

                    }
                }
            }
        }
        //    foreach (Section i in this.mySections)
        //    {
        //        foreach (Piece j in i.PiecesList)
        //        {

        //            foreach (CoupleCellInfo k in coupleCellInfo)
        //            {
        //                if (k.tag.Content.Equals(j.PieceName))
        //                {
        //                    if (k.Present.Content.Equals("1"))
        //                    {
        //                        j.IsPresent = 1;
        //                    }
        //                    else
        //                    {
        //                        j.IsPresent = 0;
        //                    }


        //                    j.SheetName = PresentCell.Sheet;
        //                    j.ExcelColumn = PresentCell.Column;
        //                    j.ExcelRow = k.Present.Row;


        //                }

        //            }
        //        }

        //    }
        //}


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
    }
    #endregion


}
















