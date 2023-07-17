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
using NPOI.OpenXmlFormats.Dml.Diagram;
using Section = ApplicationInventaire.Core.PieceSections.Section;

namespace ApplicationInventaire.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour PAGE_5_2.xaml
    /// </summary>
    public partial class PAGE_5_2 : Page, INotifyPropertyChanged
    {
        public PAGE_5_2()
        {

            InitializeComponent();
            GlobalPages.page_5_2 = this;
            this.RedFramePath = GlobalProjectData.RedFramePath;

            this.projectData = new(new ProjectInfos(GlobalProjectData.CurrentProjectName));
            DataContext = this;

            this.InitializeCurrentPiece();

            SetBorderPosition();
        }
        #region Variables
        private ProjectData projectData;
        private Piece CurrentPiece;
        #endregion
        #region bindingVariables
        private string redFramePath;

        private string imageSection2;
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

        public string ImageSection2
        {
            get { return imageSection2; }
            set
            {
                imageSection2 = value;
                OnPropertyChanged(nameof(ImageSection2));
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region privateMethod
        private void InitializeCurrentPiece()
        {

            foreach (Section i in this.projectData.mySections)
            {
                foreach (Piece j in i.PiecesList)
                {
                    if (GlobalProjectData.CurrentPieceName.Equals(j.PieceName))
                    {
                        foreach (ImageInfos k in this.projectData.ImageSectionList)
                        {
                            if (k.Name.Equals(i.SectionName))
                            {
                                this.ImageSection2 = k.Path;
                                break;
                            }
                        }
                        this.CurrentPiece = j;
                        return;

                    }

                }
            }
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
       
        #endregion
    }





}

