
using Project_Recht.Service;
using Project_Recht.UserControls;
using Project_Recht_DAL;
using Project_Recht_DAL.UnitOfWork;
using System;
using System.Collections.Generic;
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
                NotifyPropertyChanged();
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
                NotifyPropertyChanged();
            }
        }
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

                NotifyPropertyChanged();
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
                NotifyPropertyChanged();
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
                NotifyPropertyChanged();
            }
        }

        public override string this[string columnName] => throw new NotImplementedException();

        public RechtzakenBeherenViewModel()
        {
            Title = "Rechtzaken beheren";
            Tree = new ObservableCollection<TreeViewItem>();
            TreeItem = new TreeViewItem();
            Rechtbanken = new ObservableCollection<Rechtbank>(uow.RechtbankRepo.Ophalen(y => y.Rechtzaken));
            IntroRechtbankenEnRechters intro = new IntroRechtbankenEnRechters();
            Control = intro;
        }

        //gaat de lijst van Tree opvullen met treeviewitems
        public void BouwBoom()
        {
            foreach (var rechtbank in Rechtbanken)
            {
                TreeViewItem parent = new TreeViewItem() { Header = rechtbank.Naam };
                foreach (var rechtzaak in rechtbank.Rechtzaken)
                {
                    parent.Items.Add(new TreeViewItem() { Header = "Rechtzaak - " + rechtzaak.Code, Tag = rechtzaak.RechtzaakID, });
                }
                Tree.Add(parent);
            }
        }

        //Ik geef de UnitOfWork mee door omdat ik anders een bug had bij het verwijderen van een item in zowel rechters als rechtbanken

        public void OperatieOpenen(string keuze)
        {

            if (keuze == "Details")
            {
                ///details view openen
            }

            else
            {
                RechtzaakBeheren view = new RechtzaakBeheren();
                if (keuze == "Rechtzaak" && TreeItem.IsSelected && TreeItem.Header.ToString().Contains("Rechtzaak -"))
                {
                    view.DataContext = new OperatieRechtzaakBeherenViewModel(uow, false, (int)TreeItem.Tag);
                }
                else
                {
                    view.DataContext = new OperatieRechtzaakBeherenViewModel(uow, false);
                }

                view.ShowDialog();
                UpdateBoom();
            }

            TreeItemHerinstellen();


        }
        public void UpdateBoom()
        {
            Tree = new ObservableCollection<TreeViewItem>();
            Rechtbanken = new ObservableCollection<Rechtbank>(uow.RechtbankRepo.Ophalen(x => x.Rechtzaken));
            foreach (var item in Tree)
            {
                item.IsExpanded = true;
            }
        }

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
