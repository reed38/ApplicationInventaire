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

namespace ApplicationInventaire.Core.GlobalProjectData
{



    /// <summary>
    /// used to contain image path and image name
    /// </summary>









    public static class GlobalProjectData
    {
        #region variables
        public static string CurrentProjectName;
        public static ProjectData CurrentProjectData;

        #endregion
        #region ImagePathVariable
        public static string RedCirclePath = AppDomain.CurrentDomain.BaseDirectory + "Images/redCircle.png";
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







        #endregion

        #region debugging

        #endregion




    }
}


























