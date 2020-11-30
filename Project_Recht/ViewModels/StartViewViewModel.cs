
using Project_Recht_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Project_Recht.ViewModels
{
    public class StartViewViewModel : Basis
    {

        public override string this[string columnName] => throw new NotImplementedException();

        public void OpenRechtzakenBeheren()
        {
            ViewUserControl view = new ViewUserControl();

            RechtzakenBeherenViewModel viewModel = new RechtzakenBeherenViewModel();
            view.DataContext = viewModel;
            view.ShowDialog();
        }



        public void OpenRechtbanken()
        {
            ViewUserControl view = new ViewUserControl();

            RechtbankenRechtersViewModel viewModel = new RechtbankenRechtersViewModel();
            view.DataContext = viewModel;

            view.ShowDialog();
        }
        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            switch (parameter.ToString())
            {
                case "Rechtzaken":
                    OpenRechtzakenBeheren();
                    break;
                case "RechtersRechtbanken":
                    OpenRechtbanken();
                    break;
                default:
                    break;
            }
        }
    }
}
