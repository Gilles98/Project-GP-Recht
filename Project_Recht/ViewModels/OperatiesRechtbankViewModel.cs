using Project_Colruyt_WPF.ViewModels;
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
        private readonly Action action;
        private IUnitOfWork Uow { get; set; }
        public string erd;

        private string _naam;
        private string _gemeente;
        private string _straat;
        private string _huisnummer;
        private int _postcode;
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
                    
                    for (int i = 0; i <= lijst.Count -1; i++)
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

        public string Naam
        {
            get
            {
                return _naam;
            }
            set
            {
                _naam = value;
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

        public void RechtbankInstellen()
        {
            Rechtbank = new Rechtbank();
            Rechtbank.Naam = Naam;
            Rechtbank.HuisNr = Huisnummer;
            Rechtbank.Postcode = Postcode;
            Rechtbank.Straat = Straat;
            Rechtbank.Gemeente = Gemeente;
        }

        public void Reset()
        {
            Rechtbank = new Rechtbank();
            Naam = null;
            Huisnummer = null;
            Postcode = 0;
            Straat = null;
            Gemeente = null;
        }

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
                NotifyPropertyChanged();
            }
        }


        public OperatiesRechtbankViewModel(IUnitOfWork unitOfWork, Action parentAction)
        {
            this.action = parentAction;
            Uow = unitOfWork;
        }

        public OperatiesRechtbankViewModel(int id, IUnitOfWork unitOfWork, Action parentAction)
        {

            this.action = parentAction;
            this.Uow = unitOfWork;
            Rechtbank = Uow.RechtbankRepo.ZoekOpPK(id);
            PropertiesInstellen();
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }
        //public string FoutmeldingInstellen()
        //{
           
        //}
        public void Verwijderen()
        {
           

            if (Rechtbank.RechtbankID > 0)
            {
                if (Rechtbank.Rechters.Count > 0)
                {
                    if (dialog.ToonMessageBoxPlusReturnAntwoord("Deze rechtbank bevat nog rechters en rechtzaken, bent u zeker dat u deze wil verwijderen?", "Melding"))
                    {
                        Uow.RechterRepo.VerwijderenRange(Rechtbank.Rechters);
                        Uow.RechtbankRepo.Verwijderen(Rechtbank);
    
                        int ok = Uow.Save();
                        if (ok > 0)
                        {
                            action.Invoke();
                        }
                    }
                }
                else
                {
                    Uow.RechtbankRepo.Verwijderen(Rechtbank);
                    int ok = Uow.Save();
                    if (ok > 0)
                    {
                       
                        action.Invoke();
                    }
                }    
            }
            Reset();

        }
        public void Bewaren()
        {
            if (this.IsGeldig())
            {


                RechtbankInstellen();
                if (Rechtbank.RechtbankID <= 0)
            {
                

                if (Rechtbank.Naam != "")
                {
                    if (Rechtbank.Gemeente != "")
                    {
                        Uow.RechtbankRepo.Toevoegen(Rechtbank);
                        int ok = Uow.Save();
                        if (ok > 0)
                        {
                            action.Invoke();
                        }
                    }

                }
            }
            else
            {
                Uow.RechtbankRepo.Aanpassen(Rechtbank);
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
