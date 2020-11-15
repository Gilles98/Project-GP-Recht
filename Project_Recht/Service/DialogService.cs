﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Project_Recht.Service
{
    public class DialogService : IDialogService
    {
        public void ToonMessageBox(string bericht)
        {
            MessageBox.Show(bericht);
        }

        public bool ToonMessageBoxPlusAntwoord(string bericht, string titel)
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