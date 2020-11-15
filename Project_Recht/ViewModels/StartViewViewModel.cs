using Project_Colruyt_WPF.ViewModels;
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

        public void OpenControl()
        {
            //MainWindow view = new MainWindow();
            //RechtzakenBeherenViewModel viewModel = new RechtzakenBeherenViewModel();
            //view.DataContext = viewModel;
            //view.ShowDialog();
        }



        public void OpenRechtbanken()
        {
            //was de mainwindow eerst.
            RechtersRechtbanken view = new RechtersRechtbanken();

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
                    OpenControl();
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
