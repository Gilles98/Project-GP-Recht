using Project_Recht.Service;
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
    public class DetailsViewModel : Basis
    {
        private string _voornaam;
        private string _achternaam;
        IDialog dialog = new Dialog();
        private IUnitOfWork Uow { get; set; }
        private ObservableCollection<Jurylid> _leden;
        private Jurylid _jurylid;
        public List<Jury> Jury
        {
            get;
            set;
        }
        public Jurylid Jurylid
        {
            get
            {
                return _jurylid;
            }
            set
            {
                _jurylid = value;
                Selected();

            }
        }

        public ObservableCollection<Jurylid> Leden
        {
            get
            {
                return _leden;
            }
            set
            {
                _leden = value;

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
        public Rechtzaak Rechtzaak { get; set; } = new Rechtzaak();

        //gaat de juryleden instellen aan de hand van een id en voegt deze toe aan de property leden
        //zet ook al een nieuw jurylid klaar
        public void JuryInstellen(int id)
        {
            Jury = Uow.JuryRepo.Ophalen(x => x.RechtzaakID == id).ToList();
            Leden = new ObservableCollection<Jurylid>();
            foreach (var jurylid in Jury)
            {
                Leden.Add(Uow.JurylidRepo.ZoekOpPK(jurylid.JurylidID));
            }
            Jurylid = new Jurylid();
        }

   

        public DetailsViewModel(IUnitOfWork uow,int id)
        {
            this.Uow = uow;
            Rechtzaak = Uow.RechtzaakRepo.Ophalen(x => x.RechtzaakID == id, includes: x => x.Rechter).SingleOrDefault();
            JuryInstellen(id);
        }

        public void Selected()
        {
            if (Jurylid != null)
            {
                Voornaam = Jurylid.Voornaam;
                Achternaam = Jurylid.Achternaam;
            }
        }
        public override string this[string columnName] {

            get
            {
                string melding = "* Verplicht veld";
                if (columnName =="Voornaam" && string.IsNullOrWhiteSpace(Voornaam))
                {
                    return melding;
                }

                if (columnName == "Achternaam" && string.IsNullOrWhiteSpace(Achternaam))
                {
                    return melding;
                }
                return "";
            }
        }

        public void Bewaren()
        {
            if (this.IsGeldig())
            {
                if (Jurylid.JurylidID <= 0 )
                {
                    Jurylid lid = new Jurylid() { Achternaam = Achternaam, Voornaam = Voornaam };
                    Uow.JurylidRepo.Toevoegen(lid);
                    int ok = Uow.Save();
                    if (ok > 0)
                    {
                        if (dialog.ToonMessageBoxPlusReturnAntwoord("Wilt u het pas aangemaakte jurylid aan deze rechtzaak toevoegen?", "Toevoegen?"))
                        {
                            Uow.JuryRepo.Toevoegen(new Jury() { RechtzaakID = Rechtzaak.RechtzaakID, JurylidID = lid.JurylidID });
                            lid.Opgeroepen = true;
                            Uow.JurylidRepo.Aanpassen(lid);
                            Uow.Save();
                            Leden.Add(lid);
                           
                        }
                        dialog.ToonMessageBox("Acties waren succesvol!");
                    }
                    Jurylid = new Jurylid();

                }
                else
                {
                    string volledigeNaam = Jurylid.VolledigeNaam;
                    Jurylid.Voornaam = Voornaam;
                    Jurylid.Achternaam = Achternaam;
                    if (volledigeNaam != Jurylid.VolledigeNaam)
                    {
                        Uow.JurylidRepo.Aanpassen(Jurylid);
                        int ok = Uow.Save();
                        if (ok > 0)
                        {
                            dialog.ToonMessageBox("Aanpassing was succesvol! Instellingen worden gereset.");
                            //hergebruik methode
                            JuryInstellen(Rechtzaak.RechtzaakID);
                        }
                    }
                    else
                    {
                        dialog.ToonMessageBox("Eerst iets werkelijk veranderen!");
                    }
                    
                }
            }
           
        }
        public void Verwijderen()
        {
            if (Leden.Count < 4)
            {
                dialog.ToonMessageBox("Er moeten minstens 3 persoon in de jury aanwezig blijven! Voeg eerst een jurlid toe!");
                Jurylid = new Jurylid();
                Voornaam = "";
                Achternaam = "";
            }
            else
            {
                if (Jurylid.JurylidID > 0)
                {
                    Uow.JuryRepo.Verwijderen(Uow.JuryRepo.Ophalen(x => x.JurylidID == Jurylid.JurylidID).SingleOrDefault());
                    if (dialog.ToonMessageBoxPlusReturnAntwoord("Wilt u het jurylid ook verwijderen uit de database en niet alleen uit de rechtzaak?", "Kies"))
                    {
                        Uow.JurylidRepo.Verwijderen(Jurylid);
                    }
                    else ///status aanpassen voor als het jurylid wel nog in de database mag bestaan zodat deze opnieuw kan worden aangewezen aan een rechtzaak bij het aanmaken van een rechtzaak
                    {
                        Jurylid.Opgeroepen = false;
                        Uow.JurylidRepo.Aanpassen(Jurylid);
                    }
                    int ok = Uow.Save();
                    if (ok > 0)
                    {
                        dialog.ToonMessageBox("Verwijdering voltooid!");
                        JuryInstellen(Rechtzaak.RechtzaakID);
                    }
                    
                }
                else
                {
                    dialog.ToonMessageBox("Er is geen jurylid geselecteerd!");
                }
            }
        }

        public override bool CanExecute(object parameter)
        {
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
