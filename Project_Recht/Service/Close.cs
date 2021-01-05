using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Recht.Service
{
    public class Close: IClose
    {
        //eventhandler die wordt ge invoked
        public event EventHandler CloseWindow;

        public void CloseView(object s, EventArgs e)
        {
            CloseWindow.Invoke(s, e);
        }
    }
}
