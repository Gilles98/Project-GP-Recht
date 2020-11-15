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

        [Required(ErrorMessage = "Voornaam is een verplicht veld!")]
        public string Voornaam { get; set; }

        [Required(ErrorMessage = "Achternaam is een verplicht veld!")]
        public string Achternaam { get; set; }
       
        public int RechtbankID { get; set; }
        //navigatie

        public Rechtbank Rechtbank { get; set; }

        public ICollection<Rechtzaak> Rechtzaken { get; set; }

    }
}
