using Project_Recht_DAL;
using Project_Recht_DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Recht.Static
{
    //deze klasse gaat bij alles helpen waar een persoon aan gebonden is bij een rechtzaak
    public delegate void BeklaagdesDelegate(Beklaagde beklaagde);

    public delegate void AanklagersDelegate(Aanklager aanklager);
    public delegate void PersoonUpdateDelegate(object persoon);
    public static class StaticHelper
    {

        public static event PersoonUpdateDelegate UpdateLijst;
        public static event BeklaagdesDelegate Beklaagdes;

        public static event AanklagersDelegate Aanklagers;

        public static event Action<object> LijstInstellen;


        public static void LijstAanpassen( object nieuwePersoon)
        {
            UpdateLijst?.Invoke(nieuwePersoon);
        }
        public static void Updaten(object persoon)
        {
            LijstInstellen?.Invoke(persoon);
        }

        public static void KrijgBeklaagdes(Beklaagde beklaagde)
        {
            Beklaagdes?.Invoke(beklaagde);
        }

        public static void KrijgAanklagers(Aanklager aanklager)
        {
            Aanklagers?.Invoke(aanklager);
        }
    }
}
