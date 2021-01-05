using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Recht_DAL
{
    [Table("Beklaagdes")]
    public class Beklaagde
    {
        public int BeklaagdeID { get; set; }

        public string Voornaam { get; set; }


        public string Achternaam { get; set; }

    

        [NotMapped]
        public string VolledigeNaam => $"{Voornaam} {Achternaam}";
        public string Gemeente { get; set; }

        public string Straat { get; set; }


        public string HuisNr { get; set; }

        public int Postcode { get; set; }

       //navigatie

        public ICollection<RechtzaakBeklaagde> Aanklachten { get; set; }


    }
}
