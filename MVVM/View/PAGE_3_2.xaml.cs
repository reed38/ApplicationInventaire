using ApplicationInventaire.Core.GlobalPages;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

using ApplicationInventaire.Core.Config;
using ApplicationInventaire.Core.DatabaseManagement;
using ApplicationInventaire.Core.ExcelManagement;
using ApplicationInventaire.Core.PieceSections;
using ApplicationInventaire.MVVM.View;
using ApplicationInventaire.Core.GlobalProjectData;
using Microsoft.Win32;
using ApplicationInventaire.Core.ProjectDataSet;

namespace ApplicationInventaire.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour PAGE_3_2.xaml
    /// </summary>
    public partial class PAGE_3_2 : Page, INotifyPropertyChanged
    {

        public PAGE_3_2()
        {
            InitializeComponent();
            GlobalPages.page_3_2 = this;
            DataContext = this;
            ProjectInfos tmp = new ProjectInfos(GlobalProjectData.CurrentProjectName);
            projectData = new ProjectData(tmp);
            this.RedFramePath = GlobalProjectData.RedFramePath;
           
            if (GlobalProjectData.ExcelContinuPath != null)
            {
                File.Copy(GlobalProjectData.ExcelContinuPath, tmp.TmpExcelPath, true);

            }
            projectData.InitializePieceFromExcel();

            ResetTextBox();
            HideTextBoxSerialNumberConstructor();
            RedFrameImage.Visibility = Visibility.Visible;
            IndiceSection = 0;
            IndicePiece = 0;
            foreach (ImageInfos im in this.projectData.ImageSectionList)
            {
                if (im.Name == projectData.mySections[IndiceSection].SectionName)
                {
                    ImageSection = im.Path;
                    break;

                }
            }

            FindNextNoPresent();
            SetBorderPosition();

        }
        #region Variables
        private int IndiceSection { set; get; }
        private int IndicePiece { set; get; }
        private ProjectData projectData { set; get; }
        #endregion

        #region bindingVariablesSources
        private string imageSection;
        private string redFramePath;
        private string imageReleve;
        private double xCoordinatevar;
        private double yCoordinatevar;
        private Piece currentPiece;


        private Section currentSection;

        #endregion
     
        #region bindingMethods

        public Section CurrentSection
        {
            get { return currentSection; }
            set
            {
                currentSection = value;
                OnPropertyChanged(nameof(CurrentSection));
            }
        }

        public Piece CurrentPiece
        {
            get { return currentPiece; }
            set
            {
                currentPiece = value;
                OnPropertyChanged(nameof(CurrentPiece));
            }
        }

        public string ImageReleve
        {
            get { return imageReleve; }
            set
            {
                imageReleve = value;
                OnPropertyChanged(nameof(ImageReleve));
            }
        }

        public string RedFramePath
        {
            get { return redFramePath; }
            set
            {
                redFramePath = value;
                OnPropertyChanged(nameof(RedFramePath));
            }
        }
        public double XCoordinate
        {
            get { return xCoordinatevar; }
            set
            {
                xCoordinatevar = value;
                OnPropertyChanged(nameof(XCoordinate));
            }
        }
        public double YCoordinate
        {
            get { return yCoordinatevar; }
            set
            {
                yCoordinatevar = value;
                OnPropertyChanged(nameof(YCoordinate));
            }
        }

        public string ImageSection
        {
            get { return imageSection; }
            set
            {
                imageSection = value;
                OnPropertyChanged(nameof(ImageSection));
            }
        }



        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region PrivateMethods

        private void UpdateCurrent()
        {
            this.CurrentSection = projectData.mySections[IndiceSection];
            this.CurrentPiece = projectData.mySections[IndiceSection].PiecesList[IndicePiece];

        }

        private void GotoNextPiece(string answ)
        {
            ManageReleveImage();
            projectData.myTmpExcelFile.SetCellValue(currentPiece.SheetName, answ, currentPiece.ExcelColumn, currentPiece.ExcelRow);
            FindNextNoPresent();
            SetBorderPosition();

        }

        private void FindNextNoPresent()
        {
            ImageReleve = null;

            if (IndiceSection == projectData.mySections.Count - 1 && IndicePiece == projectData.mySections[IndiceSection].PiecesList.Count)
            {
                SaveAndQuit();
                return;

            }

            else if (IndicePiece == projectData.mySections[IndiceSection].PiecesList.Count)
            {
                IndiceSection++;
                IndicePiece = 0;
                foreach (ImageInfos im in this.projectData.ImageSectionList)
                {
                    if (im.Name == projectData.mySections[IndiceSection].SectionName)
                    {
                        ImageSection = im.Path;
                        break;

                    }

                }

            }


            for (int section = IndiceSection; section < projectData.mySections.Count; section++)
            {

                for (int piece = IndicePiece; piece < projectData.mySections[section].PiecesList.Count; piece++)
                {

                    if (projectData.mySections[section].PiecesList[piece].IsPresent == 0)
                    {
                        this.IndicePiece = piece;
                        this.IndiceSection = section;
                        UpdateCurrent();
                        if (this.CurrentPiece.IsReleveRequired == 1)
                        {
                            ShowTextBoxSerialNumberConstructor();
                            foreach (ImageInfos i in projectData.ImageReleveList)
                            {
                                if (i.Name.IndexOf(this.CurrentPiece.PieceName)>=0)
                                {
                                    ImageReleve = i.Path;
                                    break;
                                }

                            }
                        }
                        IndicePiece++;
                        return;
                    }



                }


            }
        }
       
        private void ManageReleveImage()
        {
            if (this.CurrentPiece.IsReleveRequired == 1)
            {
                foreach (ImageInfos i in projectData.ImageReleveList)
                {
                    if (i.Name.Equals(this.CurrentPiece.PieceName))
                    {
                        imageReleve = i.Path;
                        break;
                    }
                }
            }

        }

        private void SetBorderPosition()
        {
            ChangeFrameCoordinates(CurrentPiece.X - this.RedFrameImage.Height / 2, CurrentPiece.Y - this.RedFrameImage.Width / 2);
            ChangeLabelcoordinates(CurrentPiece.X, CurrentPiece.Y);


        }
       
        private void ChangeFrameCoordinates(double x, double y)
        {
            Canvas.SetLeft(this.RedFrameImage, x);
            Canvas.SetTop(this.RedFrameImage, y);
        }

        private void ChangeLabelcoordinates(double x, double y)
        {
            Canvas.SetLeft(this.LabelNameTag, x - 60);
            Canvas.SetTop(this.LabelNameTag, y - 60);

        }


        private void SaveAndQuit()
        {



            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // Set the default file name and extension
            saveFileDialog.DefaultExt = ".xls";

            // Set the file filters
            saveFileDialog.Filter = "excel file (*.xls )|*.xls|excel file (*.xlsx)|*.xlsx";  // Set allowed file extensions

            // Show the save file dialog
            bool? result = saveFileDialog.ShowDialog();

            // Process the selected file
            if (result == true)
            {
                string filePath = saveFileDialog.FileName;
                File.Copy(projectData.myProjectInfos.TmpExcelPath, filePath, true);

            }

            GlobalPages.SetCurrentPageBack(GlobalPages.PAGE_3_1);
        }

        private void UpdateComment()
        {
            if (!TextBoxComment.Text.Equals(string.Empty))
            {
                CellInfo tmp = projectData.myTmpExcelFile.FindValue("Commentaire");
                projectData.myTmpExcelFile.SetCellValue(tmp.Sheet, TextBoxComment.Text, tmp.Column, CurrentPiece.ExcelRow);
                TextBoxComment.Clear();
            }



        }

        private void UpdateSerialNumber()
        {
            if (!TextBoxSerialNumber.Text.Equals(string.Empty))
            {
                CellInfo tmp = projectData.myTmpExcelFile.FindValue("N° SERIE");
                projectData.myTmpExcelFile.SetCellValue(tmp.Sheet, TextBoxSerialNumber.Text, tmp.Column, CurrentPiece.ExcelRow);
                TextBoxSerialNumber.Clear();
                TextBoxSerialNumber.Visibility = Visibility.Hidden;
            }


        }

        private void UpdateConstructor()
        {
            if (!TextBoxConstructor.Text.Equals(string.Empty))
            {
                CellInfo tmp = projectData.myTmpExcelFile.FindValue("FABRICANT");
                projectData.myTmpExcelFile.SetCellValue(tmp.Sheet, TextBoxConstructor.Text, tmp.Column, CurrentPiece.ExcelRow);
                TextBoxConstructor.Clear();
            }


        }

        private void ResetTextBox()
        {
            TextBoxComment.Clear();
            TextBoxConstructor.Clear();
            TextBoxSerialNumber.Clear();

        }

        private void HideTextBoxSerialNumberConstructor()
        {
            TextBoxSerialNumber.Visibility = Visibility.Hidden;
            TextBoxConstructor.Visibility = Visibility.Hidden;
            labelConstructor.Visibility = Visibility.Hidden;
            labelSerialNumber.Visibility = Visibility.Hidden;
        }
        
        private void ShowTextBoxSerialNumberConstructor()
        {
            TextBoxSerialNumber.Visibility = Visibility.Visible;
            labelConstructor.Visibility = Visibility.Visible;
            labelSerialNumber.Visibility = Visibility.Visible;
            TextBoxConstructor.Visibility = Visibility.Visible;
        }


        #endregion

        #region UIMethods

        private void ButtonClickSaveAndQuit(object sender, RoutedEventArgs e)
        {
            UpdateSerialNumber();
            UpdateConstructor();
            UpdateComment();
            SaveAndQuit();
        }

        private void ButtonClickNo(object sender, RoutedEventArgs e)
        {
            ResetTextBox();
            HideTextBoxSerialNumberConstructor();
            GotoNextPiece("0");

        }

        private void ButtonClickYes(object sender, RoutedEventArgs e)
        {
            if (CurrentPiece.IsReleveRequired == 1 && ((TextBoxSerialNumber.Text.Equals(string.Empty)) || TextBoxConstructor.Text.Equals(string.Empty)))
            {
                PopupNoSerialNumberConstructor.IsOpen = true;
                return;

            }
            UpdateComment();
            UpdateSerialNumber();
            UpdateConstructor();
            ResetTextBox();
            HideTextBoxSerialNumberConstructor();
            GotoNextPiece("1");


        }

        private void Grid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PopupNoSerialNumberConstructor.IsOpen = false;

        }

     #endregion


    }
}

