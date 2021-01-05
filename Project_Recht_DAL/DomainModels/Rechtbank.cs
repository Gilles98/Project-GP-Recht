using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Recht_DAL
{
    [Table("Rechtbanken")]
    public class Rechtbank
    {
        public int RechtbankID { get; set; }

        public string Naam { get; set; }
        public string Gemeente { get; set; }

        public string Straat { get; set; }

        public string HuisNr { get; set; }

        public int Postcode { get; set; }

        //navigatie
        
        public ICollection<Rechter> Rechters { get; set; }
        public ICollection<Rechtzaak>Rechtzaken { get; set; }

        public override string ToString()
        {
            return Naam.ToString();
        }
    }
}
