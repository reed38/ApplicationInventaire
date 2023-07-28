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
    /// Used to store Path of the application data as well as other infomations such as name, description author.
    /// </summary>

    public class ProjectInfos
    {

        #region variables

        [PrimaryKey, AutoIncrement]
        int id { get; set; }

        #endregion

        #region set_get
        public string DatabasePath { set; get; }//Path to the Database used by the project
        public string ExcelPath { set; get; }
        public string AppPath { set; get; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserData");
        public string TmpPath { set; get; }
        public string TmpExcelPath { set; get; }
        public string ProjectPath { set; get; }
        public string ImagePath { set; get; }
        public string ImageSectionPath { set; get; }//path to the folder where Section image are stored
        public string ImageRelevePath { set; get; } //path to the folder where Releve images are stored
        public string PlansPath { set; get; } //path to the folder where plans pdf are stored
        public string ProjectName { set; get; }
        public string Author { set; get; } //person who created the project
        public DateTime CreationDate { set; get; }
        public string LastEditor { set; get; }
        public DateTime LastEditionDate { set; get; }
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
            this.ExcelPath = Path.Combine(this.ProjectPath, "Excel.xls");
            this.TmpPath = Path.Combine(this.ProjectPath, "Tmp");
            this.TmpExcelPath = Path.Combine(TmpPath, "tmp.xls");
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

        public ProjectData(ProjectInfos project)
        {



            File.Copy(project.ExcelPath, project.TmpExcelPath, true);



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
                this.myDatabase = new Database(myProjectInfos.DatabasePath);
                mySections = new List<Section>();
                this.myProjectInfos = project;
                this.myProjectInfos.CreationDate = DateTime.Now;
                this.myProjectInfos.Author= Environment.UserName;
                this.myProjectInfos.LastEditor = Environment.UserName;
                this.myProjectInfos.LastEditionDate = DateTime.Now;

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

        /// <summary>
        /// Used to Initialize ImageReleveList with the list of the image name and path used for releve
        /// </summary>
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
                pieceNames.Add((TagCellTmp.Content));
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

            List<(CellInfo, CellInfo, int, string)> CellData = new List<(CellInfo, CellInfo, int, string)>();


            if (PresentCell == null || PIDtCell == null || AmountCell == null || ConstructorCell == null)
            {

                return;
            }

            int n = PresentCell.Row + 1;
            string res;
            do
            {
                res = myTmpExcelFile.GetCellValue(PIDtCell.Sheet, n, AmountCell.Column);

                CellInfo TagCellTmp = new CellInfo(n, PIDtCell.Column, PIDtCell.Sheet, myTmpExcelFile.GetCellValue(PIDtCell.Sheet, n, PIDtCell.Column));

                CellInfo ContentCellTmp = new CellInfo(n, PresentCell.Column, PresentCell.Sheet, myTmpExcelFile.GetCellValue(PIDtCell.Sheet, n, PresentCell.Column));

                string description = myTmpExcelFile.GetCellValue(PIDtCell.Sheet, n, DescriptionCell.Column);

                ////IsReleveRequire, we check if the cell color is yellow
                int isYellow;
                byte[] color = myTmpExcelFile.GetCellColor(ConstructorCell.Sheet, n, ConstructorCell.Column);

                if (color[0] == 255 && color[1] == 255 && color[2] == 0) //we test if the color of the cell is yellow
                {
                    isYellow = 1;
                }
                else
                {
                    isYellow = 0;
                }
                CellData.Add((TagCellTmp, ContentCellTmp, isYellow, description));
                n++;


            } while (!res.Equals(""));


            for (int i = 0; i < this.mySections.Count; i++)
            {
                for (int j = 0; j < this.mySections[i].PiecesList.Count; j++)
                {
                    foreach (var k in CellData)
                    {
                        Piece tmp = this.mySections[i].PiecesList[j];
                        if (k.Item1.Content.Equals(this.mySections[i].PiecesList[j].PieceName))
                        {
                            if (k.Item2.Content.Equals("1"))
                            {
                                this.mySections[i].PiecesList[j].IsPresent = 1;
                            }
                            else
                            {
                                this.mySections[i].PiecesList[j].IsPresent = 0;
                            }
                            this.mySections[i].PiecesList[j].IsReleveRequired = k.Item3;


                            this.mySections[i].PiecesList[j].SheetName = PresentCell.Sheet;
                            this.mySections[i].PiecesList[j].ExcelColumn = PresentCell.Column;
                            this.mySections[i].PiecesList[j].ExcelRow = k.Item2.Row;
                            this.mySections[i].PiecesList[j].Description = k.Item4;


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
            myProjectInfos.DispDataProjectInfos();
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
            File.Copy(myProjectInfos.ExcelPath, myProjectInfos.TmpExcelPath, true);
        }
    }
    #endregion


}
















