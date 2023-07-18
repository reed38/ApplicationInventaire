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
    /// Logique d'interaction pour PAGE_1_1.xaml
    /// </summary>
    public partial class PAGE_1_1 : Page, INotifyCollectionChanged, INotifyPropertyChanged
    {
        public PAGE_1_1()
        {
            InitializeComponent();
            GlobalPages.page_1_1 = this;
            DataContext = this;
            ProjectNameList = new ObservableCollection<string>(GlobalProjectData.GetProjectNames());
            ProjectPathList = GlobalProjectData.GetProjecPaths();

            //loading templates present in memoty

        }
        #region variables
        private string [] ProjectPathList;
        #endregion

        #region BindingVariables
        private ObservableCollection<string> projectNameList = new ObservableCollection<string>();

        private string selectedValue;
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

        private void ListBoxSectionFileSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            selectedValue = (string)listBox.SelectedItem;


        }

        private void ButtonClickExportTemplate(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(selectedValue))
            {
                POPUP.ShowPopup("Please select a template to export");
                return;
            };
            string TemplateFolderPath = ProjectPathList[ProjectNameList.IndexOf(selectedValue)];
            string destPath = FileManager.OpenSelectZipSavePopup(selectedValue);
            ProjectInfos tmp = new(selectedValue);
            FileManager.CreateZipArchive(TemplateFolderPath, destPath);
            GlobalPages.SetCurrentPageBack();
        }
    }
}


