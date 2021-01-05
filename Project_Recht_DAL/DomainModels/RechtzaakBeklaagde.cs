using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Recht_DAL
{
    [Table("RechtzaakBeklaagde")]
    public class RechtzaakBeklaagde
    {
        public int RechtzaakBeklaagdeID { get; set; }

        [Index("IX_BeklaagdeIDRechtzaakID", 1, IsUnique = true)]
        public int BeklaagdeID { get; set; }

        [Index("IX_BeklaagdeIDRechtzaakID", 2, IsUnique = true)]
        public int RechtzaakID { get; set; }
        //navigatie
        public Beklaagde Beklaagde { get; set; }


        public Rechtzaak Rechtzaak { get; set; }
    }
}
