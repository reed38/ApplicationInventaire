using System;
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

using ApplicationInventaire.Core.DatabaseManagement;
using ApplicationInventaire.Core.ExcelManagement;
using ApplicationInventaire.Core.GlobalPages;
using ApplicationInventaire.Core.PieceSections;
using ApplicationInventaire.Core.ProjectDataSet;
using ApplicationInventaire.Core.GlobalProjectData;
using System.IO;
using System.ComponentModel;

namespace ApplicationInventaire
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public List<string> Items { get; set; }
        public static MainWindow Instance { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            GlobalPages.mainWindow = this;
            GlobalPages.SetCurrentPage(GlobalPages.PAGE_1);
            GlobalPages.CurrentPage = GlobalPages.PAGE_1;
            ImageLogo = GlobalTemplateData.AventechLogoPath;
            DataContext = this;
            GlobalTemplateData.UserRigth = 0;
        }


        private string imageLogo;

        public string ImageLogo
        {
            get { return imageLogo; }
            set
            {
                imageLogo = value;
                OnPropertyChanged(nameof(ImageLogo));
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        public event PropertyChangedEventHandler PropertyChanged;

        public void ButtonGoBackClick(object sender, RoutedEventArgs e)
        {
            if (GlobalPages.CurrentPage == GlobalPages.PAGE_3_6_1 || GlobalPages.CurrentPage == GlobalPages.PAGE_3_2 || GlobalPages.CurrentPage == GlobalPages.PAGE_4_1)  //if user is currently editing a template
            {
                MessageBoxResult result = MessageBox.Show("Êtes-vous vraiment sûr de vouloir quitter? Les modifications seront perdus.", "Confirmer", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    GlobalPages.PageGoBack();

                }


            }

            else if (GlobalPages.CurrentPage == GlobalPages.PAGE_3_6_2 || GlobalPages.CurrentPage == GlobalPages.PAGE_4_2)
            {
             
                    MessageBoxResult result = MessageBox.Show("Êtes-vous vraiment sûr de vouloir quitter? Les modifications seront perdus.", "Confirmer", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        GlobalPages.PageGoBack();
                        GlobalPages.PageGoBack();


                    }


                


            }
            else
            {
                GlobalPages.PageGoBack();

            }


        }

        public static void HidePage()
        {
            Instance.MainFrame.Visibility = Visibility.Collapsed;
        }

        private void ButtonSettingsClick(object sender, RoutedEventArgs e)
        {
            GlobalPages.SetCurrentPage(GlobalPages.PAGE_0);
        }
    }
}
