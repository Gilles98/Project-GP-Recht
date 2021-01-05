using Project_Recht.Service;
using Project_Recht.Static;
using Project_Recht_DAL;
using Project_Recht_DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Project_Recht.ViewModels
{
    public class OperatieRechtzaakBeherenViewModel : Basis
    {

        private string _klacht;
        private Rechtzaak _rechtzaak;
        IDialog service = new Dialog();
        private Rechtbank _selectedRechtbank;
        private Rechter _selectedRechter;
        private ObservableCollection<Rechtbank> _rechtbanken;
        private ObservableCollection<Rechter> _rechters;
        private ObservableCollection<Beklaagde> _beklaagdes;
        private ObservableCollection<Aanklager> _aanklagers;

        private ObservableCollection<Beklaagde> _bestaandeBeklaagdes;
        private ObservableCollection<Aanklager> _bestaandeAanklagers;
        private bool _enableSelectionRechter;
        private DateTime _selectedDateTime;
        private object _selectedPersoon;

        private Beklaagde _bestaandeBeklaagde;
        private Aanklager _bestaandeAanklager;
        public bool IsSelected { get; set; }
        private IUnitOfWork Uow { get; set; }




        public IClose Closer;
        public Beklaagde BestaandeBeklaagde
        {
            get
            {
                return _bestaandeBeklaagde;
            }
            set
            {
                _bestaandeBeklaagde = value;
            }
        }

        public Aanklager BestaandeAanklager
        {
            get
            {
                return _bestaandeAanklager;
            }
            set
            {
                _bestaandeAanklager = value;
            }
        }


        public Rechtzaak Rechtzaak
        {
            get
            {
                return _rechtzaak;
            }
            set
            {
                _rechtzaak = value;
            }
        }
        public object SelectedPersoon
        {
            get
            {
                return _selectedPersoon;
            }
            set
            {
                _selectedPersoon = value;
                OpenPartijBeheren("");

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

        public ObservableCollection<Aanklager> Aanklagers
        {
            get
            {
                return _aanklagers;
            }
            set
            {
                _aanklagers = value;
            }
        }

        public ObservableCollection<Beklaagde> Beklaagdes
        {
            get
            {
                return _beklaagdes;
            }
            set
            {
                _beklaagdes = value;
            }
        }

        public ObservableCollection<Aanklager> BestaandeAanklagers
        {
            get
            {
                return _bestaandeAanklagers;
            }
            set
            {
                _bestaandeAanklagers = value;
            }
        }

        public ObservableCollection<Beklaagde> BestaandeBeklaagdes
        {
            get
            {
                return _bestaandeBeklaagdes;
            }
            set
            {
                _bestaandeBeklaagdes = value;
            }
        }

        public ObservableCollection<Rechter> Rechters
        {
            get
            {
                return _rechters;
            }
            set
            {
                _rechters = value;
            }
        }
        public string Klacht
        {
            get
            {
                return _klacht;
            }
            set
            {
                _klacht = value;
            }
        }
        public bool EnableSelectionRechter
        {
            get
            {
                return _enableSelectionRechter;
            }
            set
            {
                _enableSelectionRechter = value;
            }
        }

        public void EnableRechters()
        {

            if (SelectedRechtbank != null)
            {
                if (SelectedRechtbank.Rechters.Count > 0)
                {
                    EnableSelectionRechter = true;
                    Rechters = new ObservableCollection<Rechter>(SelectedRechtbank.Rechters);
                }
                else
                {
                    service.ToonMessageBox("Deze rechtbank bevat nog geen rechters en kan niet worden gebruikt om een rechtzaak aan te maken!");

                    //properties resetten
                    SelectedRechtbank = null;
                    EnableSelectionRechter = false;
                    Rechters = new ObservableCollection<Rechter>();
                }
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
                EnableRechters();
            }
        }

        public Rechter SelectedRechter
        {
            get
            {
                return _selectedRechter;
            }
            set
            {
                _selectedRechter = value;
            }
        }

        public void OpenPartijBeheren(string keuze)
        {
            Partijbeheren view = new Partijbeheren();


            if (keuze == "Nieuw")
            {
                view.DataContext = new OperatiePartijBeherenViewModel(Uow);
                view.ShowDialog();
            }
            else
            {
                if (SelectedPersoon is Aanklager || SelectedPersoon is Beklaagde)
                {
                    view.DataContext = new OperatiePartijBeherenViewModel(SelectedPersoon, Uow);
                    view.ShowDialog();
                }
            }
        }

        //voorkomt bug bij opstarten bij de kalender
        public bool ToonMelding { get; set; }
        public DateTime SelectedDateTime
        {
            get
            {
                return _selectedDateTime;
            }

            set
            {
                _selectedDateTime = value;
                CheckDateTime();
                NotifyPropertyChanged();
            }
        }
        public OperatieRechtzaakBeherenViewModel(IUnitOfWork uow, bool selected)
        {
            IsSelected = selected;
            ToonMelding = true;
            this.Uow = uow;
            StandaardPropertiesInstellen();
            ComboboxenInstellen();
            //om fout te voorkomen

            InstellenStaticEvents();

        }

        //voor de constructor hierboven om de properties in te stellen waar geen parameter is aan meegegeven in de constructor
        public void StandaardPropertiesInstellen()
        {
            Beklaagdes = new ObservableCollection<Beklaagde>();
            Aanklagers = new ObservableCollection<Aanklager>();
            BestaandeAanklager = new Aanklager();
            BestaandeBeklaagde = new Beklaagde();
            Rechtbanken = new ObservableCollection<Rechtbank>(Uow.RechtbankRepo.Ophalen(includes: x => x.Rechters));
            SelectedDateTime = DateTime.Now;
            SelectedPersoon = new object();
            Rechters = new ObservableCollection<Rechter>();
            Rechtzaak = new Rechtzaak();
        }
        /// gaat de update en delete operaties van een partij in de datagrid vertonen
        public void LijstenInstellen(object persoon)
        {
            var rechtzaakAanklagers = Uow.RechtzaakAanklagerRepo.Ophalen(x => x.RechtzaakID == Rechtzaak.RechtzaakID);
            var rechtzaakBeklaagdes = Uow.RechtzaakBeklaagdeRepo.Ophalen(x => x.RechtzaakID == Rechtzaak.RechtzaakID);
            if (rechtzaakAanklagers.Count() > 0)
            {
                Aanklagers = new ObservableCollection<Aanklager>();
                foreach (var item in rechtzaakAanklagers)
                {
                    Aanklagers.Add(Uow.AanklagerRepo.Ophalen(x => x.AanklagerID == item.AanklagerID).SingleOrDefault());
                }
            }

            if (rechtzaakBeklaagdes.Count() > 0)
            {
                Beklaagdes = new ObservableCollection<Beklaagde>();
                foreach (var item in rechtzaakBeklaagdes)
                {
                    Beklaagdes.Add(Uow.BeklaagdeRepo.Ophalen(x => x.BeklaagdeID == item.BeklaagdeID).SingleOrDefault());
                }
            }


            ///verwijderen regelen persoon dat nog niet gelinked is aan de rechtzaak in de database
            if (persoon is Aanklager || persoon is Beklaagde)
            {
                if (persoon is Beklaagde beklaagde)
                {
                    if (Beklaagdes.Contains(beklaagde))
                    {
                        Beklaagdes.Remove(beklaagde);
                    }

                }
                else
                {
                    if (Aanklagers.Contains((Aanklager)persoon))
                    {
                        Aanklagers.Remove((Aanklager)persoon);
                    }

                }
            }

            ComboboxenInstellen();
        }




        ///comboboxen opvullen met personen die nog niet zijn geselecteerd
        public void ComboboxenInstellen()
        {

            BestaandeAanklagers = new ObservableCollection<Aanklager>(Uow.AanklagerRepo.Ophalen());
            BestaandeBeklaagdes = new ObservableCollection<Beklaagde>(Uow.BeklaagdeRepo.Ophalen());
            ///filteren
            if (Aanklagers.Count > 0)
            {
                foreach (var aanklager in Aanklagers)
                {
                    if (BestaandeAanklagers.Contains(aanklager))
                    {
                        BestaandeAanklagers.Remove(aanklager);
                    }
                }
            }
            if (Beklaagdes.Count > 0)
            {
                foreach (var beklaagde in Beklaagdes)
                {
                    if (BestaandeBeklaagdes.Contains(beklaagde))
                    {
                        BestaandeBeklaagdes.Remove(beklaagde);
                    }
                }
            }


        }
        public void InstellenStaticEvents()
        {
            StaticHelper.UpdateLijst += PersoonWijzigenInLijst;
            StaticHelper.Aanklagers += VulAanklagers;
            StaticHelper.Beklaagdes += VulBeklaagdes;
            StaticHelper.LijstInstellen += LijstenInstellen;
        }
        public OperatieRechtzaakBeherenViewModel(IUnitOfWork uow, bool selected, int id)
        {
            IsSelected = selected;
            this.Uow = uow;
            Rechtzaak = Uow.RechtzaakRepo.ZoekOpPK(id);


            LijstenInstellen(new object());

            Rechtbanken = new ObservableCollection<Rechtbank>(Uow.RechtbankRepo.Ophalen(includes: x => x.Rechters));
            SelectedDateTime = Rechtzaak.Moment;
            ToonMelding = true;
            SelectedRechtbank = Uow.RechtbankRepo.ZoekOpPK(Rechtzaak.RechtbankID);
            BestaandeBeklaagde = new Beklaagde();
            BestaandeAanklager = new Aanklager();

            Klacht = Rechtzaak.OmschrijvingKlacht;
            SelectedRechter = uow.RechterRepo.ZoekOpPK(Rechtzaak.RechterID);
            InstellenStaticEvents();

        }
        public override string this[string columnName]
        {
            get
            {
                if (columnName == "SelectedDateTime" && SelectedDateTime.Date == DateTime.Now.Date)
                {
                    return "Selecteer een dag die niet vandaag is!";
                }

                if (columnName == "SelectedRechtbank" && SelectedRechtbank == null)
                {
                    return "U moet een rechtbank selecteren!";
                }
                if (columnName == "SelectedRechtbank" && SelectedRechtbank.Rechters.Count <= 0)
                {
                    return "Selecteer een geldige rechtbank!";
                }
                if (columnName == "SelectedRechter" && SelectedRechter == null)
                {
                    return "Selecteer de rechter uit de gekozen rechtbank";
                }
                if (columnName == "Klacht" && Klacht == null)
                {
                    return "Er moet een klacht worden opgegeven!";
                }


                return "";
            }
        }


        public void CheckDateTime()
        {
            //voor bij het starten van de view doe ik deze if. verder gaat hij er normaal altijd door
            if (SelectedDateTime.Date != DateTime.Now.Date)
            {
                //controle op een afgelope datum 
                if (DateTime.Now.Date.CompareTo(SelectedDateTime) >= 1)
                {
                    service.ToonMessageBox("Het is niet mogelijk om een afgelopen datum te selecteren!");
                    SelectedDateTime = DateTime.Now.Date;
                }
                //controle toekomstige datum
                else
                {
                    //voorkomt veel bugs door deze boolean te gebruiken
                    if (ToonMelding)
                    {
                        int beschikbareDatum = (int)(DateTime.Now.Date.AddDays(3) - SelectedDateTime.Date).TotalDays;
                        if (beschikbareDatum == 1)
                        {
                            service.ToonMessageBox("Hey hey");
                        }
                        if (beschikbareDatum < 3 && beschikbareDatum > 0)
                        {
                                service.ToonMessageBox("het is niet mogenlijk om een rechtzaak volledig te organiseren binnen de 3 dagen van de huidige datum");
                                SelectedDateTime = DateTime.Now.Date;
                        }
                    }
                }
            }
        }
        //gaat 5 juryleden aanduiden
        public List<Jury> JuryAanduiden()
        {
            //jury leden eerst ophalen
            List<Jurylid> leden = Uow.JurylidRepo.Ophalen(x => x.Opgeroepen == false).ToList();
            Random random = new Random();
            List<Jury> Jury = new List<Jury>();
            //de ik doe een while lus omdat het niet zeker is dat de for lus altijd 5 gaat halen
            while (Jury.Count < 5)
            {
                for (int i = 0; i <= leden.Count - 1; i++)
                {
                    //uitkomst random bepalen. kans is 1 op 5
                    int check = random.Next(5);
                    if (check == 3)
                    {
                        //als de check overeenkomt met het getal het huidige jurylid toevoegen
                        Jury juryLid = new Jury() { JurylidID = leden[i].JurylidID, RechtzaakID = Rechtzaak.RechtzaakID };

                        //desondanks dat de kans klein is dat een lid 2 keer erin kan komen filter ik hem toch uit de lijst van de loop
                        leden.Remove(leden[i]);
                        Jury.Add(juryLid);
                        leden[i].Opgeroepen = true;
                        Uow.JurylidRepo.Aanpassen(leden[i]);
                        Uow.Save();
                    }
                    if (Jury.Count == 5)
                    {
                        break;
                    }
                }
            }
            return Jury;
        }

        //gaat de naam van de rechtzaak genereren aan de hand van willekeurige karakters
        public string GenereerCodeRechtzaak()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            //for loop in principe
            return new string(Enumerable.Repeat(chars, 7).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        //gaat aan aanklagers een persoon toevoegen
        public void VulAanklagers(Aanklager aanklager)
        {
            Aanklagers.Add(aanklager);
        }

        //gaat aan beklaagdes een persoon toevoegen
        public void VulBeklaagdes(Beklaagde beklaagde)
        {
            Beklaagdes.Add(beklaagde);
        }

        //gaat de rechtzaak instellen voor zowel het toevoegen als het wijzigen
        public void RechtzaakInstellen()
        {
            //voor toevoegen
            if (Rechtzaak.RechtzaakID == 0)
            {
                Rechtzaak.Code = GenereerCodeRechtzaak();
            }
            Rechtzaak.RechtbankID = SelectedRechtbank.RechtbankID;
            Rechtzaak.RechterID = SelectedRechter.RechterID;
            Rechtzaak.Moment = SelectedDateTime;
            Rechtzaak.OmschrijvingKlacht = Klacht;
        }

        //als een persoon nog niet gelinked is aan de rechtzaak maar men wil hem wel editen dan wordt deze methode gebruikt om de lijst te updaten
        public void PersoonWijzigenInLijst(object nieuwPersoon)
        {
            if (SelectedPersoon is Aanklager oudeAanklager && nieuwPersoon is Aanklager vernieuwdeAanklager)
            {
                Aanklagers[Aanklagers.IndexOf(oudeAanklager)] = vernieuwdeAanklager;
                //datagrid updaten omdat dit niet automatisch gebeurde
                Aanklagers = new ObservableCollection<Aanklager>(Aanklagers);
            }
            if (SelectedPersoon is Beklaagde oudeBeklaagde && nieuwPersoon is Beklaagde vernieuwdeBeklaagde)
            {
                Beklaagdes[Beklaagdes.IndexOf(oudeBeklaagde)] = vernieuwdeBeklaagde;

                //datagrid updaten omdat dit niet automatisch gebeurde
                Beklaagdes = new ObservableCollection<Beklaagde>(Beklaagdes);
            }
            //nodig om de aangepaste gebruiker terug te filteren
            ComboboxenInstellen();
            SelectedPersoon = new object();
        }

        public void Verwijderen()
        {
            if (Rechtzaak.RechtbankID > 0)
            {
                //jury herinstellen
                var jury = Uow.JuryRepo.Ophalen(x => x.RechtzaakID == Rechtzaak.RechtzaakID);
                foreach (Jury personen in jury)
                {
                    var leden = Uow.JurylidRepo.Ophalen(x => x.JurylidID == personen.JurylidID);
                    foreach (Jurylid lid in leden)
                    {
                        //jurylid status aanpassen
                        lid.Opgeroepen = false;
                        Uow.JurylidRepo.Aanpassen(lid);
                    }
                    //manuele cascade
                    Uow.JuryRepo.Verwijderen(personen);
                }
                //manuele cascade
                var rechtzaakAanklagers = Uow.RechtzaakAanklagerRepo.Ophalen(x => x.RechtzaakID == Rechtzaak.RechtzaakID);
                var rechtzaakBeklaagdes = Uow.RechtzaakBeklaagdeRepo.Ophalen(x => x.RechtzaakID == Rechtzaak.RechtzaakID);
                Uow.RechtzaakAanklagerRepo.VerwijderenRange(rechtzaakAanklagers);
                Uow.RechtzaakBeklaagdeRepo.VerwijderenRange(rechtzaakBeklaagdes);
                Uow.Save();
                Uow.RechtzaakRepo.Verwijderen(Rechtzaak);
                Uow.Save();
                service.ToonMessageBox("Zaak verwijderd!");

                //mvvm vriendelijke this.close :D
                Closer.CloseView(this, new EventArgs());

            }
        }


        public void Bewaren()
        {
            if (this.IsGeldig())
            {
                //partijen controleren
                if (Beklaagdes.Count == 0 || Aanklagers.Count == 0)
                {
                    service.ToonMessageBox("Een rechtbank is pas legitiem als beide partijen zijn opgevuld met minstens 1 persoon");
                }
                //beschikbaarheid juryleden controleren
                else if (Uow.JurylidRepo.Ophalen(x => x.Opgeroepen == false).Count() < 5)
                {
                    service.ToonMessageBox("Er zijn niet genoeg juryleden op dit moment beschikbaar om een rechtzaak aan te maken!\nWacht tot een lopende rechtzaak is afgelopen, verwijder deze of haal juryleden uit de rechtzaak");
                }
                else
                {
                    RechtzaakInstellen();
                    if (Rechtzaak.RechtzaakID <= 0)
                    {
                        //rechtzaak als eerste toevoegen
                        Uow.RechtzaakRepo.Toevoegen(Rechtzaak);
                        Uow.Save();

                        //jury laten genereren
                        Uow.JuryRepo.ToevoegenRange(JuryAanduiden());
                        Uow.Save();


                        //aanklachten instellen
                        foreach (Aanklager aanklager in Aanklagers)
                        {
                            Uow.RechtzaakAanklagerRepo.Toevoegen(new RechtzaakAanklager() { AanklagerID = aanklager.AanklagerID, RechtzaakID = Rechtzaak.RechtzaakID });
                        }
                        foreach (Beklaagde beklaagde in Beklaagdes)
                        {
                            Uow.RechtzaakBeklaagdeRepo.Toevoegen(new RechtzaakBeklaagde() { BeklaagdeID = beklaagde.BeklaagdeID, RechtzaakID = Rechtzaak.RechtzaakID });
                        }

                        int ok = Uow.Save();

                        //hier wel een ok omdat al de rest tot hiertoe dan is gelukt
                        if (ok > 0)
                        {
                            service.ToonMessageBox("Rechtzaak is succesvol aangemaakt!\nDe deelnemende partijen worden vandaag nog op de hoogte gebracht.");
                        }
                    }
                    else
                    {
                        if (service.ToonMessageBoxPlusReturnAntwoord("Bent u zeker dat u klaar bent met het wijzigen?", "Controle"))
                        {


                            Uow.RechtzaakRepo.Aanpassen(Rechtzaak);

                            //aanklachten instellen voor als er een nieuwe persoon is toegevoegd bij het aanpassen van de rechtzaak
                            foreach (Aanklager aanklager in Aanklagers)
                            {
                                var persoon = Uow.RechtzaakAanklagerRepo.Ophalen(x => x.RechtzaakID == Rechtzaak.RechtzaakID && x.AanklagerID == aanklager.AanklagerID).SingleOrDefault();
                                if (persoon == null)
                                {
                                    Uow.RechtzaakAanklagerRepo.Toevoegen(new RechtzaakAanklager() { AanklagerID = aanklager.AanklagerID, RechtzaakID = Rechtzaak.RechtzaakID });
                                }

                            }
                            foreach (Beklaagde beklaagde in Beklaagdes)
                            {
                                var persoon = Uow.RechtzaakBeklaagdeRepo.Ophalen(x => x.RechtzaakID == Rechtzaak.RechtzaakID && x.BeklaagdeID == beklaagde.BeklaagdeID).SingleOrDefault();
                                if (persoon == null)
                                {
                                    Uow.RechtzaakBeklaagdeRepo.Toevoegen(new RechtzaakBeklaagde() { BeklaagdeID = beklaagde.BeklaagdeID, RechtzaakID = Rechtzaak.RechtzaakID });
                                }

                            }
                            int ok = Uow.Save();
                            if (ok > 0)
                            {
                                service.ToonMessageBox("Voltooid!");
                            }
                        }
                    }
                }

            }
        }
        public void BestaandeBeklaagdeToevoegen()
        {
            //gaat uit de combobox de bestaande beklaagde aan de lijst van de datagrid toevoegen na controle op ID
            if (BestaandeBeklaagde.BeklaagdeID > 0)
            {
                VulBeklaagdes(BestaandeBeklaagde);
                //stelt de comboboxen weer in
                ComboboxenInstellen();
            }
            BestaandeBeklaagde = new Beklaagde();
        }

        public void BestaandeAanklagerToevoegen()
        {
            //gaat uit de combobox de bestaande aanklager aan de lijst van de datagrid toevoegen na controle op ID
            if (BestaandeAanklager.AanklagerID > 0)
            {
                VulAanklagers(BestaandeAanklager);
                //stelt de comboboxen weer in
                ComboboxenInstellen();
            }
            BestaandeAanklager = new Aanklager();
        }
        public void Sluiten()
        {
            //checken of het een nieuwe rechtzaak is of een bestaande
            if (Rechtzaak != null && Rechtzaak.RechtzaakID > 0)
            {
                var rechtzaakAanklagers = Uow.RechtzaakAanklagerRepo.Ophalen(x => x.RechtzaakID == Rechtzaak.RechtzaakID);
                var rechtzaakBeklaagdes = Uow.RechtzaakBeklaagdeRepo.Ophalen(x => x.RechtzaakBeklaagdeID == Rechtzaak.RechtzaakID);

                //om te voorkomen dat een rechtzaak per ongeluk bij het sluiten van het scherm zonder beklaagdes of aanklagers komt te zitten
                //het veroorzaakte zo een bug
                if (rechtzaakAanklagers == null || rechtzaakBeklaagdes == null)
                {
                    service.ToonMessageBox("1 van de partijen bevat geen enkele rechtspersoon die aan de rechtzaak is gelinked!\nBewaar eerst de rechtzaak en sluit daarna pas af!");
                }
                else
                {
                    //vragen of het ok is om af te sluiten zonder iets op te slagen
                    if (service.ToonMessageBoxPlusReturnAntwoord("Als u nu afsluit dan gaan niet opgeslagen gewijzigde gegevens verloren\nBent u zeker dat u wil afsluiten?", "controle"))
                    {
                        //venster afsluiten
                        Closer.CloseView(this, new EventArgs());
                    }

                }
            }
            else
            {
                //voor als een rechtzaak nieuw is en er wil afgesloten worden
                if (service.ToonMessageBoxPlusReturnAntwoord("Als u dit scherm sluit gaan de gegevens verloren\nBent u zeker dat u wil afsluiten?", "controle"))
                {
                    //venster afsluiten
                    Closer.CloseView(this, new EventArgs());
                }

            }

        }
        public override bool CanExecute(object parameter)
        {
            switch (parameter.ToString())
            {
                case "Verwijderen":
                    //beschikbaareid van de verwijder knop instellen
                    if (IsSelected)
                    {
                        return true;
                    }
                    return false;
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
                case "PartijBeheren":
                    OpenPartijBeheren("Nieuw");
                    break;
                case "Verwijderen":
                    Verwijderen();
                    break;
                case "BeklaagdeToevoegen":
                    BestaandeBeklaagdeToevoegen();
                    break;
                case "AanklagerToevoegen":
                    BestaandeAanklagerToevoegen();
                    break;
                case "Sluiten":
                    Sluiten();
                    break;
            }
        }
    }
}
