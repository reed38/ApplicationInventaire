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
            GlobalProjectData.InitializeGlobalProjectData();
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
                File.Copy(selectedFilePath, GlobalProjectData.CurrentProjectData.myProjectInfos.TmpExcelPath);
            }
           



        }


        

        private void ButtonNewInventoryClick(object sender, RoutedEventArgs e)
        {
            GlobalProjectData.InitializeGlobalProjectData();
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
                GlobalProjectData.IndicePiece = 0;
                GlobalProjectData.IndiceSection = 0;
                GlobalPages.page_3_2.CurrentPiece = GlobalProjectData.CurrentProjectData.mySections[GlobalProjectData.IndiceSection].PiecesList[GlobalProjectData.IndicePiece];
                GlobalPages.page_3_2.SetBorderPosition();



            }

        }
        #endregion
    }
}
