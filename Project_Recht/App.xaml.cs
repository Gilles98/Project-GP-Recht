using Project_Recht.ViewModels;
using Project_Recht_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Project_Recht
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //vanwege code in de methode zelf wordt deze niet telkens uitgevoerd
            DatabaseInitializer.InsertDB();

            
            StartView start = new StartView();
            StartViewViewModel startView = new StartViewViewModel();
            start.DataContext = startView;
            start.Show();
        }

       
    }
}
