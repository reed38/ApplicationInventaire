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
            ImageSection = GlobalProjectData.CurrentImageSectionList[0].Path;
                

        }
       
        public bool isRunning = true;
        public List<Section> toto;

        #region bindingVariablesSources
        private string imageSection;
        private string imageReleve;
        #endregion

        #region bindingMethods
        public string ImageSection
        {
            get { return imageSection; }
            set
            {
                imageSection = value;
                OnPropertyChanged(nameof(ImageSection));
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
      
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        public event PropertyChangedEventHandler PropertyChanged;




        #endregion

        #region staticMethods
        private static void GotoNextPiece(string answ)
        {
            if (GlobalProjectData.IndiceSection < GlobalProjectData.CurrentProjectData.mySections.Count)
            {
                Section tmpSection = GlobalProjectData.RemainingSections[0];
                Piece tmpPiece = tmpSection.PiecesList[GlobalProjectData.IndicePiece];
                GlobalProjectData.CurrentProjectData.myExcelFile.SetCellValue(tmpPiece.SheetName, answ, tmpPiece.X, tmpPiece.Y);
                GlobalProjectData.IndicePiece++;
                if (tmpSection.PiecesList.Count <= GlobalProjectData.IndicePiece)
                {
                    GlobalProjectData.IndiceSection++;
                    GlobalProjectData.IndicePiece = 0;
                    foreach (ImageInfos i in GlobalProjectData.CurrentImageSectionList)
                    {
                        if (i.Name == tmpSection.SectionName)
                        {
                            GlobalPages.page_3_2.ImageSection = i.Path;

                        }
                    }


                }

            }

            else
            {


            }

        }

        #endregion


        #region methodsUI
        private void ButtonClickSaveAndQuit(object sender, RoutedEventArgs e)
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
                File.Copy(GlobalProjectData.CurrentProjectData.myProjectInfos.TmpPath + "/tmpExcel.xls", filePath,true);
            }

            GlobalPages.SetCurrentPageBack(GlobalPages.PAGE_3_1);
        }

        private void ButtonClickNo(object sender, RoutedEventArgs e)
        {
            GotoNextPiece("1");

        }

        private void ButtonClickYes(object sender, RoutedEventArgs e)
        {
            GotoNextPiece("2");


        }
        #endregion





    }






}

