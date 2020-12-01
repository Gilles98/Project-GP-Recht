using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Recht_DAL
{
    [Table("Rechtzaken")]
    public class Rechtzaak
    {
        public int RechtzaakID { get; set; }

        public DateTime Moment { get; set; }

        public string Code { get; set; }

        public int RechtbankID { get; set; }

        public string OmschrijvingKlacht { get; set; }

        public int RechterID { get; set; }

        //navigatie

        public Rechter Rechter { get; set; }

        public Rechtbank Rechtbank { get; set; }
        
        public ICollection<Jury>Jury { get; set; }

        public ICollection<RechtzaakAanklager> RechtzaakAanklagers { get; set; }
        public ICollection<RechtzaakBeklaagde>RechtzaakBeklaagdes { get; set; }
    }
}
