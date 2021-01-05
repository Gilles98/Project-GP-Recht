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
        public bool EnabledVerwijderen { get; set; }
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

        public bool EnabledBeklaagde
        {
            get
            {
                return _enabledBeklaagde;
            }

            set
            {
                _enabledBeklaagde = value;
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
            }
        }


        public override string this[string columnName]
        {
            get
            {
                    string verplichtMelding = "* Verplicht veld";

                    if (columnName == "Postcode" && Postcode < 1000 || Postcode > 9999)
                    {
                        return "* Een postcode ligt tussen 1000 en 9999";
                    }

                    if (columnName == "Voornaam" && string.IsNullOrWhiteSpace(Voornaam))
                    {
                        return verplichtMelding;
                    }
                    if (columnName == "Achternaam" && string.IsNullOrWhiteSpace(Achternaam))
                    {
                        return verplichtMelding;
                    }

                    if (columnName == "Straat" && string.IsNullOrWhiteSpace(Straat))
                    {
                        return verplichtMelding;
                    }

                    if (columnName == "Gemeente" && string.IsNullOrWhiteSpace(Gemeente))
                    {
                        return verplichtMelding;
                    }

                    if (columnName == "Huisnummer" && string.IsNullOrWhiteSpace(Huisnummer))
                    {
                    return verplichtMelding;
                    }
                return "";
            }
        }

        public OperatiePartijBeherenViewModel(IUnitOfWork unitOfWork)
        {
            this.Uow = unitOfWork;
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
            EnabledVerwijderen = true;
        }

        public override bool CanExecute(object parameter)
        {
            switch (parameter.ToString())
            {
                case "Verwijderen":
                    //beschikbaareid van de verwijder knop instellen
                    if (EnabledVerwijderen)
                    {
                        return true;
                    }
                    return false;
            }
            return true;
        }
        //kijken in hoeveel zaken de persoon nog actief is. meer dan 1 betekent dat er eerst een rechtzaak moet aflopen of moet worden verwijderd
        public int CheckAantalZaken()
        {
            //teller 
            int check = 0;

            if (Persoon is Beklaagde beklaagde)
            {
                var relatie = Uow.RechtzaakBeklaagdeRepo.Ophalen(x => x.BeklaagdeID == beklaagde.BeklaagdeID);
                if (relatie.Count() > 0 )
                {
                    foreach (var item in relatie)
                    {
                        check++;
                    }
                }
                

            }
            if (Persoon is Aanklager aanklager)
            {
                var relatie = Uow.RechtzaakAanklagerRepo.Ophalen(x => x.AanklagerID == aanklager.AanklagerID);
                if (relatie.Count() > 0)
                {
                    foreach (var item in relatie)
                    {
                        check++;
                    }
                }
                
            }
            return check;
        }
        public void Verwijderen()
        {
            //aantal lopende zaken checken
            if (CheckAantalZaken() <= 1)
            {
                //check of het een beklaagde is
                if (Persoon is Beklaagde beklaagde)
                {

                    //manuele cascade
                    var fkRelatie = Uow.RechtzaakBeklaagdeRepo.Ophalen(x => x.BeklaagdeID == beklaagde.BeklaagdeID).SingleOrDefault();
                    if (fkRelatie != null)
                    {
                        Uow.RechtzaakBeklaagdeRepo.Verwijderen(fkRelatie);
                    }

                    Uow.BeklaagdeRepo.Verwijderen((Beklaagde)Persoon);
                }
                //check of het een aanklager is
                if (Persoon is Aanklager aanklager)
                {
                    //manuele cascade
                    var fkRelatie = Uow.RechtzaakAanklagerRepo.Ophalen(x => x.AanklagerID == aanklager.AanklagerID).SingleOrDefault();
                    if (fkRelatie != null)
                    {
                        Uow.RechtzaakAanklagerRepo.Verwijderen(fkRelatie);
                    }
                    Uow.AanklagerRepo.Verwijderen((Aanklager)Persoon);
                }
                Uow.Save();
                StaticHelper.Updaten(Persoon);
                dialog.ToonMessageBox("De persoon is succesvol verwijderd!");
            }
            //wanneer een persoon meer dan 1 zaak heeft lopen
            else
            {
                dialog.ToonMessageBox("Deze persoon is bij meer dan 1 rechtzaak betrokken en mag niet verwijderd worden. verwijder een aantal rechtzaken tot er 1 overblijft of wacht af");
            }
           
            
        }
        //methode om de properties te resetten
        public void Reset()
        {
            Persoon = new object();
            Voornaam = "";
            Achternaam = "";
            Postcode = 0;
            Huisnummer = "";
            Straat = "";
            Gemeente = "";
        }
        public void Bewaren()
        {
            if (this.IsGeldig())
            {
                if (dialog.ToonMessageBoxPlusReturnAntwoord("Bent u zeker van al deze instellingen?", "Controle"))
                {
                    if (Persoon is Aanklager aanklager)
                    {
                        //aanklager instellen
                        aanklager.Voornaam = Voornaam;
                        aanklager.Achternaam = Achternaam;
                        aanklager.Postcode = Postcode;
                        aanklager.Straat = Straat;
                        aanklager.HuisNr = Huisnummer;
                        aanklager.Gemeente = Gemeente;

                        //toevoegen aan lijst en database
                        Uow.AanklagerRepo.Aanpassen(aanklager);
                        Uow.Save();
                        StaticHelper.LijstAanpassen(aanklager);
                        dialog.ToonMessageBox("Aanklager is aangepast!");
                    }
                   else if (Persoon is Beklaagde beklaagde)
                    {
                        //beklaagde aanpassen
                        beklaagde.Voornaam = Voornaam;
                        beklaagde.Achternaam = Achternaam;
                        beklaagde.Postcode = Postcode;
                        beklaagde.Straat = Straat;
                        beklaagde.HuisNr = Huisnummer;
                        beklaagde.Gemeente = Gemeente;

                        //toevoegen aan lijst en database
                        Uow.BeklaagdeRepo.Aanpassen(beklaagde);
                        Uow.Save();
                        StaticHelper.LijstAanpassen(beklaagde);
                        dialog.ToonMessageBox("Beklaagde is aangepast!");
                    }
                    else
                    {
                        if (CheckRadio1)
                        {
                            Persoon = new Beklaagde() { Achternaam = Achternaam, Voornaam = Voornaam, Gemeente = Gemeente, HuisNr = Huisnummer, Straat = Straat, Postcode = Postcode };

                            //checkt of de aanklager al bestaat
                            var checkPersoon = Uow.BeklaagdeRepo.Ophalen(x => x.Voornaam == Voornaam && x.Achternaam == Achternaam).SingleOrDefault();
                            if (checkPersoon == null)
                            {
                             //wordt al toegevoegd aan de database. bij opslagen van de rechtzaak wordt elke beklaagde of aanklager aan de rechtzaak gelinked
                                Uow.BeklaagdeRepo.Toevoegen((Beklaagde)Persoon);
                                Uow.Save();
                                StaticHelper.KrijgBeklaagdes((Beklaagde)Persoon);
                            }
                            else
                            {
                                //enkel toegevoegd aan de lijst
                                StaticHelper.KrijgBeklaagdes(checkPersoon);
                            }
                            Reset();
                            dialog.ToonMessageBox("Er is een beklaagde toegevoegd!\nslaag de rechtzaak op om hem officieel te linken aan de rechtzaak");
                        }
                        else if (CheckRadio2)
                        {
                            Persoon = new Aanklager() { Achternaam = Achternaam, Voornaam = Voornaam, Gemeente = Gemeente, HuisNr = Huisnummer, Straat = Straat, Postcode = Postcode };
                           

                            var checkPersoon = Uow.AanklagerRepo.Ophalen(x => x.Voornaam == Voornaam && x.Achternaam == Achternaam).SingleOrDefault();
                            //checkt of de aanklager al bestaat
                            if (checkPersoon == null)
                            {
                                //wordt al toegevoegd aan de database. bij opslagen van de rechtzaak wordt elke beklaagde of aanklager aan de rechtzaak gelinked
                                Uow.AanklagerRepo.Toevoegen((Aanklager)Persoon);
                                Uow.Save();
                                StaticHelper.KrijgAanklagers((Aanklager)Persoon);
                            }

                            else
                            {
                                //enkel toegevoegd aan lijst
                                StaticHelper.KrijgAanklagers(checkPersoon);
                            }
                            //resetten
                            Reset();
                            dialog.ToonMessageBox("Er is een aanklager toegevoegd!\nslaag de rechtzaak op om hem officieel te linken aan de rechtzaak");
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
