using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Recht_DAL
{
    [Table("Aanklachten")]
    public class Aanklacht
    {
        public int AanklachtID { get; set; }

        [Index("IX_BeklaagdeIDAanklagerIDRechtzaakID", 1, IsUnique =true)]
        public int BeklaagdeID { get; set; }

        [Index("IX_BeklaagdeIDAanklagerIDRechtzaakID", 2, IsUnique = true)]
        public int AanklagerID { get; set; }

        [Index("IX_BeklaagdeIDAanklagerIDRechtzaakID", 3, IsUnique = true)]
        public int RechtzaakID { get; set; }

        [MaxLength(100, ErrorMessage = "Een omschrijving is verplicht!")]
        public string OmschrijvingKlacht { get; set; }

        //navigatie
        public Aanklager Aanklager { get; set; }
        public Beklaagde Beklaagde { get; set; }

        public ICollection<Rechtzaak>Rechtzaken { get; set; }
    }
}
