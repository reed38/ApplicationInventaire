/// <summary>
/// ce fichier contient l'ensembe des classes et méthodes utilisés
/// </summary>
using System.Linq.Expressions;
using EnumsNET;
using NPOI.OpenXmlFormats.Dml.Diagram;
using SQLite;
using System.Collections.Generic;

using System;

using ApplicationInventaire.Core.Config;
using ApplicationInventaire.Core.DatabaseManagement;
using ApplicationInventaire.Core.ExcelManagement;
using ApplicationInventaire.Core.GlobalPages;
using ApplicationInventaire.Core.PieceSections;
using ApplicationInventaire.Core.ProjectDataSet;


namespace ApplicationInventaire.Core.PieceSections
{
    public class Piece
    {
        #region set_get
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public double X { get; set; }
        public double Y { get; set; }
        public int ExcelColumn { get; set; }
        public int ExcelRow { get; set; }
        public string Comment { get; set; }
        public string SerialNumber { get; set; }
        public string SectionName { get; set; }
        public string PieceName { get; set; }
        public string SheetName { get; set; }
        public int IsPresent { get; set; }
        public int IsReleveRequired { get; set; }

        public int SectionId { get; set; } // Foreign key to Section

        #endregion

        #region constructor
        public Piece(int x, int y, int excelColumn, int excelRow, string comment, string pieceName, string SectionName, int isPresent, int isReleveRequired, int SectionId)
        {
            X = x;
            Y = y;
            ExcelColumn = excelColumn;
            ExcelRow = excelRow;
            Comment = comment;
            PieceName = pieceName;
            IsPresent = isPresent;
            IsReleveRequired = isReleveRequired;
            this.SectionId = SectionId;
            this.SectionName = SectionName;
        }

        public Piece()
        {

        }
        public Piece(int x, int y, string pieceName, int SectionId)
        {
            this.X = x;
            this.Y = y;
            this.PieceName = pieceName;
            this.SectionId = SectionId;
        }
        #endregion


        #region debugging
        /// <summary>
        /// used to display the inforamations about a piece
        /// </summary>
        public void DispDataPiece()
        {
            Console.WriteLine("Tag: " + PieceName);
            Console.WriteLine("Dans la section: " + SectionId);
            Console.WriteLine("x: " + X);
            Console.WriteLine("y: " + Y);
            Console.WriteLine("excel column: " + ExcelColumn);
            Console.WriteLine("excel row: " + ExcelRow);
            Console.WriteLine("comment: " + Comment);
            Console.WriteLine("Present: " + IsPresent);
            Console.WriteLine("Releve: " + IsReleveRequired);
            Console.WriteLine();
        }
        #endregion
    }

    public class Section
    {
        #region set_get
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string SectionName { get; set; }

        [Ignore]
        public List<Piece> PiecesList { get; set; }

        #endregion
        #region constructor

        public Section(string sectionName)
        {
            SectionName = sectionName;
            PiecesList = new List<Piece>();
        }
        public Section()
        {

        }
        #endregion

        #region debugging
        /// <summary>
        /// Display informations about a section.
        /// </summary>
        public void DispDataSection()
        {
            Console.WriteLine("Nom section: " + SectionName);
            Console.WriteLine("Liste des pièces: ");
            foreach (Piece piece in PiecesList)
            {
                piece.DispDataPiece();
            }
        }
        #endregion
    }

}







