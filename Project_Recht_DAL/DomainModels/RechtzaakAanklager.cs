using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Recht_DAL
{
    [Table("RechtzaakAanklager")]
    public class RechtzaakAanklager
    {
        public int RechtzaakAanklagerID { get; set; }

        [Index("IX_AanklagerIDRechtzaakID", 1, IsUnique = true)]
        public int AanklagerID { get; set; }

        [Index("IX_AanklagerIDRechtzaakID", 2, IsUnique = true)]
        public int RechtzaakID { get; set; }
        //navigatie
        public Aanklager Aanklager { get; set; }


        public Rechtzaak Rechtzaak { get; set; }
    }
}
