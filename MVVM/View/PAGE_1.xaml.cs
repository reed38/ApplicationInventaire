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
            InitializeComponent();
            ProjectData proj1=new ProjectData(new ProjectInfos("proj1"));
            ProjectData proj2 = new ProjectData(new ProjectInfos("proj2"));
            ProjectData proj3 = new ProjectData(new ProjectInfos("proj3"));
            Items2 =new( (GlobalProjectData.GetProjectNames()).ToList());
            //InitializeTestSet();
            GlobalPages.page_1 = this;
        





        }

        #region bindingVariable
        private ObservableCollection<string> items2  = new ObservableCollection<string>();

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



        private void ButtonClickSave(object sender, RoutedEventArgs e)
        {
        }

        #region UImethods

        private void ButtonMainMenuSelectTypeClick(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            GlobalProjectData.CurrentProjectName=clickedButton.Content.ToString();
            GlobalProjectData.ExcelContinuPath = null;
            GlobalPages.SetCurrentPage(GlobalPages.PAGE_3_1);
           
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
        private void  CopyDirectory(string sourceDirectory, string destinationDirectory)
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
        #endregion

        private void InitializeTestSet()
        {
            GlobalProjectData.CurrentProjectName = ("TestSet");
            ProjectData tmp = new ProjectData(new ProjectInfos(GlobalProjectData.CurrentProjectName));

            tmp.myProjectInfos.Author = "respo";
            List<Section> listSection = new List<Section>();


            Section Alphabet = new Section("SECTION_alphabet");
            Alphabet.PiecesList.Add(new Piece(128, 60, "A", 1));
            Alphabet.PiecesList.Add(new Piece(350, 66, "B", 1));
            Alphabet.PiecesList.Add(new Piece(610, 72, "C", 1));
            Alphabet.PiecesList.Add(new Piece(902, 77, "D", 1));
            Alphabet.PiecesList.Add(new Piece(125, 234, "E", 1));
            Alphabet.PiecesList.Add(new Piece(334, 254, "F", 1));
            Alphabet.PiecesList.Add(new Piece(602, 268, "G", 1));
            Alphabet.PiecesList.Add(new Piece(910, 240, "H", 1));
            Alphabet.PiecesList.Add(new Piece(124, 466, "I", 1));
            Alphabet.PiecesList.Add(new Piece(341, 446, "J", 1));
            Alphabet.PiecesList.Add(new Piece(614, 456, "K", 1));
            Alphabet.PiecesList.Add(new Piece(898, 493, "L", 1));
            listSection.Add(Alphabet);

            Section Nombre = new Section("SECTION_nombres");
            Nombre.PiecesList.Add(new Piece(102, 69, "1", 2));
            Nombre.PiecesList.Add(new Piece(342, 78, "8", 2));
            Nombre.PiecesList.Add(new Piece(666, 98, "9", 2));
            Nombre.PiecesList.Add(new Piece(924, 108, "7", 2));
            Nombre.PiecesList.Add(new Piece(93, 231, "3", 2));
            Nombre.PiecesList.Add(new Piece(336, 236, "2", 2));
            Nombre.PiecesList.Add(new Piece(650, 247, "15", 2));
            Nombre.PiecesList.Add(new Piece(916, 241, "5", 2));
            Nombre.PiecesList.Add(new Piece(100, 422, "4", 2));
            Nombre.PiecesList.Add(new Piece(304, 433, "6", 2));
            Nombre.PiecesList.Add(new Piece(662, 440, "10", 2));
            Nombre.PiecesList.Add(new Piece(920, 444, "12", 2));

            listSection.Add(Nombre);

            Section Couleur = new Section("SECTION_couleurs");
            Couleur.PiecesList.Add(new Piece(199, 80, "gris", 3));
            Couleur.PiecesList.Add(new Piece(422, 86, "marron", 3));
            Couleur.PiecesList.Add(new Piece(650, 80, "rouge", 3));
            Couleur.PiecesList.Add(new Piece(408, 44, "orange", 3));
            Couleur.PiecesList.Add(new Piece(543, 43, "jaune", 3));
            Couleur.PiecesList.Add(new Piece(38, 145, "blanc", 3));
            Couleur.PiecesList.Add(new Piece(168, 145, "rose", 3));
            Couleur.PiecesList.Add(new Piece(290, 136, "violet", 3));
            Couleur.PiecesList.Add(new Piece(406, 142, "bleu", 3));
            Couleur.PiecesList.Add(new Piece(543, 133, "vert", 3));



            listSection.Add(Couleur);

            tmp.mySections = listSection;
            //Console.WriteLine("données contenues en mémoire");
            tmp.InitializePieceFromExcel();

            tmp.DispDataProjectData();
            tmp.Save();
        }

        private void ButtonClickExport(object sender, RoutedEventArgs e)
        {
            GlobalPages.SetCurrentPage(GlobalPages.PAGE_1_1);
        }
    }
}
