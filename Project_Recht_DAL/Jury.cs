using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Recht_DAL
{
    [Table("Jurys")]
    public class Jury
    {
        public int JuryID { get; set; }

        [Index("IX_JurylidIDRechtzaakID", 1, IsUnique =true)]
        public int JurylidID { get; set; }

        [Index("IX_JurylidIDRechtzaakID", 2, IsUnique = true)]
        public int RechtzaakID { get; set;}


        //navigatie

        public Rechtzaak Rechtzaak { get; set; }
        public Jurylid Jurylid { get; set; }
    }
}
