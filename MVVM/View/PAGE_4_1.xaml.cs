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
    /// The user clicked on the button new.
    /// Most of the code used here is the same as PAGE_3_6_1
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
        private ObservableCollection<ImageInfos> imageSectionsInfos = new ObservableCollection<ImageInfos>();
        private ObservableCollection<ImageInfos> imageReleveInfos = new ObservableCollection<ImageInfos>();
        private List<string> PlansPathList = new List<string>();
        private string excelPath;
        #endregion

        #region BindingVariables

        private ObservableCollection<string> imageSectionsName = new ObservableCollection<string>();
        private ObservableCollection<string> imageReleveName = new ObservableCollection<string>();
        private ObservableCollection<string> plansNameList = new ObservableCollection<string>();
        private string excelName;
        private string selectedValueSection;
        private string selectedValueReleve;
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
        public ObservableCollection<string> ImageReleveName
        {
            get { return imageReleveName; }
            set
            {
                imageReleveName = value;
                OnPropertyChanged(nameof(ImageReleveName));
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




        

        private void ListBoxPlanFileSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            selectedValuePlan = (string)listBox.SelectedItem;



        }

        private void ListBoxReleveSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            selectedValueReleve = (string)listBox.SelectedItem;
        }

        private void ListBoxSectionSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            selectedValueSection = (string)listBox.SelectedItem;
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


        private bool CheckAllIsHere()
        {
            bool res = true;

            if (string.IsNullOrEmpty(TextBoxDescription.Text))
            {
                res = false;
                POPUP.ShowPopup("Veillez saisir une descirption");
            }
            if (string.IsNullOrEmpty(TextBoxName.Text))
            {
                res = false;
                POPUP.ShowPopup("Veuillez saisir le nom du template");
            }
            if (string.IsNullOrEmpty(excelPath))
            {
                res = false;
                POPUP.ShowPopup("Veuillez sélectionner un fichier excel");
            }
            if (imageSectionsInfos.Count == 0)
            {
                res = false;
                POPUP.ShowPopup("Veuillez sélectionner au moins une image de section");
            }
            if (PlansNameList.Count == 0)
            {
                res = false;
                POPUP.ShowPopup("veuillez sélectionner au moins 1 plan");
            }




            string[] projectList = GlobalData.GetProjectNames();
            foreach (string project in projectList)
            {
                if (project.Equals(TextBoxName.Text))
                {
                    res = false;
                    POPUP.ShowPopup("This name is already used ");
                    break;
                }
            }
            return res;
        }

        #endregion

        #region UImethods

        private void ButtonClickSelectImageSection(object sender, RoutedEventArgs e)
        {
            string[] tmp = FileManager.OpenImagePopup();
            if( tmp!=null && (tmp.Length > 0))
            {
                foreach (string i in tmp)
                {
                    imageSectionsInfos.Add(new ImageInfos(Path.GetFileNameWithoutExtension(i), i));
                    ImageSectionsName.Add(Path.GetFileNameWithoutExtension(i));
                }
            }
        }

        private void ButtonClickSelectImageReleve(object sender, RoutedEventArgs e)
        {
            string[] tmp = FileManager.OpenImagePopup();
            if( tmp!=null && (tmp.Length > 0))
            { 
                foreach (string i in tmp)
                {
                    imageReleveInfos.Add(new ImageInfos(Path.GetFileNameWithoutExtension(i), i));
                    ImageReleveName.Add(Path.GetFileNameWithoutExtension(i));
                }
            }
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
            TemplateInfos TemplateInfos = new TemplateInfos(TextBoxName.Text);
            TemplateInfos.Description = TextBoxDescription.Text;
            File.Copy(excelPath, TemplateInfos.ExcelPath, true);
            File.Copy(excelPath, TemplateInfos.TmpExcelPath, true);
            TemplateData projectData = new TemplateData(TemplateInfos);

            File.Copy(excelPath, TemplateInfos.ExcelPath, true);
            File.Copy(excelPath, TemplateInfos.TmpExcelPath, true);


            foreach (ImageInfos i in imageSectionsInfos) //saving selected image section to directory
            {
                string destinationPath = Path.Combine(projectData.myTemplateInfos.ImageSectionPath, i.Name);
                File.Copy(i.Path, destinationPath, true);
                projectData.mySections.Add(new Core.PieceSections.Section(i.Name));

            }

            foreach(ImageInfos i in imageReleveInfos) //saving releve image to directory
            {
                string destinationPath=Path.Combine(projectData.myTemplateInfos.ImageRelevePath, i.Name);
                File.Copy(i.Path, destinationPath);
            }
            for (int i = 0; i < PlansNameList.Count; i++) //saving plans
            {
                string destinationPath = Path.Combine(projectData.myTemplateInfos.PlansPath, PlansNameList[i]);
                File.Copy(PlansPathList[i], destinationPath, true);

            }
            projectData.myTemplateInfos.Author = Environment.UserName;
            projectData.myTemplateInfos.CreationDate= DateTime.Now;
            projectData.myTemplateInfos.LastEditor = Environment.UserName;
            projectData.myTemplateInfos.LastEditionDate = DateTime.Now;
            projectData.Save();
            GlobalTemplateData.CurrentProjectName = projectData.myTemplateInfos.ProjectName;
            GlobalTemplateData.CurrentProjectData = new TemplateData(new TemplateInfos(new(TemplateInfos.ProjectName)));
            GlobalPages.SetCurrentPage(GlobalPages.PAGE_4_2);


        }

        private void ButtonClickSelectPlans(object sender, RoutedEventArgs e)
        {
            string[] tmp = FileManager.OpenPdfPopup();
            if(tmp!=null)
            {
                foreach (string i in tmp)
                {
                    PlansNameList.Add(Path.GetFileNameWithoutExtension(i));
                    PlansPathList.Add(i);
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

        private void ButtonClickDeleteSectionSelected(object sender, RoutedEventArgs e)
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

        private void ButtonClickDeleteReleveSelected(object sender, RoutedEventArgs e)
        {

            for (int i = 0; i < imageReleveInfos.Count; i++)
            {

                if (imageReleveInfos[i].Name.Equals(selectedValueReleve))
                {
                    ImageReleveName.Remove(imageReleveInfos[i].Name);
                    imageReleveInfos.Remove(imageReleveInfos[i]);
                }
            }

        }

        
    }

   
    #endregion
}
