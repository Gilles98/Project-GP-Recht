using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Recht_DAL
{
    [Table("Rechter")]
   public class Rechter
    {
        public int RechterID { get; set; }

        public string Voornaam { get; set; }

        public string Achternaam { get; set; }

        public int RechtbankID { get; set; }
        //navigatie
        [NotMapped]
        public string Weergave => $"Rechter: {Voornaam} {Achternaam}";
        public Rechtbank Rechtbank { get; set; }

        public ICollection<Rechtzaak> Rechtzaken { get; set; }

        public override string ToString()
        {
            return Voornaam + " " + Achternaam;  
        }
    }
}
