using Project_Colruyt_WPF.ViewModels;
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
        private IUnitOfWork Uow { get; set; }
        public string erd;

        public override string this[string columnName]
        {

            get
            {
                //if (Rechter.Voornaam == null)
                //{
                //    return "De voornaam van een rechter mag niet een numeriek getal zijn";
                //}
                //if (Rechter.Achternaam == null)
                //{
                //    return "De achternaam van een rechter mag niet een numeriek getal zijn";
                //}

                return "";
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
            SelectedRechtbank = Uow.RechtbankRepo.Ophalen(x => x.RechtbankID == Rechter.RechtbankID).SingleOrDefault();
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }
        public string FoutmeldingInstellen()
        {
            string melding = "";
            if (SelectedRechtbank == null)
            {

            }
            return melding;
        }
        public void Verwijderen()
        {
            ///controle doen of dat de rechter nog rechtzaken heeft!

            if (Rechter.RechterID > 0)
            {
                if (dialog.ToonMessageBoxPlusReturnAntwoord("Deze rechter heeft mogelijk nog rechtzaken. Bent u zeker dat u hem wil verwijderen", "Melding"))
                {
                    Uow.RechterRepo.Verwijderen(Rechter);
                    int ok = Uow.Save();


                    if (ok > 0)
                    {
                        Rechter = new Rechter();
                        SelectedRechtbank = null;
                        action.Invoke();
                    }
                }
            }


        }
        public void Bewaren()
        {


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
                Rechter.RechtbankID = SelectedRechtbank.RechtbankID;
                Uow.RechterRepo.Aanpassen(Rechter);
                int ok = Uow.Save();
                if (ok > 0)
                {
                    action.Invoke();
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
