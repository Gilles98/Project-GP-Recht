
using Project_Recht.Service;
using Project_Recht_DAL;
using Project_Recht_DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Project_Recht.ViewModels
{
    public class OperatiesRechtbankViewModel : Basis
    {
        private Rechtbank _rechtbank;
        Service.IDialog dialog = new Service.Dialog();
        private readonly ITreeUpdate action;
        private IUnitOfWork Uow { get; set; }

        private string _naam;
        private string _gemeente;
        private string _straat;
        private string _huisnummer;
        private int _postcode;


        public override string this[string columnName]
        {

            get
            {
                string melding = "* Verplicht veld";
                if (columnName == "Postcode" && Postcode == 0)
                {
                    return "* Geef de postcode in";
                }
                if (columnName == "Postcode" && Postcode < 1000 || Postcode > 9999)
                {
                    return "* Een postcode ligt tussen 1000 en 9999";
                }
                if (columnName == "Naam" && string.IsNullOrWhiteSpace(Naam))
                {
                    return melding;
                }

                if (columnName == "Straat" && string.IsNullOrWhiteSpace(Straat))
                {
                    return melding;
                }

                if (columnName == "Gemeente" && string.IsNullOrWhiteSpace(Gemeente))
                {
                    return melding;
                }
                if (columnName == "Huisnummer" && string.IsNullOrWhiteSpace(Huisnummer))
                {
                    return melding;
                }

                return "";
            }

        }

        public string Naam
        {
            get
            {
                return _naam;
            }
            set
            {
                _naam = value;
            }
        }

        public string Gemeente
        {
            get
            {
                return _gemeente;
            }
            set
            {
                _gemeente = value;
            }
        }

        public string Straat
        {
            get
            {
                return _straat;
            }
            set
            {
                _straat = value;
            }
        }


        public string Huisnummer
        {
            get
            {
                return _huisnummer;
            }
            set
            {
                _huisnummer = value;
            }
        }

        public int Postcode
        {
            get
            {
                return _postcode;
            }
            set
            {
                _postcode = value;
            }
        }
        //gaat rechtbank instellen
        public void RechtbankInstellen()
        {
            if (Rechtbank == null)
            {
                Rechtbank = new Rechtbank();
            }

            Rechtbank.Naam = Naam;
            Rechtbank.HuisNr = Huisnummer;
            Rechtbank.Postcode = Postcode;
            Rechtbank.Straat = Straat;
            Rechtbank.Gemeente = Gemeente;
        }
        //gaat resetten 
        public void Reset()
        {
            Rechtbank = new Rechtbank();
            Naam = null;
            Huisnummer = null;
            Postcode = 0;
            Straat = null;
            Gemeente = null;
        }
        //gaat properties instellen
        public void PropertiesInstellen()
        {
            Naam = Rechtbank.Naam;
            Huisnummer = Rechtbank.HuisNr;
            Postcode = Rechtbank.Postcode;
            Straat = Rechtbank.Straat;
            Gemeente = Rechtbank.Gemeente;
        }

        public Rechtbank Rechtbank
        {
            get
            {
                return _rechtbank;
            }

            set
            {
                _rechtbank = value;
            }
        }


        public OperatiesRechtbankViewModel(IUnitOfWork unitOfWork, ITreeUpdate parentAction)
        {
            //interface gelijkstellen aan parameter
            this.action = parentAction;
            Uow = unitOfWork;
        }

        public OperatiesRechtbankViewModel(int id, IUnitOfWork unitOfWork, ITreeUpdate parentAction)
        {
            //interface gelijkstellen aan parameter
            this.action = parentAction;
            this.Uow = unitOfWork;

            //instellen van properties
            Rechtbank = Uow.RechtbankRepo.ZoekOpPK(id);
            PropertiesInstellen();
        }


        public void Verwijderen()
        {
            // de rechters aanwezig checken
            if (Rechtbank.Rechters.Count > 0)
            {
                if (dialog.ToonMessageBoxPlusReturnAntwoord("Deze rechtbank bevat nog rechters en rechtzaken, bent u zeker dat u deze wil verwijderen?", "Melding"))
                {
                    //tot en met lijn 234 cascade regelen
                    var rechtzaken = Uow.RechtzaakRepo.Ophalen(x => x.RechtbankID == Rechtbank.RechtbankID);
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
                                //status jurylid aanpassen
                                lid.Jurylid.Opgeroepen = false;
                                Uow.JurylidRepo.Aanpassen(lid.Jurylid);
                            }
                            Uow.JuryRepo.VerwijderenRange(jury);
                        }
                        Uow.RechtzaakRepo.VerwijderenRange(rechtzaken);
                    }
                    Uow.RechterRepo.VerwijderenRange(Rechtbank.Rechters);
                    Uow.RechtbankRepo.Verwijderen(Rechtbank);

                    int ok = Uow.Save();
                    //veiligheidscheck
                    if (ok > 0)
                    {
                        //treeview updaten
                        action.Update();
                    }
                }
            }
            else
            {
                Uow.RechtbankRepo.Verwijderen(Rechtbank);
                int ok = Uow.Save();
                //veiligheidscheck
                if (ok > 0)
                {
                    //treeview updaten
                    action.Update();
                }
            }
            //resetten
            Reset();

        }
        public void Bewaren()
        {
            if (this.IsGeldig())
            {

                //rechtbank instellen
                RechtbankInstellen();
                //controle op id om te bepalen of de rechtbank wordt toegevoegd of aangepast
                if (Rechtbank.RechtbankID <= 0)
                {

                    Uow.RechtbankRepo.Toevoegen(Rechtbank);
                    int ok = Uow.Save();
                    //veiligheidscheck
                    if (ok > 0)
                    {
                        //resetten
                        Reset();
                        //treeview updaten
                        action.Update();
                    }

                }
                else
                {
                    Uow.RechtbankRepo.Aanpassen(Rechtbank);
                    int ok = Uow.Save();
                    //veiligheidscheck
                    if (ok > 0)
                    {
                        //treeview updaten
                        action.Update();
                    }
                }
            }
        }
        public override bool CanExecute(object parameter)
        {
            if (parameter.ToString() == "Verwijderen")
            {
                if (Rechtbank == null || Rechtbank.RechtbankID <= 0)
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
