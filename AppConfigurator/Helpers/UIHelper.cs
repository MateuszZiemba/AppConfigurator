using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppConfigurator.Helpers
{
    public static class UIHelper
    {
        public static OpenFileDialog GetConfigOpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = System.Environment.CurrentDirectory;
            openFileDialog.Filter = "Config files (*.config)|*.config|All files (*.*)|*.*";
            openFileDialog.Multiselect = false;
            return openFileDialog;
        }
    }
}
