using ApplicationInventaire.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using ApplicationInventaire.Core.Config;
using ApplicationInventaire.Core.DatabaseManagement;
using ApplicationInventaire.Core.ExcelManagement;
using ApplicationInventaire.Core.GlobalPages;
using ApplicationInventaire.Core.PieceSections;
using ApplicationInventaire.Core.ProjectDataSet;
using ApplicationInventaire.MVVM.View;
using System.Runtime.CompilerServices;
using System.IO;
using System.Security.Policy;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using NPOI.OpenXmlFormats.Dml;
using Microsoft.Win32;
using System.IO.Compression;
using System.Windows.Forms;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using MessageBox = System.Windows.MessageBox;

namespace ApplicationInventaire.Core.GlobalProjectData
{



    /// <summary>
    /// used to contain image path and image name
    /// </summary>









    public static class GlobalProjectData
    {
        
        #region variables
        public static string CurrentProjectName;
        public static string CurrentSectionName;
        public static string CurrentPieceName;
        public static string AppRootDirectoryPath=GlobalProjectData.InitializeRootDirectory();
        public static ProjectData CurrentProjectData;

        #endregion
       
        
        #region ImagePathVariable
        public static string RedFramePath = Path.Combine(AppRootDirectoryPath, "Image\\RedFrame.png");
        public static string RedCirclePath = Path.Combine(AppRootDirectoryPath, "Image\\RedCircle.png");

        #endregion

        #region BindingMethods

        #endregion
        
        #region PAGE_3_1Variables
        public static string ExcelContinuPath;

        #endregion







        /// <summary>
        /// Used to get an array of string containing the list of the name of the projects present in the UserData folder
        /// </summary>
        /// <returns> an array of string</returns>
        public static string[] GetProjectNames()
        {
            string UserDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory , "UserData");
            string[] result = Directory.GetDirectories(UserDataPath);
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Path.GetFileNameWithoutExtension(result[i]);

            }

            return (result);



        }

        public static string[] GetProjecPaths()
        {
            string UserDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserData");
            string[] result = Directory.GetDirectories(UserDataPath);
            return result;

        }

        public static string[] GetPlansNames()
        {
            ProjectInfos tmp = new(CurrentProjectName);
            string PlansPath = tmp.PlansPath;
            string[] result = Directory.GetFiles(PlansPath);
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Path.GetFileNameWithoutExtension(result[i]);

            }

            return (result);

        }

        public static string[] GetPlansPath()
        {
            ProjectInfos tmp = new(CurrentProjectName);
            string PlansPath = tmp.PlansPath;
            string[] result = Directory.GetFiles(PlansPath);
          
            return (result);

        }


        #region methodsInitializeData
        private static  string InitializeRootDirectory()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectDirectory = Directory.GetParent(baseDirectory).Parent.FullName;
            for (int i = 0; i < 2; i++)
            {
                baseDirectory = Directory.GetParent(baseDirectory).Parent.FullName;
            }
           return baseDirectory;

        }

        private static BitmapImage GetImageSource(Uri imagePath)
        {
            BitmapImage imageSource = new BitmapImage();
            imageSource.BeginInit();
            imageSource.UriSource = imagePath;
            imageSource.EndInit();
            return imageSource;
        }







        #endregion

        #region debugging

        #endregion




    }

    public static class FileManager
    {

        public static string[] OpenImagePopup()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp";

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string[] selectedFiles = openFileDialog.FileNames;
                return selectedFiles;

                // Process the selected image files
                
            }
            return null;
        }

        public static string OpenImagePopupSingle()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp";

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string selectedFiles = openFileDialog.FileName;
                return selectedFiles;

                // Process the selected image files

            }
            return null;
        }

        public static string OpenSelectZipLoadPopup()
        {
           OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter =  "ZIP Files (*.zip)|*.zip|All Files (*.*)|*.*";


            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string selectedFiles = openFileDialog.FileName;
                return selectedFiles;

                // Process the selected image files

            }
            return null;
        }
    


        public static string OpenExcelFilePopup()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files (*.xlsx;*.xls)|*.xlsx;*.xls";
            openFileDialog.Multiselect = false;

            bool? dialogResult = openFileDialog.ShowDialog();

            if (dialogResult == true)
            {
                string filePath = openFileDialog.FileName;

                return filePath;
            }
            return null;
        }

        public static string OpenSelectZipSavePopup(string defaultFileName)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "ZIP Files (*.zip)|*.zip|All Files (*.*)|*.*";
            saveFileDialog.FileName = defaultFileName; // Set the default filename here

            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                string selectedFilePath = saveFileDialog.FileName;
                string directoryPath = Path.GetDirectoryName(selectedFilePath);

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                return selectedFilePath;
            }

            return null;
        }

        public static void  CreateZipArchive(string folderPath, string zipFilePath)
        {
            if(!string.IsNullOrEmpty(zipFilePath))
            {
                ZipFile.CreateFromDirectory(folderPath, zipFilePath);

            }
        }


    }

    public static class POPUP
    {

        public static  void ShowPopup(string message)
        {
            MessageBox.Show(message, "Field Missing", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}


























