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

        [Required(ErrorMessage = "Naam is een verplicht veld!")]
        public string Naam { get; set; }

        [Required(ErrorMessage = "Gemeente is een verplicht veld!")]
        public string Gemeente { get; set; }

        [Required(ErrorMessage = "Straat is een verplicht veld!")]

        public string Straat { get; set; }

        [Required(ErrorMessage = "Husinummer is een verplicht veld!")]
        public string HuisNr { get; set; }

        [Required(ErrorMessage = "Postcode is een verplicht veld!")]
        public int Postcode { get; set; }

        //navigatie
        
        public ICollection<Rechter> Rechters { get; set; }
        public ICollection<Rechtzaak>Rechtzaken { get; set; }
    }
}
