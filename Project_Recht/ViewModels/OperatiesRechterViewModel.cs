
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
using System.Windows;
using System.Windows.Controls;

namespace Project_Recht.ViewModels
{
    public class OperatiesRechterViewModel : Basis
    {
        //private RechtersRechtbanken context = (RechtersRechtbanken)Application.Current.Windows[1];
        private Rechtbank _selectedRechtbank;
        private ObservableCollection<Rechtbank> _rechtbanken;
        private Rechter _rechter;
        private readonly Action action;
        private Service.IDialog dialog = new Dialog();


        private string _voornaam;
        private string _achternaam;

        private IUnitOfWork Uow { get; set; }
        public string erd;

        public override string this[string columnName]
        {

            get
            {
                try
                {

                    var lijst = this.GetType().GetProperties().ToList();
                    
                    for (int i = 0; i <= lijst.Count - 1; i++)
                    {
                        if (lijst[i].Name == "Item" || lijst[i].Name == "SelectedRechtbank")
                        {
                            ///item wordt mee gegenereerd dus deze wordt gefilterd net zoals SelectedRechtbank om daar geen verwarring in te krijgen
                            lijst.Remove(lijst[i]);
                        }
                        if (lijst[i].Name == columnName && lijst[i].GetValue(this, null) == null || (string)lijst[i].GetValue(this, null)== "")
                        {
                            return "* Verplicht veld";
                        }
                    }
                }

                catch (Exception ex)
                {
                    Foutlogger.FoutLoggen(ex);
                }
                if (columnName =="SelectedRechtbank" && SelectedRechtbank == null)
                {
                    return "* Selecteer een rechtbank";
                }

                return "";
            }

        }


      
        public string Voornaam
        {
            get
            {
                return _voornaam;
            }
            set
            {
                _voornaam = value;
                NotifyPropertyChanged();
            }
        }

        public string Achternaam
        {
            get
            {
                return _achternaam;
            }
            set
            {
                _achternaam = value;
                NotifyPropertyChanged();
            }
        }
        public Rechter Rechter
        {
            get
            {
                return _rechter;
            }

            set
            {
                _rechter = value;
                NotifyPropertyChanged();
            }
        }

        public Rechtbank SelectedRechtbank
        {
            get
            {
                return _selectedRechtbank;
            }

            set
            {
                _selectedRechtbank = value;
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
            }
        }


        public OperatiesRechterViewModel(IUnitOfWork unitOfWork, Action parentAction)
        {
            this.action = parentAction;
            Uow = unitOfWork;
            Rechter = new Rechter();
            Rechtbanken = new ObservableCollection<Rechtbank>(Uow.RechtbankRepo.Ophalen());
        }

        public OperatiesRechterViewModel(int id, IUnitOfWork unitOfWork, Action parentAction)
        {
            this.action = parentAction;
            this.Uow = unitOfWork;
            Rechter = Uow.RechterRepo.ZoekOpPK(id);
            Rechtbanken = new ObservableCollection<Rechtbank>(Uow.RechtbankRepo.Ophalen());
            PropertiesInstellen();
        }


        public void RechterInstellen()
        {
            Rechter.Voornaam = Voornaam;
            Rechter.Achternaam = Achternaam;
            Rechter.RechtbankID = SelectedRechtbank.RechtbankID;
        }

        public void Reset()
        {
            SelectedRechtbank = null;
            Voornaam = "";
            Achternaam = "";
            Rechter = new Rechter();
        }

        public void PropertiesInstellen()
        {
            Voornaam = Rechter.Voornaam;
            Achternaam = Rechter.Achternaam;
            SelectedRechtbank = Uow.RechtbankRepo.Ophalen(x => x.RechtbankID == Rechter.RechtbankID).SingleOrDefault();
          
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }
        public void Verwijderen()
        {
            ///controle doen of dat de rechter nog rechtzaken heeft!

            if (Rechter.RechterID > 0)
            {
                if (dialog.ToonMessageBoxPlusReturnAntwoord("Deze rechter heeft mogelijk nog rechtzaken. Bent u zeker dat u hem wil verwijderen?", "Melding"))
                {
                    var rechtzaken = Uow.RechtzaakRepo.Ophalen(x => x.RechterID == Rechter.RechterID);
                    if (rechtzaken.Count() > 0)
                    {
                        foreach (var item in rechtzaken)
                        {
                            Uow.RechtzaakAanklagerRepo.VerwijderenRange(Uow.RechtzaakAanklagerRepo.Ophalen(x => x.RechtzaakID == item.RechtzaakID));
                            Uow.RechtzaakBeklaagdeRepo.VerwijderenRange(Uow.RechtzaakBeklaagdeRepo.Ophalen(x => x.RechtzaakID == item.RechtzaakID));
                            //jury reset
                            var jury = Uow.JuryRepo.Ophalen(x => x.RechtzaakID == item.RechtzaakID, includes: x => x.Jurylid);
                            foreach (var lid in jury)
                            {
                                lid.Jurylid.Opgeroepen = false;
                                Uow.JurylidRepo.Aanpassen(lid.Jurylid);
                            }
                            Uow.JuryRepo.VerwijderenRange(jury);
                        }
                        Uow.RechtzaakRepo.VerwijderenRange(rechtzaken);
                    }
                    
                    Uow.RechterRepo.Verwijderen(Rechter);
                    int ok = Uow.Save();
                    if (ok > 0)
                    {
                        Reset();
                        action.Invoke();
                    }
                }
            }


        }
        public void Bewaren()
        {
            if (this.IsGeldig())
            {
                RechterInstellen();
          

            if (Rechter.RechterID <= 0)
            {

                if (SelectedRechtbank != null)
                {
                    Rechter.RechtbankID = SelectedRechtbank.RechtbankID;
                    if (Rechter.Voornaam != "")
                    {
                        if (Rechter.Achternaam != "")
                        {
                            Uow.RechterRepo.Toevoegen(Rechter);
                            int ok = Uow.Save();
                            if (ok > 0)
                            {
                                action.Invoke();
                            }
                        }
                    }
                }
            }
            else
            {
                Uow.RechterRepo.Aanpassen(Rechter);
                int ok = Uow.Save();
                if (ok > 0)
                {
                    action.Invoke();
                }
            }
            }
        }

        public override void Execute(object parameter)
        {
            switch (parameter.ToString())
            {
                case "Bewaren":
                    Bewaren();
                    break;
                case "Verwijderen":
                    Verwijderen();
                    break;
            }
        }
    }
}
