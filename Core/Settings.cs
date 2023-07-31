using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationInventaire.Core.SettingsManagement
{
    /// <summary>
    /// This class is used to store application settings and methods used to aply it
    /// </summary>
    public class Settings
    {
        public string Language { set; get; }
        public string NameTagString { set; get; }
        public string DescriptionString { set; get; }
        public string isPresentString { set;get; }
        public string AmountString { set; get; }
        public string ManufacturerString { set; get; }
        public string SerialNuberString { set; get; }
        public string CommentString { set; get;}



        public Settings()
        {

        }
    }

}
