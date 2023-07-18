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
using NPOI.HPSF;
using NPOI.OpenXmlFormats.Dml.Diagram;
using System.IO;
using Path = System.IO.Path;
using System.ComponentModel.Design;

namespace ApplicationInventaire.MVVM.View
{



    /// <summary>
    /// Logique d'interaction pour PAGE_4_1.xaml
    /// </summary>
    public partial class PAGE_4_1 : Page, INotifyPropertyChanged, INotifyCollectionChanged
    {

        public PAGE_4_1()
        {
            InitializeComponent();
            DataContext = this;
            GlobalPages.page_4_1 = this;

        }

        #region PrivateVariables
        private string Description;
        private ObservableCollection<ImageInfos> imageSectionsInfos = new ObservableCollection<ImageInfos>();
        private List<string> PlansPathList= new List<string> ();
        private string excelPath;
        #endregion

        #region BindingVariables
        private ObservableCollection<string> imageSectionsName = new ObservableCollection<string>();
        private ObservableCollection<string> plansNameList= new ObservableCollection<string>();
        private string excelName;

        private string selectedValueSection;
        private string selectedValuePlan;

        #endregion

        #region BindingMethods

        public ObservableCollection<string> ImageSectionsName
        {
            get { return imageSectionsName; }
            set
            {
                imageSectionsName = value;
                OnPropertyChanged(nameof(ImageSectionsName));
            }
        }

        public ObservableCollection<string> PlansNameList
        {
            get { return plansNameList; }
            set
            {
                plansNameList = value;
                OnPropertyChanged(nameof(PlansNameList));
            }
        }

        public string ExcelName
        {
            get { return excelName; }
            set
            {
                excelName = value;
                OnPropertyChanged(nameof(ExcelName));
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
        private void ListBoxSectionFileSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            selectedValueSection = (string)listBox.SelectedItem;

        }

    
        
        private bool CheckAllIsHere()
        {
            bool res = true;
            if (string.IsNullOrEmpty(TextBoxAuthor.Text))
            {
                res = false;
                ShowPopup("please enter author");
            }
            if (string.IsNullOrEmpty(TextBoxDescription.Text))
            {
                res = false;
                ShowPopup("please enter description");
            }
            if (string.IsNullOrEmpty(TextBoxName.Text))
            {
                res = false;
                ShowPopup("please enter name");
            }
            if (string.IsNullOrEmpty(excelPath))
            {
                res = false;
                ShowPopup("please select an excel file");
            }
            if (imageSectionsInfos.Count == 0)
            {
                res = false;
                ShowPopup("please select at least 1 image");
            }
            if (PlansNameList.Count == 0)
            {
                res = false;
                ShowPopup("please select at least 1 Plan");
            }


            string[] projectList=GlobalProjectData.GetProjectNames();
            foreach(string project in projectList) 
            {
                if(project.Equals(TextBoxName.Text))
                {
                    res = false;
                    ShowPopup("This name is already used ");
                }
            }
            return res;
        }

        #endregion

        #region UImethods


      


        private void ButtonClickSelectImage(object sender, RoutedEventArgs e)
        {
            string[] tmp = FileManager.OpenImagePopup();
            if(tmp.Length>0)
            {
                foreach (string i in tmp)
                {
                    imageSectionsInfos.Add(new ImageInfos(Path.GetFileNameWithoutExtension(i), i));
                    ImageSectionsName.Add(Path.GetFileNameWithoutExtension(i));
                }
            }
        

        }


        private void ShowPopup(string message)
        {
            MessageBox.Show(message, "Field Missing", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void ButtonClickSelectExcel(object sender, RoutedEventArgs e)
        {
            excelPath = FileManager.OpenExcelFilePopup();
            ExcelName = Path.GetFileNameWithoutExtension(excelPath);

        }
        private void ButtonClickSaveContinu(object sender, RoutedEventArgs e)
        {
            if (CheckAllIsHere() == false)
            {
                return;
            }
            ProjectInfos projectInfos = new ProjectInfos(TextBoxName.Text);
            projectInfos.Description = TextBoxDescription.Text;
            projectInfos.Author = TextBoxAuthor.Text;
            File.Copy(excelPath, projectInfos.ExcelPath, true);
            File.Copy(excelPath, projectInfos.TmpExcelPath, true);
            ProjectData projectData = new ProjectData(projectInfos);
            
            File.Copy(excelPath, projectInfos.ExcelPath, true);
            File.Copy(excelPath, projectInfos.TmpExcelPath, true);


            foreach (ImageInfos i in imageSectionsInfos)
            {
                string destinationPath = Path.Combine(projectData.myProjectInfos.ImageSectionPath, i.Name);
                File.Copy(i.Path, destinationPath, true);
                projectData.mySections.Add(new Core.PieceSections.Section(i.Name));

           }
            for (int i=0; i < PlansNameList.Count;i++)
            {
                string destinationPath = Path.Combine(projectData.myProjectInfos.PlansPath, PlansNameList[i]);
                File.Copy(PlansPathList[i], destinationPath, true);

            }
            projectData.Save();
            GlobalProjectData.CurrentProjectName = projectData.myProjectInfos.ProjectName;
            GlobalPages.SetCurrentPage(GlobalPages.PAGE_4_2);


        }

        private void ListBoxPlanFileSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            selectedValuePlan = (string)listBox.SelectedItem;
            


    }


    private void ButtonClickSelectPlans(object sender, RoutedEventArgs e)
        {
            string[] tmp = FileManager.OpenPdfPopup();
            foreach (string i in tmp)
            {
                PlansNameList.Add(Path.GetFileNameWithoutExtension(i));
                PlansPathList.Add(i);
            }

        }

        private void ButtonClickDeleteImageSelected(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < imageSectionsInfos.Count; i++)
            {

                if (imageSectionsInfos[i].Name.Equals(selectedValueSection))
                {
                    ImageSectionsName.Remove(imageSectionsInfos[i].Name);
                    imageSectionsInfos.Remove(imageSectionsInfos[i]);
                }
            }
        }


        private void ButtonClickDeletePlanSelected(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < PlansNameList.Count; i++)
            {

                if (PlansNameList[i].Equals(selectedValuePlan))
                {
                    PlansPathList.Remove(PlansNameList[i]);
                    PlansNameList.Remove(PlansNameList[i]);
                }
            }

        }
    }
    #endregion
}
