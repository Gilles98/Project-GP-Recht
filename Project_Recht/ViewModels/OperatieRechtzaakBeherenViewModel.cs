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
        private bool _enableSelectionRechter;
        private DateTime _selectedDateTime;
        private object _selectedPersoon;
        public bool IsSelected { get; set; }
        private IUnitOfWork Uow { get; set; }

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
                NotifyPropertyChanged();
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
                NotifyPropertyChanged();
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
                NotifyPropertyChanged();
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
                NotifyPropertyChanged();
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
                NotifyPropertyChanged();
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
                NotifyPropertyChanged();
            }
        }

        public void EnableRechters()
        {

            if (SelectedRechtbank != null)
            {
                EnableSelectionRechter = true;
                Rechters = new ObservableCollection<Rechter>(Uow.RechterRepo.Ophalen(x => x.RechtbankID == SelectedRechtbank.RechtbankID));
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
                NotifyPropertyChanged();
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
                NotifyPropertyChanged();
            }
        }

        public void OpenPartijBeheren(string keuze)
        {
            Partijbeheren view = new Partijbeheren();

            ///omdat het verwijderen en het wijzigen van een persoon wel in een ander scherm gebeurt doe ik 2x showdialog
            ///om te vermijden dat bij het aanmaken de persoon naar de database wordt gezet terwijl je nog zou kunnen wegklikken
            ///van het scherm waar dit viewmodel aan gelinked is

            if (keuze == "Nieuw")
            {
                view.DataContext = new OperatiePartijBeherenViewModel();
                view.ShowDialog();
            }
            else
            {
                if (SelectedPersoon is Aanklager || SelectedPersoon is Beklaagde)
                {
                    view.DataContext = new OperatiePartijBeherenViewModel(SelectedPersoon, Uow);
                    SelectedPersoon = new object();
                    view.ShowDialog();

                }
            }
        }


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
            this.Uow = uow;
            Beklaagdes = new ObservableCollection<Beklaagde>();
            Aanklagers = new ObservableCollection<Aanklager>();
            Rechtbanken = new ObservableCollection<Rechtbank>(Uow.RechtbankRepo.Ophalen());
            SelectedDateTime = DateTime.Now;
            //om fout te voorkomen
            Rechters = new ObservableCollection<Rechter>();
            Rechtzaak = new Rechtzaak();
            StaticPersoon.Aanklagers += VulAanklagers;
            StaticPersoon.Beklaagdes += VulBeklaagdes;

            StaticPersoon.LijstInstellen += LijstenInstellen;

        }

        /// bij constructor met ID en na het verwijderen of wijzigen van een persoon
        public void LijstenInstellen()
        {
            var rechtzaakAanklagers = Uow.RechtzaakAanklagerRepo.Ophalen(x => x.RechtzaakID == Rechtzaak.RechtzaakID);
            var rechtzaakBeklaagdes = Uow.RechtzaakBeklaagdeRepo.Ophalen(x => x.RechtzaakID == Rechtzaak.RechtzaakID);

            Aanklagers = new ObservableCollection<Aanklager>();
            foreach (var item in rechtzaakAanklagers)
            {
                Aanklagers.Add(Uow.AanklagerRepo.Ophalen(x => x.AanklagerID == item.AanklagerID).SingleOrDefault());
            }
            Beklaagdes = new ObservableCollection<Beklaagde>();
            foreach (var item in rechtzaakBeklaagdes)
            {
                Beklaagdes.Add(Uow.BeklaagdeRepo.Ophalen(x => x.BeklaagdeID == item.BeklaagdeID).SingleOrDefault());
            }
        }
        public OperatieRechtzaakBeherenViewModel(IUnitOfWork uow, bool selected, int id)
        {
            IsSelected = selected;
            this.Uow = uow;
            Rechtzaak = Uow.RechtzaakRepo.ZoekOpPK(id);

            LijstenInstellen();
            
            Rechtbanken = new ObservableCollection<Rechtbank>(Uow.RechtbankRepo.Ophalen());
            SelectedDateTime = Rechtzaak.Moment;
            SelectedRechtbank = Uow.RechtbankRepo.ZoekOpPK(Rechtzaak.RechtbankID);

            Klacht = Rechtzaak.OmschrijvingKlacht;  
            SelectedRechter = uow.RechterRepo.ZoekOpPK(Rechtzaak.RechterID);

            StaticPersoon.Aanklagers += VulAanklagers;
            StaticPersoon.Beklaagdes += VulBeklaagdes;

            StaticPersoon.LijstInstellen += LijstenInstellen;
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
            if (SelectedDateTime != DateTime.Now.Date)
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
                    int beschikbareDatum = (int)(DateTime.Now.Date.AddDays(3) - SelectedDateTime.Date).TotalDays;
                    if (beschikbareDatum < 3 && beschikbareDatum > 0)
                    {
                        service.ToonMessageBox("het is niet mogenlijk om een rechtzaak volledig te organiseren binnen de 3 dagen");
                        SelectedDateTime = DateTime.Now.Date;
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
                for (int i = 0; i <= leden.Count -1; i++)
                {
                    int check = random.Next(5);
                    if (check == 3)
                    {
                        Jury juryLid = new Jury() { JurylidID = leden[i].JurylidID, RechtzaakID = Rechtzaak.RechtzaakID };
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

        public string GenereerCodeRechtzaak()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 7).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public void VulAanklagers(Aanklager aanklager)
        {
            Aanklagers.Add(aanklager);
        }

        public void VulBeklaagdes(Beklaagde beklaagde)
        {
            Beklaagdes.Add(beklaagde);
        }


        public void RechtzaakInstellen()
        {
            Rechtzaak.Code = GenereerCodeRechtzaak();
            Rechtzaak.RechtbankID = SelectedRechtbank.RechtbankID;
            Rechtzaak.RechterID = SelectedRechter.RechterID;
            Rechtzaak.Moment = SelectedDateTime;
            Rechtzaak.OmschrijvingKlacht = Klacht;
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
                
            }
        }


        public void Bewaren()
        {
            if (this.IsGeldig())
            {

                if (Beklaagdes.Count == 0 || Aanklagers.Count == 0)
                {
                    service.ToonMessageBox("Een rechtbank is pas legitiem als beide partijen zijn opgevuld met minstens 1 persoon");
                }
                else
                {
                    RechtzaakInstellen();
                    if (Rechtzaak.RechtzaakID <= 0)
                    {
                        //rechtzaak als eerste toevoegen
                        Uow.RechtzaakRepo.Toevoegen(Rechtzaak);
                        Uow.Save();
                        ///beklaagdes en aanklagers toevoegen
                        Uow.AanklagerRepo.ToevoegenRange(Aanklagers);
                        Uow.BeklaagdeRepo.ToevoegenRange(Beklaagdes);
                        Uow.Save();
                        ///jury laten genereren
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
                        if (ok > 0)
                        {
                            service.ToonMessageBox("Rechtzaak is succesvol aangemaakt!\n De deelnemende partijen worden vandaag nog op de hoogte gebracht.");
                        }
                    }
                    else
                    {
                        Uow.RechtzaakRepo.Aanpassen(Rechtzaak);
                        Uow.Save();
                        service.ToonMessageBox("Rechtzaak succesvol aangepast!");
                    }
                }

            }
        }
        

        public override bool CanExecute(object parameter)
        {
            switch (parameter.ToString())
            {
                case "Verwijderen":
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
                default:
                    break;
            }
        }
    }
}
