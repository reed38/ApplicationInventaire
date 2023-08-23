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
    /// The user clicked on a given project
    /// </summary>
    public partial class PAGE_3_1 : Page
    {


        public PAGE_3_1()
        {
            InitializeComponent();
             InitializeUI();
            InitializeAuthor_Description();
            GlobalPages.page_3_1 = this;
        }

        #region variable
        TemplateData templateData = GlobalTemplateData.CurrentTemplateData;  //loading project info using the static class

        #endregion
      
        #region methodsUI
        
        /// <summary>
        /// The user clicked on the button edit, this function change the page to "PAGE_3_6_1"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonEditProjectClick(object sender, RoutedEventArgs e)
        {
            GlobalPages.SetCurrentPage(GlobalPages.PAGE_3_6_1);
        }


        /// <summary>
        /// The user clicked on the button Plan, this function change the page to "PAGE_3_3"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonPlanClick(object sender, RoutedEventArgs e)
        {
            GlobalPages.SetCurrentPage(GlobalPages.PAGE_3_3);
        }

        /// <summary>
        /// The user clicked on the button ContinuInventory ",   this function change the page to "PAGE_3_2" and ask the user to select an excel file to continu from.
        /// This file is then copied in memory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonContinueInventoryClick(object sender, RoutedEventArgs e)
        {

            string tmp = FileManager.OpenExcelFilePopup();
            if (!string.IsNullOrEmpty(tmp))
            {
                File.Copy(tmp, GlobalTemplateData.CurrentTemplateData.myTemplateInfos.TmpExcelPath, true);
                GlobalTemplateData.CurrentTemplateData.InitializePieceFromExcel(); //We re initialize the data using the excel file. This is useful if a new inventory is made without reloading the project
                GlobalPages.SetCurrentPage(GlobalPages.PAGE_3_2);
            }
         
                        }
        /// <summary>
        /// The user clicked on the button New Inventory ",   this function change the page to "PAGE_3_2" .
        /// This file is then copied in memory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonNewInventoryClick(object sender, RoutedEventArgs e)
        {

            GlobalTemplateData.CurrentTemplateData.ResetTmpExcel();
            GlobalPages.SetCurrentPage(GlobalPages.PAGE_3_2);
        }

        /// <summary>
        /// The user clicked on the button Search with Image, this function cha,ges the page to "PAGE_5_1"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonsearchImageClick(object sender, RoutedEventArgs e)
        {
            GlobalPages.SetCurrentPage(GlobalPages.PAGE_3_4);
        }

        /// <summary>
        /// The User clicked on the Button Search with Tag. this function changes the page to "PAGE_5_1"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonsearchTagClick(object sender, RoutedEventArgs e)
        {
            GlobalPages.SetCurrentPage(GlobalPages.PAGE_5_1);
        }

        /// <summary>
        /// the user clicked on the Button "Delete Template". This function Delete the Directory corresponding to the project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonDeleteProjectClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Directory.Delete(templateData.myTemplateInfos.ProjectPath, true);

            }
            catch (Exception ex)
            {
                POPUP.ShowPopup("Erreur ce template contient un ficher en cours d'utilisation. Veuillez fermet l'application et recommencer");
            }
            GlobalPages.PageGoBack();
        }

        #endregion

        #region GIMethods

        /// <summary>
        /// This funciton Initialize the Description and the name of the author on the PAGE_3_1
        /// </summary>
        private void InitializeAuthor_Description()
        {
            this.TextBlocklAuthorName.Text = templateData.myTemplateInfos.Author;
            this.TextBlocklLastEditorName.Text = templateData.myTemplateInfos.LastEditor;
            this.TextBlockDesciption.Text = templateData.myTemplateInfos.Description;
            DateTime EditionDate = this.templateData.myTemplateInfos.LastEditionDate;
            string EditionDateStr= EditionDate.Day.ToString() + "/" + EditionDate.Month.ToString() + "/" + EditionDate.Year.ToString() + " heure: " + EditionDate.Hour;

            DateTime CreationDate = this.templateData.myTemplateInfos.CreationDate;
            string CreationDateStr = CreationDate.Day.ToString() +"/"+ CreationDate.Month.ToString()+"/"+ CreationDate.Year.ToString() +" heure: "+CreationDate.Hour;

            this.TextBlockCreationDate.Text = CreationDateStr;
            this.TextBlocklLastEditonDate.Text = EditionDateStr;
        }





        #endregion

        #region privateMethods
        /// <summary>
        /// This method hide some elements in fonction of the user rights.
        /// </summary>
        private void InitializeUI()
        {
            if (GlobalTemplateData.UserRigth == 0)
            {
                this.ButtonEditProject.Visibility = Visibility.Collapsed;

            }
            else
            {
                this.ButtonEditProject.Visibility = Visibility.Visible;


            }
        }
        #endregion



    }
}
