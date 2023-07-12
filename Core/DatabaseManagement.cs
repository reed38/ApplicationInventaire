

using SQLite;
using System.ComponentModel;
using System;
using System.Collections.Generic;

using ApplicationInventaire.Core.Config;
using ApplicationInventaire.Core.DatabaseManagement;
using ApplicationInventaire.Core.ExcelManagement;
using ApplicationInventaire.Core.GlobalPages;
using ApplicationInventaire.Core.PieceSections;
using ApplicationInventaire.Core.ProjectDataSet;

/// <summary>
/// used to manage database
/// </summary>
namespace ApplicationInventaire.Core.DatabaseManagement
{
    /// <summary>
    /// To create Database object. A database Object has the methods to read and write Objects in a .db sqlite database file. These objects are Section, Piece, and ProjectInfos
    /// the sqlite-net-pcl library let you directly create Table in the database to stock objects. You can then get those objects using a system of pimarykey.
    ///  But it DOESN'T ALLOW complex types such as bol or nested objects.
    /// There is the possibility to create a relation between tables using secondary key.
    /// DO NOT write a Section with Id=, it will not work. I don't know why it must be a maraboutage
    /// </summary>
    /// <summary>
    /// To create Database object. A database Object has the methods to read and write Objects in a .db sqlite database file. These objects are Section, Piece, and ProjectInfos
    /// the sqlite-net-pcl library let you directly create Table in the database to stock objects. You can then get those objects using a system of pimarykey.
    ///  But it DOESN'T ALLOW complex types such as bol or nested objects.
    /// There is the possibility to create a relation between tables using secondary key.
    /// DO NOT write a Section with Id=, it will not work. I don't know why it must be a maraboutage
    /// </summary>
    public class Database
    {
        #region variables
        public SQLiteConnection connection;

        #endregion

        #region constructor
        /// <summary>
        /// Initialize the Database with the corresponding tables
        /// </summary>
        /// <param name="databasePath"></param>
        public Database(string databasePath)
        {
            connection = new SQLiteConnection(databasePath);
            connection.CreateTable<Piece>();
            connection.CreateTable<Section>();
            connection.CreateTable<ProjectInfos>();
        }
        #endregion

        #region piece_section
        /// <summary>
        /// To insert a Piece object in the databse
        /// </summary>
        /// <param name="piece">Piece object</param>
        public void InsertPiece(Piece piece)
        {
            connection.Insert(piece);
        }
        /// <summary>
        ///To get a Piece object from the database using the primaryKey id
        /// </summary>
        /// <param name="id">id of the object</param>
        /// <returns></returns>
        public Piece GetPiece(int id)
        {
            return connection.Table<Piece>().FirstOrDefault(s => s.Id == id);
        }

        /// <summary>
        /// To insert a section in the database
        /// </summary>
        /// <param name="section"><Section to insert/param>
        public void InsertSection(Section section)
        {
            connection.Insert(section);
        }
        /// <summary>
        /// To get a Section in the database
        /// </summary>
        /// <param name="id">Id of the Section object to retrieve</param>
        /// <returns></returns>
        public Section GetSection(int id)
        {
            return connection.Table<Section>().FirstOrDefault(s => s.Id == id);
        }


        /// <summary>
        /// Return a List containing all the Section present in the database.
        /// </summary>
        /// <returns>List of Section</returns>
        public List<Section> GetAllSections()
        {
            List<Section> sections = connection.Table<Section>().ToList();
            foreach (Section section in sections)
            {
                section.PiecesList = connection.Table<Piece>().Where(p => p.SectionId == section.Id).ToList();
            }
            return sections;
        }

        /// <summary>
        /// To insert al the given Section in the Databse. This functions will reset the Piece table and the Section table by doing so.
        /// </summary>
        /// <param name="sections"></param>

        public void InsertAllSections(List<Section> sections)
        {
            connection.DropTable<Section>(); //reset the Primary Key Counter. Code doesn't work if this line isn't here, because Piece.SectionId doen't match 
            connection.CreateTable<Section>();
            connection.DropTable<Piece>();
            connection.CreateTable<Piece>();



            foreach (var section in sections)
            {
                connection.Insert(section);
                connection.InsertAll(section.PiecesList);
            }
        }

        //Project Info
        #endregion

        #region ProjectInfos

        /// <summary>
        /// Return the ProjectInfos object contained in the Database
        /// </summary>
        /// <returns>ProjectInfos corresponding to the given project</returns>
        public ProjectInfos ReadProjectInfos()
        {
            List<ProjectInfos> tmp = connection.Table<ProjectInfos>().ToList();
            if (tmp.Count > 0)
                return tmp[0];

            else
                return null;
        }
        /// <summary>
        /// 
        /// Take a given ProjectInfos object and write it in the the database. By doing do it overwrtite the last ProjectInfo Object present in it
        /// </summary>
        /// <param name="project"></param>
        public void WriteProjectInfos(ProjectInfos project)
        {
            connection.DropTable<ProjectInfos>();
            connection.CreateTable<ProjectInfos>();
            connection.Insert(project);

        }

        #endregion

        #region methods
        public bool IsDatabaseInitialized()
        {
            if (this.GetAllSections().Count < 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        /// <summary>
        /// Used for debugging. Display all the data in the database
        /// </summary>
        public void DispDataDatabase()

        {
            List<Section> mysections = this.GetAllSections();
            foreach (Section i in mysections)
            {
                i.DispDataSection();
            }
            ProjectInfos tmp = ReadProjectInfos();
            tmp.DispDataProjectInfos();
        }

    }

}

