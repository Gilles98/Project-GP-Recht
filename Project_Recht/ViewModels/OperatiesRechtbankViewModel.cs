﻿using Project_Colruyt_WPF.ViewModels;
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
            Rechtbank = new Rechtbank();
        }

        public OperatiesRechtbankViewModel(int id, IUnitOfWork unitOfWork, Action parentAction)
        {

            this.action = parentAction;
            this.Uow = unitOfWork;
            Rechtbank = Uow.RechtbankRepo.ZoekOpPK(id);
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
            ///controle doen of dat de rechtbank nog rechtzaken heeft en rechters!

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
                            Rechtbank = new Rechtbank();
                            action.Invoke();
                        }
                    }
                }
                

              
            }


        }
        public void Bewaren()
        {

            //validatie extra toevoegen
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
