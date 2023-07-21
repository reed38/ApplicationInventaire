using ApplicationInventaire.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using ApplicationInventaire.Core.GlobalProjectData;
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
        public static Uri MAINWINDOW = new Uri("MainWindow.xaml", UriKind.Relative);
        public static Uri PAGE_1 = new Uri("MVVM/View/PAGE_1.xaml", UriKind.Relative);
        public static Uri PAGE_1_1 = new Uri("MVVM/View/PAGE_1_1.xaml", UriKind.Relative);
        public static Uri PAGE_3_1 = new Uri("MVVM/View/PAGE_3_1.xaml", UriKind.Relative);
        public static Uri PAGE_3_2 = new Uri("MVVM/View/PAGE_3_2.xaml", UriKind.Relative);
        public static Uri PAGE_3_3 = new Uri("MVVM/View/PAGE_3_3.xaml", UriKind.Relative);
        public static Uri PAGE_3_4 = new Uri("MVVM/View/PAGE_3_4.xaml", UriKind.Relative);
        public static Uri PAGE_3_6_1 = new Uri("MVVM/View/PAGE_3_6_1.xaml", UriKind.Relative);
        public static Uri PAGE_3_6_2 = new Uri("MVVM/View/PAGE_3_6_2.xaml", UriKind.Relative);
        public static Uri PAGE_4_1 = new Uri("MVVM/View/PAGE_4_1.xaml", UriKind.Relative);
        public static Uri PAGE_4_2 = new Uri("MVVM/View/PAGE_4_2.xaml", UriKind.Relative);
        public static Uri PAGE_5_1 = new Uri("MVVM/View/PAGE_5_1.xaml", UriKind.Relative);
        public static Uri PAGE_5_2 = new Uri("MVVM/View/PAGE_5_2.xaml", UriKind.Relative);

        public static MainWindow mainWindow;
        public static PAGE_1 page_1;
        public static PAGE_1_1 page_1_1;
        public static PAGE_3_1 page_3_1;
        public static PAGE_3_2 page_3_2;
        public static PAGE_3_3 page_3_3;
        public static PAGE_3_4 page_3_4;
        public static  PAGE_3_6_1 page_3_6_1;
        public static PAGE_3_6_2 page_3_6_2;
        public static PAGE_4_1 page_4_1;
        public static PAGE_4_2 page_4_2;
        public static PAGE_5_1 page_5_1;
        public static PAGE_5_2 page_5_2;

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
            GlobalProjectData.GlobalProjectData.ExcelContinuPath = null;
        }

        public static void PageGoBack()
        {

            if (GlobalPages.LastPages.Count > 1)
            {
                int i = GlobalPages.LastPages.Count - 1;
                Uri tmp = GlobalPages.LastPages[i];
                GlobalPages.LastPages.Remove(tmp);
                if(GlobalPages.CurrentPage==PAGE_3_2)
                {
               
                   page_3_2.BackgroundImageSection.ImageSource = null;
                    page_3_2.ImageReleveName.Source = null;
                        }
                GlobalPages.CurrentPage = tmp;
                GlobalPages.SetCurrentPageBack(GlobalPages.CurrentPage);
            }
            if(GlobalPages.CurrentPage==PAGE_1)
            {
                mainWindow.StackPanelCurrentTemplate.Visibility= Visibility.Collapsed;
            }
            
        }
    }
}
       
























