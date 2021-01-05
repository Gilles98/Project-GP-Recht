using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Recht.Service
{
    public interface IClose
    {
        //delegate en methode voor het sluiten van een venster
        event EventHandler CloseWindow;
        void CloseView(object s, EventArgs e);
    }
}
