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
using ApplicationInventaire.Core.GlobalPages;
using ApplicationInventaire.Core.GlobalProjectData;
using System.Windows.Forms;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace ApplicationInventaire.MVVM.View
{
    /// <summary>
    /// toto
    /// Logique d'interaction pour PAGE_0.xaml
    /// </summary>
    public partial class PAGE_0 : Page
    {
        public PAGE_0()
        {
            GlobalPages.page_0 = this;
          
            InitializeComponent();
            DataContext = this;
            if (GlobalProjectData.UserRigth == 1)
            {
                RadioButtonAdmin.IsChecked = true;
                RadioButtonDefault.IsChecked = false;
            }
            else
            {
                RadioButtonAdmin.IsChecked = false;
                RadioButtonDefault.IsChecked = true;

            }
        }
        #region privateVariable
        private string passwd;
        #endregion

        #region UIMethods
        private void ButtonRadioAdminClick(object sender, RoutedEventArgs e)
        {
            PopUpPassword.IsOpen = true;
            Keyboard.Focus(PasswordBoxUserRight);


        }

        private void ButtonRadioDefaultClick(object sender, RoutedEventArgs e)
        {
            GlobalProjectData.UserRigth = 0;
          

        }


        private void ButtonCancelClick(object sender, RoutedEventArgs e)
        {
            passwd = null;
            PopUpPassword.IsOpen = false;

        }

        private void TextBoxNameTagKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (!string.IsNullOrEmpty(passwd))
                {
                    if (passwd.Equals(GlobalProjectData.password))
                    {
                        GlobalProjectData.UserRigth = 1;
                        PopUpPassword.IsOpen = false;

                        passwd = null;

                    }
                    else
                    {
                        this.PopUpPassword.IsOpen = false;
                        PopUpPassword.IsOpen = false;
                        POPUP.ShowPopup("le mot de passe saisi est incorrect");
                        this.RadioButtonDefault.IsChecked = true;
                        this.RadioButtonAdmin.IsChecked = false;

                        passwd = null;

                    }
                    PasswordBoxUserRight.Password = string.Empty;




                }

            }

        }
        private void ButtonValidateClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(passwd))
            {
                if (passwd.Equals(GlobalProjectData.password))
                {
                    GlobalProjectData.UserRigth = 1;
                    PopUpPassword.IsOpen = false;

                    passwd = null;

                }
                else
                {
                    this.PopUpPassword.IsOpen = false;
                    PopUpPassword.IsOpen = false;
                    POPUP.ShowPopup("le mot de passe saisi est incorrect");
                    this.RadioButtonDefault.IsChecked = true;
                    this.RadioButtonAdmin.IsChecked = false;

                    passwd = null;

                }
                PasswordBoxUserRight.Password = string.Empty;




            }
        }

        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            passwd = PasswordBoxUserRight.Password;
        }


        //if changing the valuie of the strings used to define colums is required. To finish.
        //#region DescriptionStringSettings

        //private void TextBoxDescriptionKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        //{

        //}

        //private void DescriptionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{

        //}


        //#endregion

        //#region NameTagStringSettings
        //private void TextBoxNameTagKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        //{

        //}

        //private void NameTagTextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{

        //}

        //#endregion

        //#region RequireStringSettings
        //private void TextBoxRequiredTagKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        //{

        //}

        //private void RequiredTextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{

        //}


        //#endregion

        //#region PresentStringSettings
        //private void TextBoxPresentKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        //{

        //}

        //private void PresentTextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{

        //}


        //#endregion

        //#region ManufacturerStringSettings
        //private void TextBoxManufacturerKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        //{

        //}

        //private void ManufacturerTextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{

        //}


        //#endregion

        //#region SerialNumberStringSettings
        //private void TextBoxSerialNumberKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        //{

        //}

        //private void SerialNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{

        //}

        //#endregion


        //#region CommentStringSettings

        //private void TextBoxCommenttKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        //{

        //}

        //private void CommentTextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{

        //}

        //#endregion

        #endregion

        #region privateMethods


        #endregion



    }
}
