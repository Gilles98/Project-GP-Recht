using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Recht.Service
{
    interface IDialog
    {
        //messageboxes kunnen tonen
        void ToonMessageBox(string bericht);
        bool ToonMessageBoxPlusReturnAntwoord(string message, string titel);
    }
}
