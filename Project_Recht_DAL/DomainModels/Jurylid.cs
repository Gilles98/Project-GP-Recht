using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Recht_DAL
{
    [Table("Juryleden")]
    public class Jurylid
    {
        public int JurylidID { get; set; }

        public string Voornaam { get; set; }

        public string Achternaam { get; set; }

        [NotMapped]
        public string VolledigeNaam => $"{Voornaam} {Achternaam}";
        public Boolean Opgeroepen { get; set; }

        //navigatie

        public ICollection<Jury>Jury { get; set; }
    }
}
