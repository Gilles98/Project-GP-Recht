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
    public delegate void BeklaagdesDelegate(Beklaagde beklaagde);

    public delegate void AanklagersDelegate(Aanklager aanklager);
    public static class StaticPersoon
    {
        public static List<Aanklager> StaticAanklagers { get; set; } = new List<Aanklager>();
        public static List<Beklaagde> StaticBeklaagdes { get; set; } = new List<Beklaagde>();

        public static event BeklaagdesDelegate Beklaagdes;

        public static event AanklagersDelegate Aanklagers;

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
