using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
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
using Microsoft.Win32;

namespace ApplicationInventaire.MVVM.View
{
    /// <summary>
    ///The User clicked on the Plan button.
    /// </summary>
    public partial class PAGE_3_3 : Page, INotifyCollectionChanged, INotifyPropertyChanged
    {
        public PAGE_3_3()
        {
            InitializeComponent();
            GlobalPages.page_3_3 = this;
            DataContext = this;
            PlansNameList = new ObservableCollection<string>(GlobalProjectData.GetPlansNames());// we get the list of tha plans
            PlansPathList = GlobalProjectData.GetPlansPath(); //we het th path to the plan pdf
            InitilizePdfReaderExe();
        }

        #region Privatevariables
        private string[] PlansPathList;
        private string PdfReaderExecutablePath;
        #endregion

        #region BindingVariables
        private ObservableCollection<string> plansNameList = new ObservableCollection<string>();

        private string selectedValue;
        #endregion

        #region BindingMethods

        public ObservableCollection<string> PlansNameList
        {
            get { return plansNameList; }
            set
            {
                plansNameList = value;
                OnPropertyChanged(nameof(PlansNameList));
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

        #region UIMethods
       /// <summary>
       /// The user clicked on the button slect file
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void ButtonClickOpenFile(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(selectedValue))
            {
                POPUP.ShowPopup("Please select a template to export");
                return;
            };
            string PdfPath = PlansPathList[PlansNameList.IndexOf(selectedValue)];
            OpenPdf(PdfPath);

        }

        #endregion

        #region PrivateMethods
        private void ListBoxSectionFileSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            selectedValue = (string)listBox.SelectedItem;
        }

        /// <summary>
        /// This used is used to gat the path of the excecutable cofigured as default pdf reader on windows. It updates the private variable PdfReaderExecutablePath
        /// </summary>
        private void InitilizePdfReaderExe()
        {
            string fileExtension = ".pdf";
            // Get the file class key associated with the file extension
            string fileClassKey = Registry.ClassesRoot.OpenSubKey(fileExtension)?.GetValue("")?.ToString();

            if (fileClassKey != null)
            {
                // Get the default application command associated with the file class
                string defaultApplicationCommand = Registry.ClassesRoot.OpenSubKey($"{fileClassKey}\\shell\\open\\command")?.GetValue("")?.ToString();

                if (defaultApplicationCommand != null)
                {
                    // Extract the executable path from the command string
                    PdfReaderExecutablePath = defaultApplicationCommand.Split('"')[1];


                }

            }
        }

        /// <summary>
        /// This function Open the pdf given in argument using the path of the executable found earlier with InitilizePdfReaderExe()
        /// </summary>
        /// <param name="pdfFilePath"> path to the pdf file</param>
        private void OpenPdf(string pdfFilePath)

        {
            string modifiedPdfFilePath = $"\"{pdfFilePath}\""; //if pdfFilePath isn't wrapped like that the pdf app will try to open multiple files, which make error file not found

            Process.Start(PdfReaderExecutablePath, modifiedPdfFilePath);

        }

        #endregion

    }
}
