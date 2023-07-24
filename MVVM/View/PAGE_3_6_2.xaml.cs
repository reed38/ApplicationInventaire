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
using ApplicationInventaire.Core.GlobalProjectData;
using ApplicationInventaire.Core.PieceSections;
using ApplicationInventaire.Core.ProjectDataSet;

namespace ApplicationInventaire.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour PAGE_3_6_2.xaml
    /// The user was on  PAGE_3_6_1 and clicked on Save And Continu.
    /// Most of the functions used in ths page are similars to the one used before.
    /// </summary>
    public partial class PAGE_3_6_2 : Page,INotifyPropertyChanged
    {
        public PAGE_3_6_2()
        {
            InitializeComponent();
            GlobalPages.page_3_6_2 = this;
            DataContext = this;


            IndiceSection = 0;
            this.RedFramePath = GlobalProjectData.RedFramePath;
            projectData = GlobalProjectData.CurrentProjectData;

            foreach (ImageInfos im in this.projectData.ImageSectionList)
            {
                if (im.Name == projectData.mySections[IndiceSection].SectionName)
                {
                    ImageSection3 = im.Path;
                    break;

                }
            }
            this.RedFrameImage.Visibility = Visibility.Hidden;

            HideNameAndDescription();
            InitializeAutoSuggestionList();
            ResetFields();
            InitializeOverlay();

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
        ///  The user was on 3_6_1 and clicked on save and continu
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

        private ProjectData projectData;
        private Piece CurrentPiece;
        private List<(Image, Piece)> OverlayImageList = new List<(Image, Piece)>();//list containing infos about the instances of little red circles on the Image
        private (Image, Piece) tmp; //used to pass argument
        private int IndiceSection;
        private int ImageMarkerWidth = 20;//used to define the width of the little red circles

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

        #region GIMethods

        

        private void ChangeFrameCoordinates(double x, double y)
        {
            this.RedFrameImage.Visibility = Visibility.Visible;
            Canvas.SetLeft(this.RedFrameImage, x - RedFrameImage.Width / 2);
            Canvas.SetTop(this.RedFrameImage, y - RedFrameImage.Height / 2);
        }

        private void ChangeLabelcoordinates(double x, double y)
        {
            this.LabelNameTag.Visibility = Visibility.Visible;
            Canvas.SetLeft(this.LabelNameTag, x - 80);
            Canvas.SetTop(this.LabelNameTag, y - 80);

        }

        /// <summary>
        /// Used to change the coordinate of the Delete Selected button
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void ChangeButtonDeleteSelectedlcoordinates(double x, double y)
        {
            this.ButtonDeleteSelected.Visibility = Visibility.Visible;
            Canvas.SetLeft(this.ButtonDeleteSelected, x);
            Canvas.SetTop(this.ButtonDeleteSelected, y + 50);

        }

        /// <summary>
        /// Hide the elements appearing when clicking on an already existing Piece
        /// </summary>
        private void HideEditPieceOverlay()
        {
            LabelNameTag.Visibility = Visibility.Hidden;
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
            image.Source = new BitmapImage(new Uri(GlobalProjectData.RedCirclePath, UriKind.Absolute));
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

        #endregion

        #region UImethods

        /// <summary>
        /// The user clicked on the button Save after having created a new Piece and entered its name. The function add the Piece to the data structure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            SavePiece();
        }

        /// <summary>
        /// Used to save the Piece when the user press the "enter"key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxNameTagKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SavePiece();

            }

        }


        /// <summary>
        /// This function is called whenever the user click on the canva. 
        /// The function will check if the user clicked on an already existing Piece. 
        /// If yes it will show the piece name tag and description and propose to delete it.
        /// If no a text box with a "Save" button will appear for the user to create a new Piece.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            if (res != (null, null))
            {
                BorderAddPiece.Visibility = Visibility.Collapsed;
                CurrentPiece = res.Item2;
                LabelNameTag.Content = res.Item2.PieceName;
                ChangeLabelcoordinates(x, y);
                ChangeButtonDeleteSelectedlcoordinates(x, y);
                InitializeNameAndDesciption();
                tmp = res;


            }
            else
            {
                BorderAddPiece.Visibility = Visibility.Visible;


            }

            CurrentPiece = new Piece();
            CurrentPiece.X = x;
            CurrentPiece.Y = y;
            ChangeFrameCoordinates(x, y);
            Keyboard.Focus(autoTextBox);


        }

        /// <summary>
        /// The User has finished working with current section and clicked on the Button Next.
        /// This will cycle forward of 1 throught Sections, and update the Section Image as well as the overlay.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClickSaveGoToNext(object sender, RoutedEventArgs e)
        {
            IndiceSection = (IndiceSection + 1) % (projectData.mySections.Count );
            ChangeSection();
         

        }
        /// <summary>
        /// The User has finished working with current section and clicked on the Button Previous.
        /// This will cycle forwbackward of 1 throught Sections, and update the Section Image as well as the overlay.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClickSaveGoPrevious(object sender, RoutedEventArgs e)
        {
            IndiceSection--;
            if (IndiceSection<0)
            {
                IndiceSection = projectData.mySections.Count-1 ;
            }
            ChangeSection();
        }


        /// <summary>
        /// The user click on the Button Delete.
        /// This button appears when the user click on an already existing Piece.
        /// This will remove the Piece from the data structure and update the overlay accordingly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClickDeleteSelected(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < OverlayImageList.Count; i++)
            {
                if (OverlayImageList[i].Item2 == tmp.Item2)
                {
                    ResetOverlay();
                    projectData.mySections[IndiceSection].PiecesList.Remove(tmp.Item2);
                    break;
                }
            }
            HideEditPieceOverlay();
            this.RedFrameImage.Visibility = Visibility.Hidden;
            InitializeOverlay();

        }

        #endregion

        #region privateMethods

        /// <summary>
        /// This function is called when the user clicked on Next or Previous. 
        /// It update the overlay ands hide currently  displaying informations.
        /// It also change the Section Image
        /// </summary>
        private void ChangeSection()
        {


            HideNameAndDescription();
            HideEditPieceOverlay();
           
           

            foreach (ImageInfos i in projectData.ImageSectionList)
            {
                if (i.Name.Equals( projectData.mySections[IndiceSection].SectionName))
                {
                    ImageSection3 = i.Path;
                    break;
           
                }
            }
            InitializeOverlay();


        }


        /// <summary>
        /// This function save the CurrentPiece to the data structure and update the overla accordingly.
        /// </summary>
        private void SavePiece()
        {
            if (string.IsNullOrEmpty(autoTextBox.Text))
            {
                POPUP.ShowPopup("Please enter Name Tag");
                return;
            }
            CurrentPiece.PieceName = autoTextBox.Text;
            CurrentPiece.SectionId = IndiceSection + 1;
            projectData.mySections[IndiceSection].PiecesList.Add(CurrentPiece);

            InitializeOverlay();
            this.RedFrameImage.Visibility = Visibility.Hidden;
            ResetFields();
        }

      
        private void ClearNameAndDescription()
        {
            this.TextBlockDesciption.Text = "";
            this.TextBlockName.Text = "";

        }
        private void HideNameAndDescription()
        {
            this.StackPanelDescription.Visibility = Visibility.Collapsed;
            this.RedFrameImage.Visibility = Visibility.Hidden;

        }
        private void ShowNameAndDescription()
        {
            this.StackPanelDescription.Visibility = Visibility.Visible;
            this.RedFrameImage.Visibility=Visibility.Visible;

        }
        private void InitializeNameAndDesciption()
        {
            ShowNameAndDescription();
            this.TextBlockDesciption.Text = CurrentPiece.Description;
            this.TextBlockName.Text = CurrentPiece.PieceName;
        }



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

        /// <summary>
        /// This function is called when the user click on the button save and quit.
        /// It saves the current data structure in the Database.db file and return to the main menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSaveAndQuit(object sender, RoutedEventArgs e)
        {
            projectData.Save();
            GlobalPages.PageGoBack();
            GlobalPages.PageGoBack();
            GlobalPages.PageGoBack();
        }
    }
}
