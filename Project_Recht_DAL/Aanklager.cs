using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Recht_DAL
{
    [Table("Aanklagers")]
    public class Aanklager
    {
        public int AanklagerID { get; set; }
        [Required(ErrorMessage = "Voornaam is verplicht!")]
        public string Voornaam { get; set; }

        [Required(ErrorMessage = "Achternaam is verplicht!")]

        public string Achternaam { get; set; }

        [Required(ErrorMessage = "Gemeente is verplicht!")]

        public string Gemeente { get; set; }

        [Required(ErrorMessage = "Straat is verplicht!")]

        public string Straat { get; set; }

        [Required(ErrorMessage = "Huisnummer is verplicht!")]

        public string HuisNr { get; set; }

        [Required(ErrorMessage = "Postcode is verplicht!")]
        public int Postcode { get; set; }

        //navigatie

        public ICollection<RechtzaakAanklager> Aanklachten { get; set; }
    }
}
