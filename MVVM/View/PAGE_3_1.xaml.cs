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
using System.Windows.Forms;

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
            
          

            Microsoft.Win32.OpenFileDialog openFileDialog  = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.InitialDirectory = @"C:\";  // Set the initial directory if desired
            openFileDialog.DefaultExt = ".xls";

            // Set the file filters
            openFileDialog.Filter = "excel file (*.xls )|*.xls|excel file (*.xlsx)|*.xlsx";  // Set allowed file extensions

            bool? result = openFileDialog.ShowDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                
                 GlobalProjectData.ExcelContinuPath = selectedFilePath;

                
            }

            GlobalPages.SetCurrentPage(GlobalPages.PAGE_3_2);






        }


        

        private void ButtonNewInventoryClick(object sender, RoutedEventArgs e)
        {


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
            GlobalPages.SetCurrentPage(GlobalPages.PAGE_5_1);
        }

        #endregion
        

        private void ButtonExportClick(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string dest = dialog.SelectedPath;
                    string source =new ProjectInfos(GlobalProjectData.CurrentProjectName).ProjectPath;

                    Directory.CreateDirectory(dest);
                    CopyDirectory(source, dest);


                }
            }
        }
      

        #region publicmethods
        public void CopyDirectory(string sourceDirectory, string destinationDirectory)
        {
            // Create the destination directory if it doesn't exist
            Directory.CreateDirectory(destinationDirectory);

            // Get the files in the source directory
            string[] files = Directory.GetFiles(sourceDirectory);

            // Copy each file to the destination directory
            foreach (string file in files)
            {
                string fileName = System.IO.Path.GetFileName(file);
                string destinationPath = System.IO.Path.Combine(destinationDirectory, fileName);
                File.Copy(file, destinationPath, true);
            }

            // Recursively copy subdirectories
            string[] directories = Directory.GetDirectories(sourceDirectory);
            foreach (string directory in directories)
            {
                string directoryName = System.IO.Path.GetFileName(directory);
                string destinationPath = System.IO.Path.Combine(destinationDirectory, directoryName);
                CopyDirectory(directory, destinationPath);
            }
        }
        #endregion
    }
}
