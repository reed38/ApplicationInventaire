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
    /// The user Clicked on the button Search with Image
    /// </summary>
    public partial class PAGE_3_4 : Page, INotifyPropertyChanged
    {
        public PAGE_3_4()
        {
            InitializeComponent();
            DataContext = this;
            GlobalPages.page_3_4 = this;
            this.projectData = GlobalTemplateData.CurrentProjectData;
            this.IndiceSection = 0;
            foreach(var i in projectData.ImageSectionList) //Initializig Section Name
            {
                if (i.Name.Equals(projectData.mySections[IndiceSection].SectionName))
                {
                    this.ImageSection3 = i.Path;
                }
            }
            InitializeOverlay(); //initlizing Overlay. It is the little red circles put in sur impression over each referenced piece



        }

        #region privateVariables
        
        private TemplateData projectData;
        private List<(Image, Piece)> OverlayImageList = new List<(Image, Piece)>();
        private int IndiceSection;
        private int ImageMarkerWidth = 20; //used to define the size of he little red circles
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
      
        /// <summary>
        /// This function is used to "go back" a section. It update the section image displayed on the page as well  as the private variable IndiceSection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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


        /// <summary>
        /// This function is used to "go forward" a section. It update the section image displayed on the page as well  as the private variable IndiceSection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// This function is called whenever the user click on the canva. it detects if a user click on a red circle, and update the private varible CurrentPiece as well as 
        /// the position of the label, its content, and the text in text Block Name and description
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canva_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point clickPosition = e.GetPosition(myCanva);
            double x = clickPosition.X;
            double y = clickPosition.Y;
            (Image, Piece) res = GetClickedPieceImage(clickPosition);
            if (res != (null, null))
            {
                CurrentPiece = res.Item2;
                InitializeNameTagAndDesciption();
            }
        }

        #endregion

        #region privateMethods
        /// <summary>
        /// This function updates the content of textBlock name and description with CurrentPiece
        /// </summary>
        private void InitializeNameTagAndDesciption()
        {
            this.TextBlockDesciption.Text = CurrentPiece.Description;
            this.TextBlockName.Text = CurrentPiece.PieceName;
        }

        /// <summary>
        /// This function return the image corresponding to the instance of red circle image clicked on
        /// </summary>
        /// <param name="clickPosition"></param>
        /// <returns></returns>
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
      
        /// <summary>
        /// This function reset the overlay. Delete instances of red circle on the image. Used whan changing section image.
        /// </summary>
        private void ResetOverlay()
        {
            foreach (var i in OverlayImageList)
            {
                myCanva.Children.Remove(i.Item1);
            }
            OverlayImageList.Clear();
            this.TextBlockDesciption.Text = "";
            this.TextBlockName.Text = "";


        }

        /// <summary>
        /// This fonction creates an instance of the red circle image at coordinates given by the Piece objects given in argument.
        /// </summary>
        /// <param name="piece">Piece object used to set coordinates and Name</param>
        private void CreateImageInstance(Piece piece)
        {
            Image image = new Image();
            image.Source = new BitmapImage(new Uri(GlobalTemplateData.RedCirclePath, UriKind.Absolute));
            image.Width = ImageMarkerWidth;
            image.Height = ImageMarkerWidth;
            Canvas.SetLeft(image, piece.X- ImageMarkerWidth/2);
            Canvas.SetTop(image, piece.Y - ImageMarkerWidth / 2);
            myCanva.Children.Add(image);
            OverlayImageList.Add((image, piece));

        }

        
        
        #endregion
        
    }
}
