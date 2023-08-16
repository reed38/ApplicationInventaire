using ApplicationInventaire.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ApplicationInventaire.Core.SettingsManagement;
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
using ComctlLib;

/// <summary>
/// This file is used to store global variables 
/// </summary>
namespace ApplicationInventaire.Core.GlobalProjectData
{



 /// <summary>
 /// This static class is used to pass Data through pages and store constant variables such as image path.
 /// </summary>
    public static class GlobalProjectData
    {
        
        #region variables
        public static string CurrentProjectName;
        public static string CurrentSectionName;
        public static string CurrentPieceName;
        public static string AppRootDirectoryPath=GlobalProjectData.InitializeRootDirectory();
        public static ProjectData CurrentProjectData;
        public static int UserRigth; //1 admin, 0 default
        public static string password = "userir";
        public static Settings toto;
        

        #endregion


        #region ImagePathVariable
        /// <summary>
        /// Contain the path to image asset used in application
        /// </summary>
        public static string RedFramePath = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources"), "Image\\RedFrame.png");
        public static string RedCirclePath = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources"), "Image\\RedCircle.png");
        public static string AventechLogoPath = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources"), "Image\\Logo_AVENTECH_Couleurs_by_Reyes.png");


        #endregion

        #region BindingMethods

        #endregion

        #region PAGE_3_1Variables
        //is used to store the excel Path if the user choose to continu the inventory. This can now be implemented diferently by directly changing the value in CurrentProjectData

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

        /// <summary>
        /// Go through the directory User Data to return the paths of the directories of the differents templates
        /// </summary>
        /// <returns> string [] containing the paths</returns>
        public static string[] GetProjecPaths()
        {
            string UserDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserData");
            string[] result = Directory.GetDirectories(UserDataPath);
            return result;

        }
        /// <summary>
        /// Go through the directory User Data to returs the names of the directories of the differents templates
        /// </summary>
        /// <returns> string [] containing the names</returns>
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
        /// <summary>
        /// Go through the directory Plans to returs the paths of the plans presents in the given template
        /// </summary>
        /// <returns> string [] containing the names</returns>
        public static string[] GetPlansPath()
        {
            ProjectInfos tmp = new(CurrentProjectName);
            string PlansPath = tmp.PlansPath;
            string[] result = Directory.GetFiles(PlansPath);
          
            return (result);

        }


        #region methodsInitializeData
/// <summary>
/// Get the root directory in which the app is currently running
/// </summary>
/// <returns> Path of that directory </returns>
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

       







        #endregion

        #region debugging

        #endregion




    }
    /// <summary>
    /// This static class contains function to invoke popup window for the user to interact with files (load, save..).
    /// </summary>
    public static class FileManager
    {

        /// <summary>
        /// Open a pop up menu to select image file, and return the paths to these images.
        /// </summary>
        /// <returns> string [] containing these paths</returns>
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

        /// <summary>
        /// Open a popup menu to select a single windows, and return the path the the file of that image
        /// </summary>
        /// <returns>string correspondind to the path of the image</returns>
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
        /// <summary>
        /// Open a pop up menu to select a single zip file and return the path to that zip file.
        /// </summary>
        /// <returns> string corresponding to the path of the given zip file</returns>
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

        /// <summary>
        /// Open a pop up menu to select multiple pdf files and return the paths to these files
        /// </summary>
        /// <returns> string[] containing the paths to the pdf files</returns>
        public static string[] OpenPdfPopup()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*";

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string[] selectedFiles = openFileDialog.FileNames;
                return selectedFiles;

                // Process the selected image files

            }
            return null;
        }

        /// <summary>
        /// Open a pop up menu to select a single Excel file, and return the path of that file
        /// </summary>
        /// <returns>string corresponding to the ppath of the excel file</returns>
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





        /// <summary>
        /// Open a pop up menu to select under which name and where to create a zip archive.
        /// </summary>
        /// <param name="defaultFileName"> default name of the zip archive</param>
        /// <returns>string corresponding to the path of the zip file</returns>
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

        /// <summary>
        /// Create a zip archive from the given folder to the given path
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="zipFilePath"></param>
        public static void  CreateZipArchive(string folderPath, string zipFilePath)
        {
            if(!string.IsNullOrEmpty(zipFilePath))
            {
                ZipFile.CreateFromDirectory(folderPath, zipFilePath);

            }
        }



    }
    /// <summary>
    /// This static class is used to store functions Showing a pop up in case of error.
    /// </summary>
    public static class POPUP
    {
        /// <summary>
        /// display an error window containing the given string
        /// </summary>
        /// <param name="message"></param>
        public static  void ShowPopup(string message)
        {
            MessageBox.Show(message, "Problem", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}


























