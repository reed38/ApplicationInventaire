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


namespace ApplicationInventaire
{
    public partial class MainWindow : Window
    {
        public List<string> Items { get; set; }

        public static MainWindow Instance { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            GlobalPages.mainWindow = this;
            GlobalPages.SetCurrentPage(GlobalPages.PAGE_1);
            GlobalPages.CurrentPage = GlobalPages.PAGE_1;
            DataContext = this;
        }


        public void ButtonGoBackClick(object sender, RoutedEventArgs e)
        {
            GlobalPages.PageGoBack();
        }

        public static void HidePage()
        {
            Instance.MainFrame.Visibility = Visibility.Collapsed;
        }


    }
}
