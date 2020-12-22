
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
            ViewUserControl view = new ViewUserControl();

            RechtzakenBeherenViewModel viewModel = new RechtzakenBeherenViewModel();
            view.DataContext = viewModel;
            view.ShowDialog();
        }

        public StartViewViewModel() 
        {
            IUnitOfWork unitOfWork = new UnitOfWork(new RechtContext());
            //al rechtzaken controleren op datum om te kijken of deze gepasseerd zijn en dus verwijderen
            var afgelopenZaken = unitOfWork.RechtzaakRepo.Ophalen(x => x.Moment < DateTime.Now);
            if (afgelopenZaken.Count() > 0)
            {
                foreach (var item in afgelopenZaken)
                {
                    var jury = unitOfWork.JuryRepo.Ophalen(x => x.RechtzaakID == item.RechtzaakID);
                    foreach (Jury personen in jury)
                    {
                        var leden = unitOfWork.JurylidRepo.Ophalen(x => x.JurylidID == personen.JurylidID);
                        foreach (Jurylid lid in leden)
                        {
                            lid.Opgeroepen = false;
                            unitOfWork.JurylidRepo.Aanpassen(lid);
                        }
                        //manuele cascade
                        unitOfWork.JuryRepo.Verwijderen(personen);
                    }
                    //manuele cascade
                    var rechtzaakAanklagers = unitOfWork.RechtzaakAanklagerRepo.Ophalen(x => x.RechtzaakID == item.RechtzaakID);
                    var rechtzaakBeklaagdes = unitOfWork.RechtzaakBeklaagdeRepo.Ophalen(x => x.RechtzaakID == item.RechtzaakID);
                    unitOfWork.RechtzaakAanklagerRepo.VerwijderenRange(rechtzaakAanklagers);
                    unitOfWork.RechtzaakBeklaagdeRepo.VerwijderenRange(rechtzaakBeklaagdes);
                    unitOfWork.Save();
                    unitOfWork.RechtzaakRepo.Verwijderen(item);
                    unitOfWork.Save();
                }
            }
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
