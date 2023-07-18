using System;
using System.Collections.Generic;
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
using ApplicationInventaire.Core.PieceSections;
using ApplicationInventaire.Core.ProjectDataSet;
using Path = System.IO.Path;

namespace ApplicationInventaire.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour PAGE_4_2.xaml
    /// </summary>
    public partial class PAGE_4_2 : Page,INotifyPropertyChanged
    {
        public PAGE_4_2()
        {
            InitializeComponent();
                        DataContext = this;

            GlobalPages.page_4_2 = this;
            IndiceSection = 0;
            this.RedFramePath = GlobalProjectData.RedFramePath;
            projectData = new ProjectData(new ProjectInfos(GlobalProjectData.CurrentProjectName));
            ImageSection3 = projectData.ImageSectionList[0].Path;
            ResetPopup();



        }

        #region Variables
        private ProjectData projectData;
        private Piece CurrentPiece;
        private string relevePath;
        private List<(Image, Piece)> OverlayImageList = new List<(Image, Piece)>();
        private int IndiceSection;
        private int ImageMarkerWidth = 20;
        #endregion
        #region bindingVariables
        private string redFramePath;

        private string imageSection3;
        #endregion

        #region BindingMethods

        public string RedFramePath
        {
            get { return redFramePath; }
            set
            {
                redFramePath = value;
                OnPropertyChanged(nameof(RedFramePath));
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

        #region privateMethod


        #region GIMethods
        private void ShowPopup(string message)
        {
            MessageBox.Show(message, "Field Missing", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void SetBorderPosition()
        {
            ChangeFrameCoordinates(CurrentPiece.X - this.RedFrameImage.Height / 2, CurrentPiece.Y - this.RedFrameImage.Width / 2);


        }
      
        private void ChangeFrameCoordinates(double x, double y)
        {
            Canvas.SetLeft(this.RedFrameImage, x);
            Canvas.SetTop(this.RedFrameImage, y);
        }

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

        }

        private void CreateImageInstance(Piece piece)
        {
            Image image = new Image();
            image.Source = new BitmapImage(new Uri(GlobalProjectData.RedCirclePath, UriKind.Absolute));
            image.Width = ImageMarkerWidth;
            image.Height = ImageMarkerWidth;
            Canvas.SetLeft(image, piece.X);
            Canvas.SetTop(image, piece.Y);
            myCanva.Children.Add(image);
            OverlayImageList.Add((image, piece));

        }

        private void ResetPopup()
        {
            TextBoxNameTag.Clear();
            LabelSelectedimage.Visibility = Visibility.Collapsed;
            LabelSelectedimagePath.Visibility= Visibility.Collapsed;
            ReleveRequiredNo.IsChecked = true;
        }
        #endregion




        #endregion


        #region UImethods
      
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(TextBoxNameTag.Text))
            {
                ShowPopup("Please enter Name Tag");
                return;
            }
            CurrentPiece.PieceName = TextBoxNameTag.Text;
            CurrentPiece.SectionId = IndiceSection + 1;

            projectData.mySections[IndiceSection].PiecesList.Add(CurrentPiece);
            if(CurrentPiece.IsReleveRequired==1)
            {
                string destPath = Path.Combine(projectData.myProjectInfos.ImageRelevePath, CurrentPiece.PieceName);
                File.Copy(relevePath, destPath);

            }
            InitializeOverlay();


            this.popup.IsOpen = false;
        }



        private void Canva_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ResetPopup();
            Point clickPosition = e.GetPosition(myCanva);
            double x = clickPosition.X - ImageMarkerWidth / 2;
            double y = clickPosition.Y - ImageMarkerWidth / 2;
            popup.IsOpen = true;
            CurrentPiece = new Piece();
            CurrentPiece.X = x;
            CurrentPiece.Y = y;







        }

        private void ButtonClickSaveGoToNext(object sender, RoutedEventArgs e)
        {
            IndiceSection++;
            if(IndiceSection==projectData.mySections.Count-1)
            {
                ButtonsaveGoNext.Content = "Save and exit";
            }

            else if (IndiceSection == projectData.mySections.Count )
            {
                projectData.Save();
                GlobalPages.SetCurrentPageBack();
                GlobalPages.SetCurrentPageBack();
                return;
            }
           
            foreach(ImageInfos i in  projectData.ImageSectionList) 
            {
                if (i.Name == projectData.mySections[IndiceSection].SectionName)
                {
                    ImageSection3 = i.Path;
                }
            }
            InitializeOverlay();


        }

        #endregion

        #region privateMethods



        private void ButtonClickReleveNo(object sender, RoutedEventArgs e)
        {
            CurrentPiece.IsReleveRequired = 0;

        }

        private void ButtonClickReleveYes(object sender, RoutedEventArgs e)
        {
            CurrentPiece.IsReleveRequired = 1;
             relevePath = FileManager.OpenImagePopupSingle();
            LabelSelectedimage.Visibility = Visibility.Visible;
            LabelSelectedimagePath.Visibility = Visibility.Visible;
            LabelSelectedimagePath.Content=Path.GetFileNameWithoutExtension(relevePath);



        }

        #endregion


    }


}
