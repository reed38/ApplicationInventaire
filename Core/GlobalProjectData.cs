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
    public class ImageInfos
    {
        #region variables
        public string Name { set; get; }
        public string Path { set; get; }
        #endregion
        #region constructor
        public ImageInfos(string name, string path)
        {
            this.Name = name;
            this.Path = path;
        }

        #endregion



        public void DispImageInfo()
        {
            Console.WriteLine("name: " + this.Name);
            Console.WriteLine("Path: " + this.Path);
        }
    }


    /// <summary>
    /// used to contain image path and image name
    /// </summary>


    public static class GlobalProjectData
    {
        #region variables
        public static string[] ProjectListName;
        public static ImageInfos[] CurrentImageSectionList;
        public static ImageInfos[] CurrentImageReleveList;
        public static string CurrentProjectName;
        public static ProjectData CurrentProjectData;

        #endregion
        #region ImagePathVariable
        public static string RedCirclePath = AppDomain.CurrentDomain.BaseDirectory + "Images/redCircle.png";
        #endregion

        #region PAGE_3_1Variables
        public static int IndiceSection;
        public static int IndicePiece;
        #endregion


        #region methodsInitializeData

        public static void InitializeGlobalProjectData()
        {
            ProjectInfos tmp = new ProjectInfos(GlobalProjectData.CurrentProjectName);

            GlobalProjectData.CurrentProjectData = new ProjectData(tmp);
            GlobalProjectData.IndicePiece = 0;
            GlobalProjectData.IndiceSection = 0;
            LoadCurrentProject();

        }
        /// <summary>
        /// used to get an array of ImageInfos object (containing file name ithout extension and file path) corresponding to a given oject
        /// </summary>
        /// <param name="project"> name of the project</param>
        /// <returns></returns>
        private static ImageInfos[] GetSectionsNames()
        {

            string ImageSectionPath = GlobalProjectData.CurrentProjectData.myProjectInfos.ImageSectionPath;
            string[] path = Directory.GetFiles(ImageSectionPath);
            ImageInfos[] result = new ImageInfos[path.Length];

            for (int i = 0; i < path.Length; i++)
            {
                result[i] = new ImageInfos(Path.GetFileNameWithoutExtension(path[i]), path[i]);

            }

            return (result);

        }
        private static ImageInfos[] GetRelevesNames()
        {

            string ImageRelevePath = AppDomain.CurrentDomain.BaseDirectory + "UserData/" + GlobalProjectData.CurrentProjectName + "/Image" + "/ImageReleve";
            string[] path = Directory.GetFiles(ImageRelevePath);
            ImageInfos[] result = new ImageInfos[path.Length];

            for (int i = 0; i < path.Length; i++)
            {
                result[i] = new ImageInfos(Path.GetFileNameWithoutExtension(path[i]), path[i]);
            }

            return (result);

        }

        /// <summary>
        /// Used to get an array of string containing the list of the name of the projects present in the UserData folder
        /// </summary>
        /// <returns> an array of string</returns>
        public static string[] GetProjectNames()
        {
            string UserDataPath = AppDomain.CurrentDomain.BaseDirectory + "UserData";
            string[] result = Directory.GetDirectories(UserDataPath);
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Path.GetFileNameWithoutExtension(result[i]);

            }

            return (result);



        }



        public static void LoadCurrentProject()
        {
            GlobalProjectData.CurrentImageSectionList = GetSectionsNames();
            GlobalProjectData.CurrentImageReleveList = GetRelevesNames();
             GlobalProjectData.CurrentProjectData.InitializePieceFromExcel();


        }





        #endregion

        #region debugging

        #endregion




    }

}

























