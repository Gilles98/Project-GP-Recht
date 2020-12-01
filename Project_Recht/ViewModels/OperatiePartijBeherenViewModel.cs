using Project_Recht.Service;
using Project_Recht.Static;
using Project_Recht_DAL;
using Project_Recht_DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Recht.ViewModels
{
    public class OperatiePartijBeherenViewModel : Basis
    {

        private object _persoon;
        private IDialog dialog = new Dialog();
        private bool _checkRadio1;
        private bool _checkRadio2;
        private bool _enabledBeklaagde;
        private bool _enabledAanklager;
        private string _voornaam;
        private string _achternaam;
        private string _gemeente;
        private string _straat;
        private string _huisnummer;
        private int _postcode;
        IUnitOfWork Uow { get; set; }
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

        public string Gemeente
        {
            get
            {
                return _gemeente;
            }
            set
            {
                _gemeente = value;
                NotifyPropertyChanged();
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
                NotifyPropertyChanged();
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
                NotifyPropertyChanged();
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
                NotifyPropertyChanged();
            }
        }

        public bool EnabledBeklaagde
        {
            get
            {
                return _enabledBeklaagde;
            }

            set
            {
                _enabledBeklaagde = value;
                NotifyPropertyChanged();
            }
        }

        public bool EnabledAanklager
        {
            get
            {
                return _enabledAanklager;
            }
            set
            {
                _enabledAanklager = value;
                NotifyPropertyChanged();
            }
        }
        public bool CheckRadio1
        {
            get
            {
                return _checkRadio1;
            }
            set
            {

                _checkRadio1 = value;
                if (CheckRadio2)
                {
                    CheckRadio2 = false;
                }
                NotifyPropertyChanged();

            }
        }

        public bool CheckRadio2
        {
            get
            {
                return _checkRadio2;
            }
            set
            {

                _checkRadio2 = value;
                if (CheckRadio1)
                {
                    CheckRadio1 = false;
                }
                NotifyPropertyChanged();

            }
        }

        public object Persoon
        {
            get
            {
                return _persoon;
            }
            set
            {
                _persoon = value;
                NotifyPropertyChanged();
            }
        }


        public override string this[string columnName]
        {
            get
            {
                try
                {
                    if (columnName == "Postcode" && Postcode == 0)
                    {
                        return "* Geef de postcode in";
                    }
                    if (columnName == "Postcode" && Postcode < 1000 || Postcode > 9999)
                    {
                        return "* Een postcode ligt tussen 1000 en 9999";
                    }

                    var lijst = this.GetType().GetProperties().ToList();

                    for (int i = 0; i <= lijst.Count - 1; i++)
                    {
                        if (lijst[i].Name == "Item")
                        {
                            lijst.Remove(lijst[i]);
                        }
                        if (lijst[i].Name == columnName && lijst[i].GetValue(this, null) == null || (string)lijst[i].GetValue(this, null) == "")
                        {
                            return "* Verplicht veld";
                        }

                    }
                }

                catch (Exception ex)
                {
                    Foutlogger.FoutLoggen(ex);

                }
                return "";
            }
        }

        public OperatiePartijBeherenViewModel()
        {
            EnabledAanklager = true;
            EnabledBeklaagde = true;
            Persoon = new object();
        }

        public void PropertiesInstellen(object persoon)
        {
            if (persoon is Aanklager aanklager)
            {
                CheckRadio2 = true;
                Voornaam = aanklager.Voornaam;
                Achternaam = aanklager.Achternaam;
                Gemeente = aanklager.Gemeente;
                Postcode = aanklager.Postcode;
                Straat = aanklager.Straat;
                Huisnummer = aanklager.HuisNr;
            }
            if (persoon is Beklaagde beklaagde)
            {
                CheckRadio1 = true;
                Voornaam = beklaagde.Voornaam;
                Achternaam = beklaagde.Achternaam;
                Gemeente = beklaagde.Gemeente;
                Postcode = beklaagde.Postcode;
                Straat = beklaagde.Straat;
                Huisnummer = beklaagde.HuisNr;
            }
            Persoon = persoon;
            
        }

        public OperatiePartijBeherenViewModel(object persoon, IUnitOfWork unitOfWork)
        {
            PropertiesInstellen(persoon);
            this.Uow = unitOfWork;
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }
        public void Verwijderen()
        {
            if (Persoon is Beklaagde beklaagde)
            {
                //manuele cascade
                var fkRelatie = Uow.RechtzaakBeklaagdeRepo.Ophalen(x => x.BeklaagdeID == beklaagde.BeklaagdeID).SingleOrDefault();
                Uow.RechtzaakBeklaagdeRepo.Verwijderen(fkRelatie);
                Uow.BeklaagdeRepo.Verwijderen((Beklaagde)Persoon);
            }
            if (Persoon is Aanklager aanklager)
            {
                //manuele cascade
                var fkRelatie = Uow.RechtzaakAanklagerRepo.Ophalen(x => x.AanklagerID == aanklager.AanklagerID).SingleOrDefault();
                Uow.RechtzaakAanklagerRepo.Verwijderen(fkRelatie);
                Uow.AanklagerRepo.Verwijderen((Aanklager)Persoon);
            }
            int ok = Uow.Save();
            StaticPersoon.Updaten();
            dialog.ToonMessageBox("De persoon is succesvol uit de rechtzaak verwijderd!");
            
        }
        public void Bewaren()
        {
            if (this.IsGeldig())
            {
                if (dialog.ToonMessageBoxPlusReturnAntwoord("Bent u zeker van al deze instellingen?", "Controle"))
                {
                    if (Persoon is Aanklager aanklager)
                    {
                        aanklager.Voornaam = Voornaam;
                        aanklager.Achternaam = Achternaam;
                        aanklager.Postcode = Postcode;
                        aanklager.Straat = Straat;
                        aanklager.HuisNr = Huisnummer;
                        aanklager.Gemeente = Gemeente;
                        if (aanklager.AanklagerID > 0)
                        {
                            Uow.AanklagerRepo.Aanpassen(aanklager);
                            Uow.Save();

                            StaticPersoon.Updaten();
                            dialog.ToonMessageBox("Aanklager is aangepast!");
                            
                        }
                    }
                   else if (Persoon is Beklaagde beklaagde)
                    {
                        beklaagde.Voornaam = Voornaam;
                        beklaagde.Achternaam = Achternaam;
                        beklaagde.Postcode = Postcode;
                        beklaagde.Straat = Straat;
                        beklaagde.HuisNr = Huisnummer;
                        beklaagde.Gemeente = Gemeente;
                        if (beklaagde.BeklaagdeID > 0)
                        {
                            Uow.BeklaagdeRepo.Aanpassen(beklaagde);
                            Uow.Save();
                            
                            StaticPersoon.Updaten();
                            dialog.ToonMessageBox("Beklaagde is aangepast!");
                        }                       
                    }
                    else
                    {
                        if (CheckRadio1)
                        {
                            Persoon = new Beklaagde() { Achternaam = Achternaam, Voornaam = Voornaam, Gemeente = Gemeente, HuisNr = Huisnummer, Straat = Straat, Postcode = Postcode };
                            StaticPersoon.KrijgBeklaagdes((Beklaagde)Persoon);
                            Persoon = new object();
                            dialog.ToonMessageBox("Er is een beklaagde toegevoegd!");
                        }
                        else if (CheckRadio2)
                        {
                            Persoon = new Aanklager() { Achternaam = Achternaam, Voornaam = Voornaam, Gemeente = Gemeente, HuisNr = Huisnummer, Straat = Straat, Postcode = Postcode };
                            StaticPersoon.KrijgAanklagers((Aanklager)Persoon);
                            Persoon = new object();
                            dialog.ToonMessageBox("Er is een aanklager toegevoegd!");
                        }
                        else
                        {
                            dialog.ToonMessageBox("Selecteer het type persoon dat je wil toevoegen!");
                        }
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
                    {
                        Verwijderen();
                    }
                    break;
            }
        }
    }
}
