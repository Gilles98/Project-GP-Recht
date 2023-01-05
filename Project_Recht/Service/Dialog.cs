using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Project_Recht.Service
{

//this class works as a indirect between the viewmodel and the view to get the result, and show, messageboxes (small popups) 
    public class Dialog: IDialog
    {
        //to call messageboxes in my viewmodels but not break the mvvm pattern
        public void ToonMessageBox(string bericht)
        {
            MessageBox.Show(bericht);
        }

        // to get the result if based on what is clicked in the popup messagebox
        public bool ToonMessageBoxPlusReturnAntwoord(string bericht, string titel)
        {
            bool resultaat = false;
            if (MessageBox.Show(bericht, titel, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                resultaat = true;
            }
            return resultaat;
        }
    }
}
