using System;
using System.Collections.Generic;
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
using ApplicationInventaire.Core.ProjectDataSet;
using ApplicationInventaire.Core.GlobalProjectData;
using ApplicationInventaire.Core.PieceSections;
using NPOI.SS.Formula.PTG;
using NPOI.OpenXmlFormats.Dml.Diagram;

namespace ApplicationInventaire.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour PAGE_3_4.xaml
    /// </summary>
    public partial class PAGE_3_4 : Page, INotifyPropertyChanged
    {
        public PAGE_3_4()
        {
            InitializeComponent();
            DataContext = this;
            GlobalPages.page_3_4 = this;
            this.projectData = GlobalProjectData.CurrentProjectData;
            this.IndiceSection = 0;
            this.ImageSection3 = this.projectData.ImageSectionList[IndiceSection].Path;
            InitializeOverlay();



        }

        #region privateVariables
        
        private ProjectData projectData;
        private List<(Image, Piece)> OverlayImageList = new List<(Image, Piece)>();
        private int IndiceSection;
        private int ImageMarkerWidth = 20;
        private Piece CurrentPiece;
        #endregion

        #region bindingVariables
        private string imageSection3;
        private double xCoordinatevar;
        private double yCoordinatevar;

        #endregion

        #region BindingMethods
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

        public string ImageSection3
        {
            get { return imageSection3; }
            set
            {
                imageSection3 = value;
                OnPropertyChanged(nameof(ImageSection3));
            }
        }



        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region UIMethods
      
        private void ButtonClickPrevious(object sender, RoutedEventArgs e)
        {
            IndiceSection = (IndiceSection - 1);
            if (IndiceSection < 0)
            {
                IndiceSection = this.projectData.ImageSectionList.Length - 1;
            }
            foreach(ImageInfos imageInfo in projectData.ImageSectionList) 
            {
                if(imageInfo.Name.Equals(projectData.mySections[IndiceSection].SectionName))
                {
                    this.ImageSection3 = imageInfo.Path;
                }
            }
            InitializeOverlay();

        }

        private void ButtonClickNext(object sender, RoutedEventArgs e)
        {
            IndiceSection = (IndiceSection + 1) % (this.projectData.ImageSectionList.Length);
            foreach (ImageInfos imageInfo in projectData.ImageSectionList)
            {
                if (imageInfo.Name.Equals(projectData.mySections[IndiceSection].SectionName))
                {
                    this.ImageSection3 = imageInfo.Path;
                }
            }
            InitializeOverlay();

        }

        private void Canva_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point clickPosition = e.GetPosition(myCanva);
            double x = clickPosition.X;
            double y = clickPosition.Y;
            (Image, Piece) res = GetClickedPieceImage(clickPosition);
            if (res != (null, null))
            {
                ChangeLabelcoordinates(res.Item2.X, res.Item2.Y);
                CurrentPiece = res.Item2;
                this.LabelNameTag.Content = CurrentPiece.PieceName;
                InitializeNameTagAndDesciption();
            }
        }

        #endregion

        #region privateMethods

        private void InitializeNameTagAndDesciption()
        {
            this.TextBlockDesciption.Text = CurrentPiece.Description;
            this.TextBlockName.Text = CurrentPiece.PieceName;
        }


        private (Image, Piece) GetClickedPieceImage(Point clickPosition)
        {

            foreach ((Image, Piece) i in OverlayImageList)
            {
                var imageBounds = new Rect(Canvas.GetLeft(i.Item1), Canvas.GetTop(i.Item1), i.Item1.ActualWidth, i.Item1.ActualHeight);
                if (imageBounds.Contains(clickPosition.X, clickPosition.Y))
                {
                    return i;
                }

            }



            return (null, null);
        }

        #endregion

        #region GIMethods
        /// <summary>
        /// For each Section this function will create instances of red point image over existing Pieces
        /// </summary>
        private void InitializeOverlay()
        {
            ResetOverlay();

            foreach (Piece i in projectData.mySections[IndiceSection].PiecesList)
            {
                CreateImageInstance(i);

            }

        }
      
        private void ResetOverlay()
        {
            foreach (var i in OverlayImageList)
            {
                myCanva.Children.Remove(i.Item1);
            }
            OverlayImageList.Clear();
            this.LabelNameTag.Content = string.Empty;

        }

        private void CreateImageInstance(Piece piece)
        {
            Image image = new Image();
            image.Source = new BitmapImage(new Uri(GlobalProjectData.RedCirclePath, UriKind.Absolute));
            image.Width = ImageMarkerWidth;
            image.Height = ImageMarkerWidth;
            Canvas.SetLeft(image, piece.X- ImageMarkerWidth/2);
            Canvas.SetTop(image, piece.Y - ImageMarkerWidth / 2);
            myCanva.Children.Add(image);
            OverlayImageList.Add((image, piece));

        }

        private void ChangeLabelcoordinates(double x, double y)
        {
            Canvas.SetLeft(this.LabelNameTag, x - 60);
            Canvas.SetTop(this.LabelNameTag, y - 60);

        }
        #endregion
        
    }
}
