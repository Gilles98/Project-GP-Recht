using Project_Colruyt_WPF.ViewModels;
using Project_Recht_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Project_Recht.ViewModels
{
   public class RechtzakenBeherenViewModel: Basis
    {
        public RechtzakenBeherenViewModel()
        {
            
        }

        public override string this[string columnName] => throw new NotImplementedException();

        public override bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public override void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
