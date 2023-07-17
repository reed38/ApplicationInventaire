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


        #region methodsInitializeData





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
}


























