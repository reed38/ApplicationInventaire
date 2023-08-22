/// <summary>
/// ce fichier contient l'ensembe des classes et méthodes utilisés
/// </summary>
using System.Linq.Expressions;
using EnumsNET;
using NPOI.OpenXmlFormats.Dml.Diagram;
using SQLite;
using System.Collections.Generic;

using System;

using ApplicationInventaire.Core.DatabaseManagement;
using ApplicationInventaire.Core.ExcelManagement;
using ApplicationInventaire.Core.GlobalPages;
using ApplicationInventaire.Core.PieceSections;
using ApplicationInventaire.Core.ProjectDataSet;

/// <summary>
/// This file contains Piece and Section classes.
/// </summary>
namespace ApplicationInventaire.Core.PieceSections
{
    /// <summary>
    /// Class used to contain the infos about a given piece.
    /// </summary>
    public class Piece
    {
        #region set_get
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public double X { get; set; }
        public double Y { get; set; }
        public int ExcelColumn { get; set; }
        public int ExcelRow { get; set; }
        public string PieceName { get; set; }
        public string SheetName { get; set; }
        public int IsPresent { get; set; } //if the piece is present in the montage. 1 for yes 0 for no.
        public int IsReleveRequired { get; set; } //if a serial number and constructeur name muste be taken on the piece. 0 if no 1 if yes.
        public int HasMarking { get;set; } //if there is a marking on the piece. Some Pieces have a marking. For a piece named "toto", the corresponding Excel file line corresponding to its marking will be labelled "toto.M"
        public string Description { get;set; }

        public int SectionId { get; set; } // Foreign key to Section, used in the database to know in which Section the Piece is stored

        #endregion

        #region constructor
        public Piece(int x, int y, int excelColumn, int excelRow, string pieceName, int isPresent, int isReleveRequired, int SectionId)
        {
            X = x;
            Y = y;
            ExcelColumn = excelColumn;
            ExcelRow = excelRow;
            PieceName = pieceName;
            IsPresent = isPresent;
            IsReleveRequired = isReleveRequired;
            this.SectionId = SectionId;
        }

        public Piece( int excelColumn, int excelRow,string PieceName, string Description, int IsPresent, int isReleveRequired)
        {
            this.ExcelRow = excelRow;
            this.ExcelColumn = excelColumn;
            this.PieceName = PieceName;
            this.IsPresent = IsPresent;
            this.Description = Description;
            this.IsReleveRequired = isReleveRequired;

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
            Console.WriteLine("Marquage: " + HasMarking);
            Console.WriteLine("excel column: " + ExcelColumn);
            Console.WriteLine("excel row: " + ExcelRow);
            Console.WriteLine("Present: " + IsPresent);
            Console.WriteLine("Releve: " + IsReleveRequired);
            Console.WriteLine();
        }
        #endregion
    }
    /// <summary>
    /// Classed used To contains the infos about a section. A Section is a set of pieces
    /// </summary>
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







