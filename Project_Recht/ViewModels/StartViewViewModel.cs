
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
    public class StartViewViewModel : Basis
    {

        public override string this[string columnName] => throw new NotImplementedException();

        public void OpenRechtzakenBeheren()
        {
            //view instellen
            ViewUserControl view = new ViewUserControl();

            RechtzakenBeherenViewModel viewModel = new RechtzakenBeherenViewModel();
            view.DataContext = viewModel;
            view.ShowDialog();
        }
        public void ControleRechtzaken()
        {
            IUnitOfWork unitOfWork = new UnitOfWork(new RechtContext());
            var zaken = unitOfWork.RechtzaakRepo.Ophalen(x => x.Moment <= DateTime.Now);
            //kijken of er in zaken een zaak zit
            if (zaken.Count() > 0)
            {
                foreach (var item in zaken)
                {
                    //jury van de rechtzaak ophalen
                    var jury = unitOfWork.JuryRepo.Ophalen(x => x.RechtzaakID == item.RechtzaakID);
                    foreach (Jury personen in jury)
                    {
                        //leden ophalen van de jury
                        var leden = unitOfWork.JurylidRepo.Ophalen(x => x.JurylidID == personen.JurylidID);
                        foreach (Jurylid lid in leden)
                        {
                            //status lid aanpassen
                            lid.Opgeroepen = false;
                            unitOfWork.JurylidRepo.Aanpassen(lid);
                        }
                        //manuele cascade tussentabel jury 
                        unitOfWork.JuryRepo.Verwijderen(personen);
                    }
                    //manuele cascade tussentabellen RechtzaakAanklagers & RechtzaakBeklaagdes
                    var rechtzaakAanklagers = unitOfWork.RechtzaakAanklagerRepo.Ophalen(x => x.RechtzaakID == item.RechtzaakID);
                    var rechtzaakBeklaagdes = unitOfWork.RechtzaakBeklaagdeRepo.Ophalen(x => x.RechtzaakID == item.RechtzaakID);
                    unitOfWork.RechtzaakAanklagerRepo.VerwijderenRange(rechtzaakAanklagers);
                    unitOfWork.RechtzaakBeklaagdeRepo.VerwijderenRange(rechtzaakBeklaagdes);
                    unitOfWork.Save();

                    //rechtzaak achteraf verwijderen om foutmelding cascade te voorkomen
                    unitOfWork.RechtzaakRepo.Verwijderen(item);
                    unitOfWork.Save();
                }
            }
        }
        public StartViewViewModel() 
        {
            //alle rechtzaken checken op datum om de gepasseerde rechtzaken en die op de huidige datum vallen te verwijderen 
            ControleRechtzaken();
        }

        public void OpenRechtbanken()
        {
            ViewUserControl view = new ViewUserControl();

            RechtbankenRechtersViewModel viewModel = new RechtbankenRechtersViewModel();
            view.DataContext = viewModel;
            view.ShowDialog();
        }
        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            switch (parameter.ToString())
            {
                case "Rechtzaken":
                    OpenRechtzakenBeheren();
                    break;
                case "RechtersRechtbanken":
                    OpenRechtbanken();
                    break;
                default:
                    break;
            }
        }
    }
}
