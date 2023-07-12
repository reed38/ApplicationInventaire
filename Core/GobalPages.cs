using ApplicationInventaire.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using ApplicationInventaire.Core.Config;
using ApplicationInventaire.Core.DatabaseManagement;
using ApplicationInventaire.Core.ExcelManagement;
using ApplicationInventaire.Core.GlobalPages;
using ApplicationInventaire.Core.PieceSections;
using ApplicationInventaire.Core.ProjectDataSet;
using ApplicationInventaire.MVVM.View;
using System.Runtime.CompilerServices;

namespace ApplicationInventaire.Core.GlobalPages
{
    public static class GlobalPages
    {
        public static Uri CurrentPage { get; set; }
        public static List<Uri> LastPages { get; set; } = new List<Uri>();
        public static Uri PAGE_1 = new Uri("MVVM/View/PAGE_1.xaml", UriKind.Relative);
        public static Uri PAGE_3_1 = new Uri("MVVM/View/PAGE_3_1.xaml", UriKind.Relative);
        public static Uri PAGE_3_2 = new Uri("MVVM/View/PAGE_3_2.xaml", UriKind.Relative);
        public static Uri PAGE_CREATIONTEST= new Uri("MVVM/View/PageCreationTest.xaml", UriKind.Relative);



        public static MainWindow mainWindow;
        public static PAGE_1 page_1;
        public static PAGE_3_1 page_3_1;
        public static PAGE_3_2 page_3_2;
        public static PageCreationTest page_CreationTest;




        public static void SetCurrentPage(Uri newPage)
        {
            GlobalPages.LastPages.Add(GlobalPages.CurrentPage);
            GlobalPages.CurrentPage = newPage;
            mainWindow.MainFrame.Source = GlobalPages.CurrentPage;
            mainWindow.MainFrame.Visibility = Visibility.Visible;

        }

        public static void SetCurrentPageBack(Uri newPage)
        {
            GlobalPages.CurrentPage = newPage;
            mainWindow.MainFrame.Source = GlobalPages.CurrentPage;
            mainWindow.MainFrame.Visibility = Visibility.Visible;

        }



    }
}
       
























