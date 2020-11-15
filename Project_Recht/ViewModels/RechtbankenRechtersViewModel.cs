using Project_Colruyt_WPF.ViewModels;
using Project_Recht.UserControls;
using Project_Recht_DAL;
using Project_Recht_DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
namespace Project_Recht.ViewModels
{
    public class RechtbankenRechtersViewModel : Basis
    {
        public IUnitOfWork uow = new UnitOfWork(new RechtContext());
        private ObservableCollection<Rechtbank> _rechtbanken;
        private IntroRechtbankenEnRechters intro = new IntroRechtbankenEnRechters();
        private UserControl _control;
        private ObservableCollection<TreeViewItem> _tree;
        private TreeViewItem _treeItem;

        public TreeViewItem TreeItem
        {
            get
            {
                return _treeItem;
            }
            set
            {
                _treeItem = value;
                OperatieOpenen("");
                NotifyPropertyChanged();
            }
        }
        public string Title { get; set; }

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

        public RechtbankenRechtersViewModel()
        {
            Title = "Rechtbanken en rechters";
            Tree = new ObservableCollection<TreeViewItem>();
            TreeItem = new TreeViewItem();
            Rechtbanken = new ObservableCollection<Rechtbank>(uow.RechtbankRepo.Ophalen(x => x.Rechters));
            IntroRechtbankenEnRechters intro = new IntroRechtbankenEnRechters();
            Control = intro;

        }

        //gaat de lijst van Tree opvullen met treeviewitems
        public void BouwBoom()
        {
            foreach (var rechtbank in Rechtbanken)
            {
                TreeViewItem parent = new TreeViewItem() { Header = rechtbank.Naam, Tag = rechtbank.RechtbankID, Name = "Rechtbank" };
                foreach (var rechter in rechtbank.Rechters)
                {
                    parent.Items.Add(new TreeViewItem() { Header = "Rechter - " + rechter.Voornaam + " " + rechter.Achternaam, Tag = rechter.RechterID, Name = "Rechter" });
                }
                Tree.Add(parent);
            }
        }

        //Ik geef de Uow mee door omdat ik anders een bug had bij het verwijderen van een item in zowel rechters als rechtbanken
       
        public void OperatieOpenen(string keuze)
        {
            if (TreeItem.IsSelected)
            {
                if (TreeItem.Name == "Rechtbank")
                {
                    Control = new OperatiesRechtbank();
                    Control.DataContext = new OperatiesRechtbankViewModel((int)TreeItem.Tag, uow);
                }
                else
                {
                    Control = new OperatiesRechter();
                    Control.DataContext = new OperatiesRechterViewModel((int)TreeItem.Tag, uow);
                }
            }
            else
            {
                if (keuze == "Rechtbank")
                {
                    Control = new OperatiesRechtbank();
                    Control.DataContext = new OperatiesRechtbankViewModel(uow);
                }
                else
                {
                    Control = new OperatiesRechter();
                    Control.DataContext = new OperatiesRechterViewModel(uow);
                }
            }
            TreeItemHerinstellen();

        }

        public void UpdateBoom()
        {
            Tree = new ObservableCollection<TreeViewItem>();
            Rechtbanken = new ObservableCollection<Rechtbank>(uow.RechtbankRepo.Ophalen(x => x.Rechters));
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
            switch (parameter.ToString())
            {
                case "OperatiesRechter":
                    OperatieOpenen("Rechter");
                    break;
                case "OperatiesRechtbank":
                    OperatieOpenen("Rechtbank");
                    break;
                case "RefreshTreeview":
                    UpdateBoom();
                    break;
            }
        }
    }
}
