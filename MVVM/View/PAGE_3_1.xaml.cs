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
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"C:\";  // Set the initial directory if desired
            openFileDialog.Filter = "excel file (*.xls )|*.xls|excel file (*.xlsx)|*.xlsx";  // Set allowed file extensions

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                GlobalProjectData.CurrentProjectData.myProjectInfos.ExcelPath = selectedFilePath;
                
                File.Copy(selectedFilePath, GlobalProjectData.CurrentProjectData.myProjectInfos.TmpPath+"/tmpExcel.xls",true);
               
                GlobalProjectData.CurrentProjectData.myExcelFile = new ExcelFile(selectedFilePath);

                InitializePAGE_3_2_Section();
                GlobalPages.SetCurrentPage(GlobalPages.PAGE_3_2);


                // Use the selected file path as needed
            }


        }

        private void ButtonNewInventoryClick(object sender, RoutedEventArgs e)
        {

            File.Copy(GlobalProjectData.CurrentProjectData.myProjectInfos.ExcelPath, GlobalProjectData.CurrentProjectData.myProjectInfos.TmpPath + "/tmpExcel.xls", true);
            GlobalProjectData.CurrentProjectData.myExcelFile = new ExcelFile(GlobalProjectData.CurrentProjectData.myProjectInfos.TmpPath);

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
            GlobalProjectData.InitializeGlobalProjectData();

            if (GlobalPages.page_3_2!=null)
                GlobalPages.page_3_2.ImageSection = GlobalProjectData.CurrentImageSectionList[0].Path ;

        }
        #endregion
    }
}
