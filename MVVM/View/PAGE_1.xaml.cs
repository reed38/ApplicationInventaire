using ApplicationInventaire.Core.GlobalPages;
using ApplicationInventaire.Core.GlobalProjectData;
using ApplicationInventaire.Core.PieceSections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using Microsoft.Win32;

using ApplicationInventaire.Core.ProjectDataSet;
using System.IO;
using Button = System.Windows.Controls.Button;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using ApplicationInventaire.Core.PieceSections;
using Section = ApplicationInventaire.Core.PieceSections.Section;
using System.IO.Compression;


namespace ApplicationInventaire.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour PAGE_1.xaml
    /// </summary>
    public partial class PAGE_1 : Page, INotifyPropertyChanged, INotifyCollectionChanged
    {
        public PAGE_1()
        {
            DataContext = this;
            InitializeComponent();

            Items2 = new((GlobalProjectData.GetProjectNames()).ToList());
            GlobalPages.page_1 = this;

        }


        #region bindingVariable
        private ObservableCollection<string> items2 = new ObservableCollection<string>(); //list of the projectName contained in the localApplication

        #endregion

        #region bindingMethods

        public ObservableCollection<string> Items2
        {
            get { return items2; }
            set
            {
                items2 = value;
                OnPropertyChanged(nameof(Items2));
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

        #region UImethods

        private void ButtonMainMenuSelectTypeClick(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            GlobalProjectData.CurrentProjectName = clickedButton.Content.ToString();
            GlobalProjectData.CurrentProjectData = new(new(GlobalProjectData.CurrentProjectName));

            GlobalPages.SetCurrentPage(GlobalPages.PAGE_3_1);
            GlobalPages.mainWindow.labelTemplateName.Content = GlobalProjectData.CurrentProjectName;// we update the banner to display the current projectName
            GlobalPages.mainWindow.StackPanelCurrentTemplate.Visibility = Visibility.Visible;
        


            }

        private void ButtonClickNew(object sender, RoutedEventArgs e)
        {
            GlobalPages.SetCurrentPage(GlobalPages.PAGE_4_1);

        }

        private void ButtonClickLoad(object sender, RoutedEventArgs e)

        {
            string zipPath = FileManager.OpenSelectZipLoadPopup();
            string dest = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "/UserData", System.IO.Path.GetFileNameWithoutExtension(zipPath));
            ZipFile.ExtractToDirectory(zipPath, dest);

            Items2.Add(System.IO.Path.GetFileNameWithoutExtension(dest));




        }
        #endregion

        #region privatemethod
        private void CopyDirectory(string sourceDirectory, string destinationDirectory)
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

        private void ButtonClickExport(object sender, RoutedEventArgs e)
        {
            GlobalPages.SetCurrentPage(GlobalPages.PAGE_1_1);
        }
        #endregion

    }
}
