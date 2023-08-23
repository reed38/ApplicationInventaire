using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
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
using ApplicationInventaire.Core.GlobalPages;
using ApplicationInventaire.Core.GlobalProjectData;
using ApplicationInventaire.Core.ProjectDataSet;

namespace ApplicationInventaire.MVVM.View
{
    /// <summary>
    /// The user clicked on export
    /// </summary>
    public partial class PAGE_1_1 : Page, INotifyCollectionChanged, INotifyPropertyChanged
    {
        public PAGE_1_1()
        {
            InitializeComponent();
            GlobalPages.page_1_1 = this;
            DataContext = this;
            ProjectNameList = new ObservableCollection<string>(GlobalData.GetProjectNames()); //we get the list of the project available in the UserData folder
            ProjectPathList = GlobalData.GetTemplatePaths(); //going through the UserData folder and initializing ProjectPathList


        }
        #region Privatevariables
        private string [] ProjectPathList;
        #endregion

        #region BindingVariables
        private ObservableCollection<string> projectNameList = new ObservableCollection<string>();  //collection used to in  the listBox
        private string selectedValue; //will be automatically updated to contain the string of the case the user is currently selecting
        #endregion

        #region BindingMethods

        public ObservableCollection<string> ProjectNameList
        {
            get { return projectNameList; }
            set
            {
                projectNameList = value;
                OnPropertyChanged(nameof(ProjectNameList));
            }
        }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }



        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region privateMethods

        private void ListBoxSectionFileSelectionChanged(object sender, SelectionChangedEventArgs e) //used to kow when the user change selection
        {
            ListBox listBox = (ListBox)sender;
            selectedValue = (string)listBox.SelectedItem; //update the value of the currently selected item
        }

        #endregion

        #region UIMethods

        private void ButtonClickExportTemplate(object sender, RoutedEventArgs e) //the user click on the export button 
        {
            if(string.IsNullOrEmpty(selectedValue)) //nothing is selected => problem
            {
                POPUP.ShowPopup("Veuillez sélectionner un template à exporter");
                return;
            };
            string TemplateFolderPath = ProjectPathList[ProjectNameList.IndexOf(selectedValue)];
            string destPath = FileManager.OpenSelectZipSavePopup(selectedValue);
            FileManager.CreateZipArchive(TemplateFolderPath, destPath); //a zip archive is created at the selected location
            GlobalPages.PageGoBack(); //going back to main menu
        }

        #endregion
    }
}


