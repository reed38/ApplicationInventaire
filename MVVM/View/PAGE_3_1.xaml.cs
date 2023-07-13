using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

using ApplicationInventaire.Core.GlobalPages;
using ApplicationInventaire.Core.GlobalProjectData;
using ApplicationInventaire.Core.ExcelManagement;
using ApplicationInventaire.Core.PieceSections;
using ApplicationInventaire.Core.ProjectDataSet;

namespace ApplicationInventaire.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour PAGE_3_1.xaml
    /// </summary>
    public partial class PAGE_3_1 : Page
    {






        public PAGE_3_1()
        {
            InitializeComponent();
            GlobalPages.page_3_1 = this;
        }

        #region methodsUI

        private void ButtonPlanClick(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonContinueInventoryClick(object sender, RoutedEventArgs e)
        {
            GlobalPages.page_3_2. projectData = new ProjectData(new ProjectInfos(GlobalProjectData.CurrentProjectName));
            InitializePAGE_3_2_Section();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"C:\";  // Set the initial directory if desired
            openFileDialog.DefaultExt = ".xls";

            // Set the file filters
            openFileDialog.Filter = "excel file (*.xls )|*.xls|excel file (*.xlsx)|*.xlsx";  // Set allowed file extensions

            bool? result = openFileDialog.ShowDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                if(GlobalPages.page_3_2 != null)
                {
                    File.Copy(selectedFilePath, GlobalPages.page_3_2.projectData.myProjectInfos.TmpExcelPath);


                }
                else
                {
                    GlobalProjectData.ExcelContinuPath = selectedFilePath;

                }
            }


           



        }


        

        private void ButtonNewInventoryClick(object sender, RoutedEventArgs e)
        {
            GlobalProjectData.CurrentProjectData.ResetPiecePresent(); //to restart the project from nothing
            InitializePAGE_3_2_Section();

            GlobalPages.SetCurrentPage(GlobalPages.PAGE_3_2);
        }



        private void ButtonsearchImageClick(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonModifyClick(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonsearchTagClick(object sender, RoutedEventArgs e)
        {

        }

        #endregion
        #region methodsStatic

        /// <summary>
        /// The page is only Loaded once, so its constructor is only called once. This funciton is used to reload section images each time a new inventory is started
        /// </summary>
        private static void InitializePAGE_3_2_Section()
        {

            //Used in case the user make two inventory without exiting the app
            if (GlobalPages.page_3_2 != null)
            {
                GlobalPages.page_3_2.projectData = new ProjectData(new ProjectInfos(GlobalProjectData.CurrentProjectName));
                GlobalPages.page_3_2.ImageSection = GlobalPages.page_3_2.projectData.ImageSectionList[0].Path;
                GlobalPages.page_3_2.IndicePiece = 0;
                GlobalPages.page_3_2.IndiceSection = 0;
                
                GlobalPages.page_3_2.CurrentPiece = GlobalPages.page_3_2.projectData.mySections[GlobalPages.page_3_2.IndiceSection].PiecesList[GlobalPages.page_3_2.IndicePiece];
                GlobalPages.page_3_2.SetBorderPosition();



            }

        }
        #endregion
    }
}
