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
using System.IO;
using System.ComponentModel;
using ApplicationInventaire.Core.GlobalPages;
using Microsoft.Win32;

namespace ApplicationInventaire.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour PageCreationTest.xaml
    /// </summary>
    public partial class PageCreationTest : Page, INotifyPropertyChanged
    {
        public PageCreationTest()
        {
            InitializeComponent();
            DataContext = this;
            GlobalPages.page_CreationTest = this;

        }



        #region bindingVariablesSources
        private string imageSection;
        private double xCoordinatevar;
        private double yCoordinatevar;
        #endregion
        #region bindingMethods
        public double XCoordinate
        {
            get { return xCoordinatevar; }
            set
            {
                xCoordinatevar = value;
                OnPropertyChanged(nameof(XCoordinate));
            }
        }
        public double YCoordinate
        {
            get { return yCoordinatevar; }
            set
            {
                yCoordinatevar = value;
                OnPropertyChanged(nameof(YCoordinate));
            }
        }

        public string ImageSection
        {
            get { return imageSection; }
            set
            {
                imageSection = value;
                OnPropertyChanged(nameof(ImageSection));
            }
        }

      

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        public event PropertyChangedEventHandler PropertyChanged;





        #endregion

        #region UImethods

        #endregion

        private void ButtonClickOpenFile(object sender, RoutedEventArgs e)
        {
              OpenFileDialog openFileDialog = new OpenFileDialog();

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                
               ImageSection= selectedFilePath;
               


                // Use the selected file path as needed
            }

        }

        private void CurrentSectionImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image image = (Image)sender;
            XCoordinate = (int)e.XButton1;
            Point position = e.GetPosition(image);
            XCoordinate = position.X;
            YCoordinate = position.Y;
            Thickness thickness = new Thickness();
            thickness.Top = YCoordinate-25 ;
            thickness.Bottom=YCoordinate - 25;
            thickness.Left = XCoordinate - 25;
            thickness.Right= XCoordinate ;
            this.RedCircleImage.Margin = thickness;


        }
    }
}
