﻿
using Project_Recht.Service;
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
            }
        }
        // view wordt hergebruikt voor als ik dus ik bind de titels en de buttons ook al direct met properties
        public string Title { get; set; }
        public string Command1 => "Operaties\nRechter";

        public string Command2 => "Operaties\nRechtbank";



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
            //hierarchie bepalen. eerst een parent maken voor de rechtbank en dan aan de parent rechters toevoegen

            //Tag = ID van de instantie dit maakt het doorgeven aan een constructor heel makkelijk
            foreach (var rechtbank in Rechtbanken)
            {
                TreeViewItem parent = new TreeViewItem() { Header = rechtbank.Naam, Tag = rechtbank.RechtbankID, Name = "Rechtbank" };
                foreach (var rechter in rechtbank.Rechters)
                {
                    parent.Items.Add(new TreeViewItem() { Header = "Rechter - " + rechter.Voornaam + " " + rechter.Achternaam, Tag = rechter.RechterID, Name = "Rechter" });
                }
                //parent met zijn rechters toevoegen aan de tree
                Tree.Add(parent);
            }
        }


        public void OperatieOpenen(string keuze)
        {
            ITreeUpdate update = new TreeUpdate();
            update.UpdateTree += UpdateBoom;
            if (TreeItem.IsSelected)
            {
                    //als er een rechtbank is geselecteerd
                    if (TreeItem.Name == "Rechtbank")
                    {
                        Control = new OperatiesRechtbank();
                        Control.DataContext = new OperatiesRechtbankViewModel((int)TreeItem.Tag, uow, update);
                    }
                    //als er een rechter is geselecteerd
                    else
                    {
                        Control = new OperatiesRechter();
                        Control.DataContext = new OperatiesRechterViewModel((int)TreeItem.Tag, uow, update);
                    }
            }
            else
            {
                //wanneer er op de knop voor een rechtbank wordt gedrukt
                if (keuze == "Rechtbank")
                {
                    Control = new OperatiesRechtbank();
                    Control.DataContext = new OperatiesRechtbankViewModel(uow, update);
                }
                //wanneer er op de knop voor een rechter wordt gedrukt
                else
                {
                    Control = new OperatiesRechter();
                    Control.DataContext = new OperatiesRechterViewModel(uow, update);
                }
            }
            //tree herinstellen
            TreeItemHerinstellen();

        }

        //methode om de boom te updaten
        public void UpdateBoom()
        {
            Tree = new ObservableCollection<TreeViewItem>();
            Rechtbanken = new ObservableCollection<Rechtbank>(uow.RechtbankRepo.Ophalen(x => x.Rechters));
            foreach (var item in Tree)
            {
                item.IsExpanded = true;
            }
        }

        //ok een lijntje maar toch in een methode gestopt
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
                OperatieOpenen("Rechter");
            }

            else
            {
                OperatieOpenen("Rechtbank");
            }
        }
    }
}
