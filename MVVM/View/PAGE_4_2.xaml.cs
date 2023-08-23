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
using Section = ApplicationInventaire.Core.PieceSections.Section;

namespace ApplicationInventaire.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour PAGE_4_2.xaml
    /// The user was on PAGE_4_1 and clicked on Save and Continu
    /// </summary>
    public partial class PAGE_4_2 : Page, INotifyPropertyChanged
    {
        public PAGE_4_2()
        {
            InitializeComponent();
            DataContext = this;
            GlobalPages.page_4_2 = this;

            IndiceSection = 0;
            this.RedFramePath = GlobalTemplateData.RedFramePath;
            projectData = GlobalTemplateData.CurrentProjectData;

            foreach (ImageInfos im in this.projectData.ImageSectionList)
            {
                if (im.Name == projectData.mySections[IndiceSection].SectionName)
                {
                    ImageSection3 = im.Path;
                    break;

                }
            }
            InitializeAutoSuggestionList();
            ResetFields();
            HideNameAndDescription();

            this.RedFrameImage.Visibility = Visibility.Hidden;

        }
        #region autoTextBox

        #region Private properties.  

        /// <summary>  
        /// Auto suggestion list property.  
        /// </summary>  
        private List<string> autoSuggestionList = new List<string>();

        #endregion

        #region Protected / Public properties.  

        /// <summary>  
        /// Gets or sets Auto suggestion list property.  
        /// </summary>  
        public List<string> AutoSuggestionList
        {
            get { return this.autoSuggestionList; }
            set { this.autoSuggestionList = value; }
        }

        #endregion

        #region Open Auto Suggestion box method  

        /// <summary>  
        ///  Open Auto Suggestion box method  
        /// </summary>  
        private void OpenAutoSuggestionBox()
        {
            try
            {
                // Enable.  
                this.autoListPopup.Visibility = Visibility.Visible;
                this.autoListPopup.IsOpen = true;
                this.autoList.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                // Info.  
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.Write(ex);
            }
        }

        #endregion

        #region Close Auto Suggestion box method  

        /// <summary>  
        ///  Close Auto Suggestion box method  
        /// </summary>  
        private void CloseAutoSuggestionBox()
        {
            try
            {
                // Enable.  
                this.autoListPopup.Visibility = Visibility.Collapsed;
                this.autoListPopup.IsOpen = false;
                this.autoList.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                // Info.  
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.Write(ex);
            }
        }

        #endregion

        #region Auto Text Box text changed the method  

        /// <summary>  
        ///  Auto Text Box text changed method.  
        /// </summary>  
        /// <param name="sender">Sender parameter</param>  
        /// <param name="e">Event parameter</param>  
        private void AutoTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                // Verification.  
                if (string.IsNullOrEmpty(this.autoTextBox.Text))
                {
                    // Disable.  
                    this.CloseAutoSuggestionBox();

                    // Info.  
                    return;
                }

                // Enable.  
                this.OpenAutoSuggestionBox();

                // Settings.  
                this.autoList.ItemsSource = this.AutoSuggestionList.Where(p => p.ToLower().Contains(this.autoTextBox.Text.ToLower())).ToList();
            }
            catch (Exception ex)
            {
                // Info.  
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.Write(ex);
            }
        }

        #endregion

        #region Auto list selection changed method  

        /// <summary>  
        ///  Auto list selection changed method.  
        /// </summary>  
        /// <param name="sender">Sender parameter</param>  
        /// <param name="e">Event parameter</param>  
        private void AutoList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // Verification.  
                if (this.autoList.SelectedIndex <= -1)
                {
                    // Disable.  
                    this.CloseAutoSuggestionBox();

                    // Info.  
                    return;
                }

                // Disable.  
                this.CloseAutoSuggestionBox();

                // Settings.  
                this.autoTextBox.Text = this.autoList.SelectedItem.ToString();
                this.autoList.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                // Info.  
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.Write(ex);
            }
        }

        #endregion

        #endregion

        #region PrivateVariables

        private TemplateData projectData;
        private Piece CurrentPiece; 
        private List<(Image, Piece)> OverlayImageList = new List<(Image, Piece)>();
        private (Image, Piece) tmp; //used to pass argument
        private int IndiceSection;
        private const int ImageMarkerWidth = 20;

        #endregion

        #region bindingVariables

        private string redFramePath;
        private string imageSection3;

        #endregion

        #region privateMethods

        private void InitializeAutoSuggestionList()
        {
            AutoSuggestionList = projectData.GetPieceNames();
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

        #region UIMethods

        private void ButtonClickSeachEnter(object sender, RoutedEventArgs e)
        {
            GlobalTemplateData.CurrentPieceName = autoTextBox.Text;
            autoTextBox.Clear();
            GlobalPages.SetCurrentPage(GlobalPages.PAGE_5_2);

        }
       
        
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

        #region GIMethods

        private void SetBorderPosition()
        {
            ChangeFrameCoordinates(CurrentPiece.X - this.RedFrameImage.Height / 2, CurrentPiece.Y - this.RedFrameImage.Width / 2);


        }

        private void ChangeFrameCoordinates(double x, double y)
        {
            this.RedFrameImage.Visibility = Visibility.Visible;
            Canvas.SetLeft(this.RedFrameImage, x - RedFrameImage.Width / 2);
            Canvas.SetTop(this.RedFrameImage, y - RedFrameImage.Height / 2);
        }

       

        private void ChangeButtonDeleteSelectedlcoordinates(double x, double y)
        {
            this.ButtonDeleteSelected.Visibility = Visibility.Visible;
            Canvas.SetLeft(this.ButtonDeleteSelected,x);
            Canvas.SetTop(this.ButtonDeleteSelected, y + 50);

        }

        private void HideEditPieceOverlay()
        {
            ButtonDeleteSelected.Visibility = Visibility.Collapsed;

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
            image.Source = new BitmapImage(new Uri(GlobalTemplateData.RedCirclePath, UriKind.Absolute));
            image.Width = ImageMarkerWidth;
            image.Height = ImageMarkerWidth;
            Canvas.SetLeft(image, piece.X - ImageMarkerWidth / 2);
            Canvas.SetTop(image, piece.Y - ImageMarkerWidth / 2);
            myCanva.Children.Add(image);
            OverlayImageList.Add((image, piece));

        }

        private void ResetFields()
        {
            autoTextBox.Clear();
        }
      
        private void ClearNameAndDescription()
        {
            this.TextBlockDesciption.Text = "";
            this.TextBlockName.Text = "";

        }
        private void HideNameAndDescription()
        {
            this.StackPanelDescription.Visibility = Visibility.Collapsed;

        }
        private void ShowNameAndDescription()
        {
            this.StackPanelDescription.Visibility = Visibility.Visible;

        }
        private void InitializeNameAndDesciption()
        {
            ShowNameAndDescription();
            this.TextBlockDesciption.Text = CurrentPiece.Description;
            this.TextBlockName.Text = CurrentPiece.PieceName;
        }


        #endregion

        #region UImethods

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            SavePiece();
        }
      
        private void TextBoxNameTagKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SavePiece();

            }

        }
       
        private void Canva_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ResetFields();
            HideEditPieceOverlay();
            ClearNameAndDescription();
            HideNameAndDescription();

            Point clickPosition = e.GetPosition(myCanva);
            double x = clickPosition.X;
            double y = clickPosition.Y;
            (Image, Piece) res = GetClickedPieceImage(clickPosition);
         
            if(res!=(null,null))
            {
                CurrentPiece = res.Item2;
                ChangeButtonDeleteSelectedlcoordinates(x, y);
                InitializeNameAndDesciption();
                tmp = res;


            }

            CurrentPiece = new Piece();
            CurrentPiece.X = x;
            CurrentPiece.Y = y;
            ChangeFrameCoordinates(x, y);
            Keyboard.Focus(autoTextBox);


        }

        private void ButtonClickSaveGoToNext(object sender, RoutedEventArgs e)
        {

            IndiceSection++;
            if (IndiceSection == projectData.mySections.Count - 1)
            {
                ButtonsaveGoNext.Content = "Save and exit";
            }

            else if (IndiceSection == projectData.mySections.Count)
            {
                projectData.Save();
                GlobalPages.PageGoBack();
                GlobalPages.PageGoBack();
                return;
            }

            foreach (ImageInfos i in projectData.ImageSectionList)
            {
                if (i.Name == projectData.mySections[IndiceSection].SectionName)
                {
                    ImageSection3 = i.Path;
                }
            }
            InitializeOverlay();
            HideNameAndDescription();
            HideEditPieceOverlay();
            RedFrameImage.Visibility = Visibility.Hidden;

        }
      
     

        private void ButtonClickDeleteSelected(object sender, RoutedEventArgs e)
        {

            for  (int  i=0;i< OverlayImageList.Count;i++)
            {
                if (OverlayImageList[i].Item2==tmp.Item2)
                {
                    ResetOverlay();
                    projectData.mySections[IndiceSection].PiecesList.Remove(tmp.Item2);
                    break;
                }
            }
            HideEditPieceOverlay();
            HideNameAndDescription();
            this.RedFrameImage.Visibility = Visibility.Hidden;
            InitializeOverlay();

        }

        #endregion

        #region privateMethods
        private void SavePiece()
        {
            if (string.IsNullOrEmpty(autoTextBox.Text) || CurrentPiece == null)
            {
                POPUP.ShowPopup("Vous devez d'abord cliquer sur une pièce et saisir un nameTag pour effectuer cette action");
                return;
            }
            CurrentPiece.PieceName = autoTextBox.Text;
            CurrentPiece.SectionId = IndiceSection + 1;
            projectData.mySections[IndiceSection].PiecesList.Add(CurrentPiece);
           
            InitializeOverlay();
            this.RedFrameImage.Visibility = Visibility.Hidden;
            ResetFields();
            CurrentPiece = null;
        }




        #endregion

      
    }


}
