
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
        private Rechtbank _selectedRechtbank;
        private ObservableCollection<Rechtbank> _rechtbanken;
        private Rechter _rechter;
        private readonly ITreeUpdate action;
        private Service.IDialog dialog = new Dialog();


        private string _voornaam;
        private string _achternaam;

        private IUnitOfWork Uow { get; set; }


        public override string this[string columnName]
        {

            get
            {
                string melding = "* Verplicht veld";
                if (columnName == "Voornaam" && string.IsNullOrWhiteSpace(Voornaam))
                {
                    return melding;
                }

                if (columnName == "Achternaam" && string.IsNullOrWhiteSpace(Achternaam))
                {
                    return melding;
                }
                if (columnName == "SelectedRechtbank" && SelectedRechtbank == null)
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


        public OperatiesRechterViewModel(IUnitOfWork unitOfWork, ITreeUpdate parentAction)
        {
            //gaat interface instellen aan de hand van de meegegeven parameter
            this.action = parentAction;
            Uow = unitOfWork;
            Rechter = new Rechter();
            Rechtbanken = new ObservableCollection<Rechtbank>(Uow.RechtbankRepo.Ophalen());
        }

        public OperatiesRechterViewModel(int id, IUnitOfWork unitOfWork, ITreeUpdate parentAction)
        {
            //gaat interface instellen aan de hand van de meegegeven parameter
            this.action = parentAction;
            this.Uow = unitOfWork;
            Rechter = Uow.RechterRepo.ZoekOpPK(id);
            Rechtbanken = new ObservableCollection<Rechtbank>(Uow.RechtbankRepo.Ophalen());
            //properties instellen
            PropertiesInstellen();
        }

        //rechter instellen
        public void RechterInstellen()
        {
            Rechter.Voornaam = Voornaam;
            Rechter.Achternaam = Achternaam;
            Rechter.RechtbankID = SelectedRechtbank.RechtbankID;
        }
        //properties resetten
        public void Reset()
        {

            SelectedRechtbank = null;
            Voornaam = "";
            Achternaam = "";
            Rechter = new Rechter();
        }
        //properties instellen
        public void PropertiesInstellen()
        {
            Voornaam = Rechter.Voornaam;
            Achternaam = Rechter.Achternaam;
            SelectedRechtbank = Uow.RechtbankRepo.Ophalen(x => x.RechtbankID == Rechter.RechtbankID).SingleOrDefault();

        }

        
        public void Verwijderen()
        {
                //de vraagstellen of dat de rechter nog rechtzaken heeft lopen
                if (dialog.ToonMessageBoxPlusReturnAntwoord("Deze rechter heeft mogelijk nog rechtzaken. Bent u zeker dat u deze rechter wil verwijderen?", "Melding"))
                {
                //cascade regelen tot lijn 210
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
                    //veiligheidscheck
                    int ok = Uow.Save();
                    if (ok > 0)
                    {
                        Reset();
                        action.Update();
                    }
                }
  
        }
        public void Bewaren()
        {
            if (this.IsGeldig())
            {
                //rechter instellen
                RechterInstellen();

                if (Rechter.RechterID <= 0)
                {
                    //rechtbankID instellen
                    Uow.RechterRepo.Toevoegen(Rechter);
                    int ok = Uow.Save();
                    if (ok > 0)
                    {
                        action.Update();
                        //resetten
                        Reset();
                    }
                }
                else
                {
                    Uow.RechterRepo.Aanpassen(Rechter);
                    int ok = Uow.Save();
                    if (ok > 0)
                    {
                        action.Update();
                    }
                }
            }
        }
        public override bool CanExecute(object parameter)
        {
            //aan de hand van of dat de rechter leeg is of niet kan ik verwijderen
            if (parameter.ToString() == "Verwijderen")
            {
                if (Rechter == null || Rechter.RechterID <= 0)
                {
                    return false;
                }
            }
            return true;
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
