using ApplicationInventaire.Core.GlobalPages;
using ApplicationInventaire.Core.GlobalProjectData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Diagnostics;

using ApplicationInventaire.Core.ProjectDataSet;

namespace ApplicationInventaire.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour PAGE_1.xaml
    /// </summary>
    public partial class PAGE_1 : Page
    {
        public PAGE_1()
        {
            InitializeComponent();
            ProjectData proj1=new ProjectData(new ProjectInfos("proj1"));
            ProjectData proj2 = new ProjectData(new ProjectInfos("proj2"));
            ProjectData proj3 = new ProjectData(new ProjectInfos("proj3"));

            GlobalPages.page_1 = this;
        





        }
        public List<string> Items { set; get; } = (GlobalProjectData.GetProjectNames()).ToList();





        private void ButtonMainMenuSelectTypeClick(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            GlobalProjectData.CurrentProjectName=clickedButton.Content.ToString();
            GlobalProjectData.InitializeGlobalProjectData();
            GlobalProjectData.RemainingSections = GlobalProjectData.CurrentProjectData.mySections;
            GlobalPages.SetCurrentPage(GlobalPages.PAGE_3_1);
           
        }

        private void ButtonClickNew(object sender, RoutedEventArgs e)
        {
            GlobalPages.SetCurrentPage(GlobalPages.PAGE_CREATIONTEST);

        }
    }
}
