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

        [Required(ErrorMessage = "Voornaam is verplicht!")]
        public string Voornaam { get; set; }

        [Required(ErrorMessage = "Voornaam is verplicht!")]
        public string Achternaam { get; set; }

        public Boolean Opgeroepen { get; set; }

        //navigatie

        public ICollection<Jury>Jury { get; set; }
    }
}
