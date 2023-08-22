using ApplicationInventaire.Core.GlobalPages;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

using ApplicationInventaire.Core.DatabaseManagement;
using ApplicationInventaire.Core.ExcelManagement;
using ApplicationInventaire.Core.PieceSections;
using ApplicationInventaire.MVVM.View;
using ApplicationInventaire.Core.GlobalProjectData;
using Microsoft.Win32;
using ApplicationInventaire.Core.ProjectDataSet;

namespace ApplicationInventaire.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour PAGE_3_2.xaml
    /// </summary>
    public partial class PAGE_3_2 : Page, INotifyPropertyChanged
    {

        public PAGE_3_2()
        {
            InitializeComponent();
            GlobalPages.page_3_2 = this;
            DataContext = this;
            projectData = GlobalProjectData.CurrentProjectData; //Loading project Data from classic class
            this.RedFramePath = GlobalProjectData.RedFramePath; //the image used in multiples pages have their path stored in the static class




            ResetTextBox(); //reset Comment, Serial Number, and Constructot textBox
            HideTextBoxSerialNumberConstructor(); //Hide it by default since we don't know if upcomming piece require it or not
            RedFrameImage.Visibility = Visibility.Visible; //hide red frame
            IndiceSection = 0;
            IndicePiece = 0;
            ClearNameTagAndDescription();
            FindNextNoPresent(); //this will update the Indice Section and IndicePiece with the ones corresponding to the first Piece with the field "IsPresent=0"
            InitializeNameTagAndDesciption();
            InitializeMarquageButton();

            IndiceSection = CurrentPiece.SectionId - 1; //The library used to store data in a sqlite database have an Indice starting at 1

            foreach (ImageInfos im in this.projectData.ImageSectionList) //initializing the current Section Image on the path
            {
                if (im.Name == projectData.mySections[IndiceSection].SectionName)
                {
                    ImageSection = im.Path;
                    break;

                }
            }
            SetBorderPosition(); //initializing the position of the red frame  and the label containing NameTag

        }
        #region Variables
        private int IndiceSection { set; get; }
        private int IndicePiece { set; get; }
        private ProjectData projectData { set; get; }
        #endregion

        #region bindingVariablesSources 
        private string imageSection;
        private string redFramePath;
        private string imageReleve;
        private double xCoordinatevar;
        private double yCoordinatevar;
        private Piece currentPiece;


        private Section currentSection;
        
        #endregion





        #region bindingMethods

        public Section CurrentSection
        {
            get { return currentSection; }
            set
            {
                currentSection = value;
                OnPropertyChanged(nameof(CurrentSection));
            }
        }

        public Piece CurrentPiece
        {
            get { return currentPiece; }
            set
            {
                currentPiece = value;
                OnPropertyChanged(nameof(CurrentPiece));
            }
        }

        public string ImageReleve
        {
            get { return imageReleve; }
            set
            {
                imageReleve = value;
                OnPropertyChanged(nameof(ImageReleve));
            }
        }

        public string RedFramePath
        {
            get { return redFramePath; }
            set
            {
                redFramePath = value;
                OnPropertyChanged(nameof(RedFramePath));
            }
        }
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

        #region GIMethods 

        /// <summary>
        /// This function changes the postion of the frame and the label  in function of CurrentPiece.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// 
        private void SetBorderPosition()
        {
            ChangeFrameCoordinates(CurrentPiece.X - this.RedFrameImage.Height / 2, CurrentPiece.Y - this.RedFrameImage.Width / 2);


        }
        private void ChangeFrameCoordinates(double x, double y)
        {
            this.RedFrameImage.Visibility = Visibility.Visible;
            Canvas.SetLeft(this.RedFrameImage, x);
            Canvas.SetTop(this.RedFrameImage, y);
        }



        private void HideTextBoxSerialNumberConstructor()
        {
            StackPanelSerialDescription.Visibility = Visibility.Hidden;
            //TextBoxSerialNumber.Visibility = Visibility.Hidden;
            //TextBoxConstructor.Visibility = Visibility.Hidden;
            //labelConstructor.Visibility = Visibility.Hidden;
            //labelSerialNumber.Visibility = Visibility.Hidden;
        }

        private void ShowTextBoxSerialNumberConstructor()
        {
            StackPanelSerialDescription.Visibility = Visibility.Visible;

        }


        private void ResetTextBox()
        {
            TextBoxComment.Clear();
            TextBoxConstructor.Clear();
            TextBoxSerialNumber.Clear();

        }

        private void HideFrame()

        {
            this.RedFrameImage.Visibility = Visibility.Hidden;
        }

        #endregion

        #region PrivateMethods

        private void InitializeMarquageButton()
        {
            if (currentPiece.HasMarking == 1)
            {
                this.ButtonsMarquageStack.Visibility = Visibility.Visible;
                this.RadioButtonMarquageNonPresent.IsChecked = true;
            }

            else
            {
                this.ButtonsMarquageStack.Visibility = Visibility.Collapsed;

            }

        }

        private void InitializeNameTagAndDesciption()
        {
            this.TextBlockDesciption.Text = CurrentPiece.Description;
            this.TextBlockName.Text = CurrentPiece.PieceName;
        }
        private void ClearNameTagAndDescription()
        {
            this.TextBlockDesciption.Text = string.Empty;
            this.TextBlockName.Text = string.Empty;

        }

        /// <summary>
        /// Update the value of currentPiece in function on the value of IndiceSection and IndicePiece
        /// </summary>
        private void UpdateCurrent()
        {
            this.CurrentSection = projectData.mySections[IndiceSection];
            this.CurrentPiece = projectData.mySections[IndiceSection].PiecesList[IndicePiece];

        }

        /// <summary>
        /// this function is used when the targeted Piece change
        /// </summary>
        /// <param name="answ"> the value written in the cell of the excel file. "1" or "0" </param>
        private void GotoNextPiece()
        {

            FindNextNoPresent(); //Find the next Piece with the field "IsPresent" set to 0, and Update Section image, IndiceSection , IndicePiece accordingly
            InitializeNameTagAndDesciption();
            InitializeMarquageButton();

            SetBorderPosition();

        }

        /// <summary>
        /// Thuis function is used to find the next Piece with the fiels "IsPresent" set to . It Updates IndiceSection, IndicePiece, Image section and ImageReleve by doing so.
        /// </summary>
        private void FindNextNoPresent()
        {
            ImageReleve = null;
            ImageReleveName.Source = null;

            if (IndiceSection == projectData.mySections.Count - 1 && IndicePiece == projectData.mySections[IndiceSection].PiecesList.Count)//if the user wen through everything
            {
                SaveAndQuit();
                return;

            }

            else if (IndicePiece == projectData.mySections[IndiceSection].PiecesList.Count) //if the user went throuht all the Pieces in the current Section
            {
                IndiceSection++;
                IndicePiece = 0;
                foreach (ImageInfos im in this.projectData.ImageSectionList)
                {
                    if (im.Name == projectData.mySections[IndiceSection].SectionName)
                    {
                        ImageSection = im.Path; //updating Section image on te page
                        break;

                    }

                }

            }


            for (int section = IndiceSection; section < projectData.mySections.Count; section++)
            {

                for (int piece = IndicePiece; piece < projectData.mySections[section].PiecesList.Count; piece++)
                {

                    if (projectData.mySections[section].PiecesList[piece].IsPresent == 0)
                    {
                        this.IndicePiece = piece;
                        this.IndiceSection = section;
                        UpdateCurrent();
                        if (this.CurrentPiece.IsReleveRequired == 1)
                        {
                            ShowTextBoxSerialNumberConstructor();
                            foreach (ImageInfos i in projectData.ImageReleveList)
                            {
                                if (i.Name.IndexOf(this.CurrentPiece.PieceName) >= 0)
                                {
                                    ImageReleve = i.Path;
                                    break;
                                }

                            }
                        }
                        IndicePiece++;
                        return;
                    }



                }


            }
        }





        /// <summary>
        /// Called when the User reached the end of the inventory or when he chosses to save and quit. 
        /// This function shows a pop up asking to select a location where to save the excelfile, and then save it to that location, 
        /// </summary>
        private void SaveAndQuit()
        {
            projectData.myTmpExcelFile.UpdateExcelFormula();


            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // Set the default file name and extension
            saveFileDialog.DefaultExt = ".xls";

            // Set the file filters
            saveFileDialog.Filter = "excel file (*.xls )|*.xls|excel file (*.xlsx)|*.xlsx";  // Set allowed file extensions

            saveFileDialog.FileName = projectData.myProjectInfos.ProjectName; // Set the default filename here
            // Show the save file dialog
            bool? result = saveFileDialog.ShowDialog();

            // Process the selected file
            if (result == true)
            {
                string filePath = saveFileDialog.FileName;
                try
                {
                    File.Copy(projectData.myProjectInfos.TmpExcelPath, filePath, true);
                }
                catch (Exception e)
                {
                    POPUP.ShowPopup("erreur, ce fichier est éjà ouvert par un autre processus");
                    SaveAndQuit();
                }

            }

            GlobalPages.SetCurrentPageBack(GlobalPages.PAGE_3_1);
        }
        /// <summary>
        /// This function is used to write in the excel file int the comment column at the corresponding row  the string contained in the Comment textBox.
        /// </summary>
        private void UpdateComment()
        {
            if (!TextBoxComment.Text.Equals(string.Empty))
            {
                CellInfo tmp = projectData.myTmpExcelFile.FindValue("Commentaire");
                projectData.myTmpExcelFile.SetCellValue(tmp.Sheet, TextBoxComment.Text, tmp.Column, CurrentPiece.ExcelRow);
                TextBoxComment.Clear();
            }



        }
        /// <summary>
        /// This function is used to write in the excel file the serial number at the corresponding row and column.
        /// </summary>
        private void UpdateSerialNumber()
        {
            if (!TextBoxSerialNumber.Text.Equals(string.Empty))
            {
                CellInfo tmp = projectData.myTmpExcelFile.FindValue("N° SERIE");
                projectData.myTmpExcelFile.SetCellValue(tmp.Sheet, TextBoxSerialNumber.Text, tmp.Column, CurrentPiece.ExcelRow);
                TextBoxSerialNumber.Clear();
            }


        }
        /// <summary>
        /// This function writes the constructor name in the excel file at the corresponding column and row.
        /// </summary>
        private void UpdateConstructor()
        {
            if (!TextBoxConstructor.Text.Equals(string.Empty))
            {
                CellInfo tmp = projectData.myTmpExcelFile.FindValue("FABRICANT");
                projectData.myTmpExcelFile.SetCellValue(tmp.Sheet, TextBoxConstructor.Text, tmp.Column, CurrentPiece.ExcelRow);
                TextBoxConstructor.Clear();
            }


        }

        /// <summary>
        /// This function check if the Current Piece have a  marquage. If it does, the fonction will then check the value of the marquage radio button and update the excel.
        /// </summary>
        private void UpdateMarquage()
        {
            if (CurrentPiece.HasMarking == 1)
            {
               
                if((this.RadioButtonMarquagePresent.IsChecked==true))
                {
                    CellInfo tmp = this.projectData.myTmpExcelFile.FindValue(CurrentPiece.PieceName + ".M");
                    this.projectData.myTmpExcelFile.SetCellValue(tmp.Sheet, "1",CurrentPiece.ExcelColumn,tmp.Row);


                }

            }
        }






            #endregion

            #region UIMethods

            /// <summary>
            /// The user clicked on the button Save and Quit. 
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
             private void ButtonClickSaveAndQuit(object sender, RoutedEventArgs e)
            {
                UpdateSerialNumber();
                UpdateConstructor();
                UpdateComment();

                projectData.myTmpExcelFile.UpdateSignature();
                SaveAndQuit();
            }

            /// <summary>
            /// The used clicked on the button No. It resets the textBox, Hide seral number/constructor textboxes, hide frame and label, and write 0 int the excel file in the colum Present
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private  void ButtonClickNo(object sender, RoutedEventArgs e)
            {
                ResetTextBox();
                HideTextBoxSerialNumberConstructor();
                HideFrame();
                projectData.myTmpExcelFile.SetCellValue(currentPiece.SheetName, "0", currentPiece.ExcelColumn, currentPiece.ExcelRow);

                GotoNextPiece();

            }
            /// <summary>
            /// The used clicked on the button No. It resets the textBox, Hide seral number/constructor textboxes, hide frame and label, and write 1 in the excel file in the colum Present
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void ButtonClickYes(object sender, RoutedEventArgs e)
            {
                if (CurrentPiece.IsReleveRequired == 1 && ((TextBoxSerialNumber.Text.Equals(string.Empty)) || TextBoxConstructor.Text.Equals(string.Empty)))
                {
                    POPUP.ShowPopup("Veuillez saisir un numéro de série et un constructeur");
                    return;

                }
                UpdateComment();
                 UpdateMarquage();
                UpdateSerialNumber();
                UpdateConstructor();
                ResetTextBox();
                HideTextBoxSerialNumberConstructor();
                HideFrame();
                projectData.myTmpExcelFile.SetCellValue(currentPiece.SheetName, "1", currentPiece.ExcelColumn, currentPiece.ExcelRow);

                GotoNextPiece();


            }

            /// <summary>
            /// This method is called when the user click on the radio button with label "marquage present". It set the Piece as  having its marquage. No all the pieces require a marquage. Only when there is another piece whose tag is "PieceName.M".
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>



            /// <summary>
            /// This method is called when the user click on the radio button with label "marquage non  present". It set the Piece as not having its marquage. Only when there is another piece whose tag is "PieceName.M".
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>



            #endregion

        }
    }


