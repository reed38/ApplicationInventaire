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
using System.Diagnostics;

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
            InitializeAuthor_Description();
            GlobalPages.page_3_1 = this;
        }

        #region variable
        ProjectData projectData = GlobalProjectData.CurrentProjectData;

        #endregion
        #region methodsUI

        private void ButtonEditProjectClick(object sender, RoutedEventArgs e)
        {
            GlobalPages.SetCurrentPage(GlobalPages.PAGE_3_6_1);
        }

        private void ButtonPlanClick(object sender, RoutedEventArgs e)
        {
            GlobalPages.SetCurrentPage(GlobalPages.PAGE_3_3);
        }
       
        private void ButtonContinueInventoryClick(object sender, RoutedEventArgs e)
        {

            string tmp = FileManager.OpenExcelFilePopup();
            if (!string.IsNullOrEmpty(tmp))
            {
                File.Copy(tmp, GlobalProjectData.CurrentProjectData.myProjectInfos.TmpExcelPath, true); ;
                GlobalPages.SetCurrentPage(GlobalPages.PAGE_3_2);
            }
        }

        private void ButtonNewInventoryClick(object sender, RoutedEventArgs e)
        {

            GlobalProjectData.CurrentProjectData.ResetTmpExcel();

            GlobalPages.SetCurrentPage(GlobalPages.PAGE_3_2);
        }

        private void ButtonsearchImageClick(object sender, RoutedEventArgs e)
        {
            GlobalPages.SetCurrentPage(GlobalPages.PAGE_3_4);
        }
                
        private void ButtonsearchTagClick(object sender, RoutedEventArgs e)
        {
            GlobalPages.SetCurrentPage(GlobalPages.PAGE_5_1);
        }

        private void ButtonDeleteProjectClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Directory.Delete(projectData.myProjectInfos.ProjectPath, true);

            }
            catch (Exception ex)
            {
                POPUP.ShowPopup("error this project contains a file  being used");
            }
            GlobalPages.PageGoBack();
        }

        #endregion

        #region GIMethods

        private void InitializeAuthor_Description()
        {
            this.TextBlocklAuthorName.Text = projectData.myProjectInfos.Author;
            this.TextBlockDesciption.Text = projectData.myProjectInfos.Description;
        }





        #endregion

       
    }
}
