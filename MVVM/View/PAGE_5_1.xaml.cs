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
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ApplicationInventaire.Core.GlobalPages;
using System.ComponentModel;

using ApplicationInventaire.Core.ProjectDataSet;
using ApplicationInventaire.Core.GlobalProjectData;
using ApplicationInventaire.Core.PieceSections;
using Section = ApplicationInventaire.Core.PieceSections.Section;


namespace ApplicationInventaire.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour PAGE_5_1.xaml
    /// The user clicked on the button Search with Tags
    /// </summary>
    public partial class PAGE_5_1 : Page
    {
        public PAGE_5_1()
        {
            InitializeComponent();
            GlobalPages.page_5_1 = this;
            DataContext = this;
            projectData = GlobalTemplateData.CurrentTemplateData;
            InitializeListPieceName();
            AutoSuggestionList = listPieceName;
        }

        #region PrivateVariables
        private TemplateData projectData;
        private List<string> listPieceName = new List<string>();
        #endregion
     
        #region Private properties.  

        /// <summary>  
        /// Auto suggestion list property.  
        /// </summary>  
        private List<string> autoSuggestionList = new List<string>(); //used to contains the names of the Piece and shw suggestions

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

        #region privateMethods

        private void InitializeListPieceName()
        {
            foreach(Section i in projectData.mySections)
            {
                foreach(Piece j in i.PiecesList)
                {
                    listPieceName.Add(j.PieceName);
                }

            }
        }


        #endregion
       
        #region UIMethods

        /// <summary>
        /// The user clicked on the button Enter after having entered a NameTag.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClickSeachEnter(object sender, RoutedEventArgs e)
        {
            if(!String.IsNullOrEmpty(autoTextBox.Text))
            {
             GlobalTemplateData.CurrentPieceName = autoTextBox.Text;
             autoTextBox.Clear();
              GlobalPages.SetCurrentPage(GlobalPages.PAGE_5_2);
            }
           

        }
        #endregion
    }
}

