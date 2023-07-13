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
            RedCircleImage.Visibility = Visibility.Visible;
            ProjectInfos tmp = new ProjectInfos(GlobalProjectData.CurrentProjectName);
            if (GlobalProjectData.ExcelContinuPath != null)
            {
                
                tmp.TmpExcelPath = GlobalProjectData.ExcelContinuPath;
                File.Copy(GlobalProjectData.ExcelContinuPath, tmp.TmpExcelPath,true);

            }
            projectData = new ProjectData(tmp);
            projectData.GetSectionsNames();
            projectData.GetRelevesNames();
            ImageSection = projectData.ImageSectionList[0].Path;
            IndicePiece = 0;
            IndiceSection = 0;
            UpdateCurrent();

            GlobalPages.page_3_2.SetBorderPosition();




        }
        #region Variables
        public int IndiceSection { set; get; }
        public int IndicePiece { set; get; }
        public ProjectData projectData { set; get; }
        #endregion

        #region bindingVariablesSources
        private string imageSection;
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

        #region methods

        private void UpdateCurrent()
        {
            this.CurrentSection = projectData.mySections[IndiceSection];
            this.CurrentPiece = projectData.mySections[IndiceSection].PiecesList[IndicePiece];

        }


        private void GotoNextPiece(string answ)
        {
            UpdateCurrent(); //for the first time
            IndicePiece++;

            if (IndicePiece < CurrentSection.PiecesList.Count)
            {
                projectData.myTmpExcelFile.SetCellValue(CurrentPiece.SheetName, answ, CurrentPiece.ExcelColumn, CurrentPiece.ExcelRow);

                UpdateCurrent();
                if (CurrentPiece.IsPresent == 1) //we don't do piece already present
                {
                    return;
                }
                SetBorderPosition();


            }


            if (IndicePiece == CurrentSection.PiecesList.Count) //end of section reached
            {

                IndiceSection++;
                IndicePiece = 0;
                UpdateCurrent();
                SetBorderPosition();


                foreach (ImageInfos i in this.projectData.ImageSectionList)
                {
                    if (i.Name == CurrentSection.SectionName)
                    {
                        ImageSection = i.Path;
                        break;

                    }
                }

            }
            if (IndiceSection == projectData.mySections.Count - 1)
            {
                SaveAndQuit();


            }



        }
        public void SetBorderPosition()
        {
            Thickness thicknessRedFrame = new Thickness();
            thicknessRedFrame.Top = CurrentPiece.Y - 17;
            thicknessRedFrame.Bottom = CurrentPiece.Y - 17;
            thicknessRedFrame.Left = CurrentPiece.X - 17;
            thicknessRedFrame.Right = CurrentPiece.X;
            this.RedCircleImage.Margin = thicknessRedFrame;

            Thickness thicknessPieceName = new Thickness();
            thicknessPieceName.Top = CurrentPiece.Y + 5;
            thicknessPieceName.Bottom = CurrentPiece.Y - 17;
            thicknessPieceName.Left = CurrentPiece.X - 30;
            thicknessPieceName.Right = CurrentPiece.X;
            this.LabelNameTag.Margin = thicknessPieceName;






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




        #endregion


        #region methodsUI
        private void ButtonClickSaveAndQuit(object sender, RoutedEventArgs e)
        {
            SaveAndQuit();
        }


        private void ButtonClickNo(object sender, RoutedEventArgs e)
        {
            GotoNextPiece("0");

        }

        private void ButtonClickYes(object sender, RoutedEventArgs e)
        {
            GotoNextPiece("1");


        }






    }

    #endregion




}

