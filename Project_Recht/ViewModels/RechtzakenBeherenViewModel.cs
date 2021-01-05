
using Project_Recht.Service;
using Project_Recht.UserControls;
using Project_Recht_DAL;
using Project_Recht_DAL.UnitOfWork;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Project_Recht.ViewModels
{
    public class RechtzakenBeherenViewModel : Basis
    {
        public IUnitOfWork uow = new UnitOfWork(new RechtContext());
        private ObservableCollection<Rechtbank> _rechtbanken;
        private IDialog dialog = new Dialog();
        private IntroRechtbankenEnRechters intro = new IntroRechtbankenEnRechters();
        private UserControl _control;
        private ObservableCollection<TreeViewItem> _tree;
        private TreeViewItem _treeItem;
        private bool _enableDetails;

        public bool EnableDetails
        {
            get
            {
                return _enableDetails;
            }
            set
            {
                _enableDetails = value;
            }
        }
        public TreeViewItem TreeItem
        {
            get
            {
                return _treeItem;
            }
            set
            {
                _treeItem = value;
            }
        }
        // view wordt hergebruikt voor als ik dus ik bind de titels en de buttons ook al direct met properties
       
        public string Title { get; set; }
        public string Command1 => "Rechtzaak\nbeheren";

        public string Command2 => "Details";



        public UserControl Control
        {
            get
            {
                return _control;
            }
            set
            {
                _control = value;
            }
        }
        public ObservableCollection<TreeViewItem> Tree
        {
            get
            {
                return _tree;
            }
            set
            {
                _tree = value;
            }
        }

        public ObservableCollection<Rechtbank> Rechtbanken
        {
            get
            {
                return _rechtbanken;
            }
            set
            {
                _rechtbanken = value;
                BouwBoom();
            }
        }

        public override string this[string columnName] => throw new NotImplementedException();

        public RechtzakenBeherenViewModel()
        {
            Title = "Rechtzaken beheren";
            Tree = new ObservableCollection<TreeViewItem>();
            TreeItem = new TreeViewItem();
            //enkel de rechtbanken met een rechtzaak worden opgehaald
            Rechtbanken = new ObservableCollection<Rechtbank>(uow.RechtbankRepo.Ophalen(x => x.Rechtzaken.Count > 0, includes:y => y.Rechtzaken));
            IntroRechtzaken intro = new IntroRechtzaken();
            Control = intro;
        }

        //gaat de lijst van Tree opvullen met treeviewitems
        public void BouwBoom()
        {
            //hierarchie bepalen. eerst een parent maken voor de rechtbank en dan aan de parent rechtzaken toevoegen

            //Tag = ID van de instantie dit maakt het doorgeven aan een constructor heel makkelijk
            foreach (var rechtbank in Rechtbanken)
            {
                TreeViewItem parent = new TreeViewItem() { Header = rechtbank.Naam };
                foreach (var rechtzaak in rechtbank.Rechtzaken)
                {
                    parent.Items.Add(new TreeViewItem() { Header = "Rechtzaak - " + rechtzaak.Code, Tag = rechtzaak.RechtzaakID, });
                }
                //parent met zijn rechtzaken toevoegen aan de tree
                Tree.Add(parent);
            }
        }

        public void OperatieOpenen(string keuze)
        {

            if (keuze == "Details")
            {
                //aan de hand van een selected item wordt er een actie uitgevoerd
                if (!TreeItem.IsSelected)
                {
                    dialog.ToonMessageBox("Eerst een rechtzaak selecteren!");
                }
                else
                {
                    ///details view openen
                    Details usc = new Details();
                    ///controle op hierarchie selectie
                    if (TreeItem.Header.ToString().Contains("Rechtzaak -"))
                    {
                        usc.DataContext = new DetailsViewModel(uow, (int)TreeItem.Tag);
                        Control = usc;

                    }
                    else
                    {
                        Service.IDialog dialog = new Service.Dialog();
                        dialog.ToonMessageBox("U moet een rechtzaak selecteren en geen rechtbank!");
                    }
                    
                }
            }

            else
            {
                RechtzaakBeheren view = new RechtzaakBeheren();
                OperatieRechtzaakBeherenViewModel vm = null;

                //aan de hand van de header naam wordt er een viewmodel ingesteld
                if (keuze == "Rechtzaak" && TreeItem.IsSelected && TreeItem.Header.ToString().Contains("Rechtzaak -"))
                {
                     vm = new OperatieRechtzaakBeherenViewModel(uow, true, (int)TreeItem.Tag);  
                }
                else
                {
                    vm = new OperatieRechtzaakBeherenViewModel(uow, false);
                }

                //gaat event in interface instellen zodat de window geclosed wordt als een rechtzaak is verwijderd, aangepast of aangemaakt
                vm.Closer = new Close();
                vm.Closer.CloseWindow += (s, e) => view.Close();
                view.DataContext = vm;

                view.ShowDialog();

                //als de view gesloten is alles updaten
                TreeItemHerinstellen();
                UpdateBoom();
            }

            


        }

        //methode om de treeview te updaten
        public void UpdateBoom()
        {
            Tree = new ObservableCollection<TreeViewItem>();
            Rechtbanken = new ObservableCollection<Rechtbank>(uow.RechtbankRepo.Ophalen(x => x.Rechtzaken.Count > 0, includes: y => y.Rechtzaken));
            
        }

        //gaat treeview herinstellen
        public void TreeItemHerinstellen()
        {
            TreeItem.IsSelected = false;
        }
        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            if (parameter.ToString() == Command1)
            {
                OperatieOpenen("Rechtzaak");
            }

            else
            {
                OperatieOpenen("Details");
            }
        }
    }
}
