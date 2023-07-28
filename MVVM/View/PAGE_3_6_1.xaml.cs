using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
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
using NPOI.OpenXmlFormats.Dml.Diagram;
using Path = System.IO.Path;

namespace ApplicationInventaire.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour PAGE_3_6_1.xaml
    /// The user clicked on the button edit template.
    /// </summary>
    public partial class PAGE_3_6_1 : Page, INotifyPropertyChanged, INotifyCollectionChanged
    {
        public PAGE_3_6_1()
        {
            InitializeComponent();
            GlobalPages.page_3_6_1 = this;
            DataContext = this;
            projectData = GlobalProjectData.CurrentProjectData;
            InitializeFields();
            

        }


        #region PrivateVariables
        private ProjectData projectData;
        private ObservableCollection<ImageInfos> imageSectionsInfos = new ObservableCollection<ImageInfos>();
        private ObservableCollection<ImageInfos> imageReleveInfos = new ObservableCollection<ImageInfos>();
        private List<string> PlansPathList = new List<string>();
        private string excelPath;
        #endregion

        #region BindingVariables

        private ObservableCollection<string> imageSectionsName = new ObservableCollection<string>(); //used to display sections names on the page
        private ObservableCollection<string> imageReleveName = new ObservableCollection<string>(); //used to display releve names on the age
        private ObservableCollection<string> plansNameList = new ObservableCollection<string>(); //used to display plan names on the page
        private string excelName;
        private string selectedValueSection;//field in the section list currently selected by the user
        private string selectedValueReleve; //field in the Releve list currently selected by the user
        private string selectedValuePlan; //field in the Plan list currently selected by the user

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
        /// <summary>
        /// this function will load curent project informations about author, description, Section Image, plan  in the TextBoxes and List as well as in the data structure. 
        /// </summary>
        private void InitializeFields()
        {
            ///textBox
            this.TextBoxDescription.Text = projectData.myProjectInfos.Description;
            this.TextBoxName.Text = projectData.myProjectInfos.ProjectName;
            ExcelName = Path.GetFileNameWithoutExtension(projectData.myProjectInfos.ExcelPath);
            excelPath = projectData.myProjectInfos.ExcelPath;
            //Files
            imageReleveInfos = new ObservableCollection<ImageInfos>(projectData.ImageReleveList);
            imageSectionsInfos = new ObservableCollection<ImageInfos>(projectData.ImageSectionList);

            foreach (var i in imageSectionsInfos)
            {
                ImageSectionsName.Add(i.Name);
            }
            foreach(var i in imageReleveInfos)
            {
                ImageReleveName.Add(i.Name);
            }
            PlansPathList =  ( Directory.GetFiles(projectData.myProjectInfos.PlansPath)).ToList();
            foreach(var i in  PlansPathList)
            {
                PlansNameList.Add(Path.GetFileNameWithoutExtension(i));
            }

        }

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


            string[] projectList = GlobalProjectData.GetProjectNames();
            foreach (string project in projectList)
            {
                if (project.Equals(TextBoxName.Text))
                {
                    res = false;
                    POPUP.ShowPopup("Ce nom est déjà utilisé");
                    break;
                }
            }
            return res;
        }

        #endregion

        #region UIMethods

        /// <summary>
        /// The user clicked on the Button SelectImageSection. It opens pop up windows to select images files on the computer. If images were selected, their names and paths are 
        /// are added to private variables  imageSectionsInfos and  ImageSectionName
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClickSelectImageSection(object sender, RoutedEventArgs e)
        {
            string[] tmp = FileManager.OpenImagePopup();
            if (tmp!= null && tmp.Length > 0)
            {
                foreach (string i in tmp)
                {
                    imageSectionsInfos.Add(new ImageInfos(Path.GetFileNameWithoutExtension(i), i));
                    ImageSectionsName.Add(Path.GetFileNameWithoutExtension(i));
                }
            }
        }

        /// <summary>
        /// The user clicked on the Button SelectImageReleve. It opens pop up windows to select images files on the computer. If images were selected, their names and paths are 
        /// are added to private variables  imageReleveInfos and  ImageReleveName
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClickSelectImageReleve(object sender, RoutedEventArgs e)
        {
            string[] tmp = FileManager.OpenImagePopup();
            if (tmp!= null &&tmp.Length > 0)
            {
                foreach (string i in tmp)
                {
                    imageReleveInfos.Add(new ImageInfos(Path.GetFileNameWithoutExtension(i), i));
                    ImageReleveName.Add(Path.GetFileNameWithoutExtension(i));
                }
            }
        }

        /// <summary>
        /// This function open a pop up window to select an excel file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClickSelectExcel(object sender, RoutedEventArgs e)
        {
            excelPath = FileManager.OpenExcelFilePopup();
            ExcelName = Path.GetFileNameWithoutExtension(excelPath);
        }

        /// <summary>
        /// The user clicked on the button Save and Continu. This function create the file structure correponding to a project (ith the right name), and copy selected files in it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClickSaveContinu(object sender, RoutedEventArgs e)
        {
            if (CheckAllIsHere() == false)
            {
                return;
            }
            ProjectInfos projectInfos = new ProjectInfos(TextBoxName.Text);
            
            projectInfos.Description = TextBoxDescription.Text;
            projectInfos.Author = GlobalProjectData.CurrentProjectData.myProjectInfos.Author;
            projectInfos.CreationDate = GlobalProjectData.CurrentProjectData.myProjectInfos.CreationDate;
           
            
            File.Copy(excelPath, projectInfos.ExcelPath, true);
            File.Copy(excelPath, projectInfos.TmpExcelPath, true);
            File.Copy(GlobalProjectData.CurrentProjectData.myProjectInfos.DatabasePath, projectInfos.DatabasePath,true);
           
            ProjectData projectData = new ProjectData(projectInfos);
           
            projectData.myProjectInfos = projectInfos;

            File.Copy(excelPath, projectInfos.ExcelPath, true);
            File.Copy(excelPath, projectInfos.TmpExcelPath, true);
            

            foreach (ImageInfos i in imageSectionsInfos) //saving selected image section to memory
            {
                string destinationPath = Path.Combine(projectData.myProjectInfos.ImageSectionPath, i.Name);
                File.Copy(i.Path, destinationPath,true);

                
                foreach(Core.PieceSections.Section j in projectData.mySections)
                {
                    if(j.SectionName.Equals( i.Name))
                    {
                        break;
                    }
                    if(projectData.mySections.IndexOf(j)==projectData.mySections.Count-1)
                    {
                        projectData.mySections.Add(new Core.PieceSections.Section(i.Name));
                        break;

                    }

                }
               


            }

            foreach (ImageInfos i in imageReleveInfos)
            {
                string destinationPath = Path.Combine(projectData.myProjectInfos.ImageRelevePath, i.Name);
               
                    File.Copy(i.Path, destinationPath,true);

                

            }
            for (int i = 0; i < PlansNameList.Count; i++)
            {
                string destinationPath = Path.Combine(projectData.myProjectInfos.PlansPath, PlansNameList[i]);
            
                    File.Copy(PlansPathList[i], destinationPath,true);

                
           
            }
       
            
            projectData.GetSectionsNames();
            projectData.GetRelevesNames();
            projectData.InitializePieceFromExcel();
            projectData.myProjectInfos.LastEditor = Environment.UserName;
            projectData.myProjectInfos.LastEditionDate = DateTime.Now;
            projectData.Save();
            projectData.UpdateSection();
            GlobalProjectData.CurrentProjectName = projectData.myProjectInfos.ProjectName;
            GlobalProjectData.CurrentProjectData =projectData;
            GlobalPages.SetCurrentPage(GlobalPages.PAGE_3_6_2);


        }

        /// <summary>
        /// This function open a pop up windows to select pdf files.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClickSelectPlans(object sender, RoutedEventArgs e)
        {
            string[] tmp = FileManager.OpenPdfPopup();
            if( tmp !=null && tmp.Length>0)
            {
                foreach (string i in tmp)
                {
                    PlansNameList.Add(Path.GetFileNameWithoutExtension(i));
                    PlansPathList.Add(i);
                }

            }
        

        }

        /// <summary>
        /// The User clicked on the button delete selected under the plans list. This will remove the path and name of the pdf from the data structure.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// The User clicked on the button delete selected under the Sections Image list. This will remove the path and name of the Image from the data structure.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// The User clicked on the button delete selected under the Releve Image list. This will remove the path and name of the Image from the data structure.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        #endregion



    }


}


