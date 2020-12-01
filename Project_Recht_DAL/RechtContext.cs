using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Project_Recht_DAL
{
    public class RechtContext:DbContext
    {
        public RechtContext(): base("name=ProjectRecht")
        {

        }

        public DbSet<RechtzaakAanklager>RechtzaakAanklager { get; set; }
        public DbSet<Aanklager> Aanklager { get; set; }
        public DbSet<Beklaagde> Beklaagdes { get; set; }
        public DbSet<Rechtzaak> Rechtzaken { get; set; }
        public DbSet<Rechtbank> Rechtbanken { get; set; }
        public DbSet<Jury> Jury { get; set; }

        public DbSet<Jurylid> Juryleden { get; set; }
        public DbSet<Rechter> Rechters { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Recht");
            base.OnModelCreating(modelBuilder);

            // vanwege Volgende cascade melding moet ik dit met fluent oplossen
            // Introducing FOREIGN KEY constraint 'FK_dbo.Rechter_dbo.Rechtbanken_RechtbankID' on table 'Rechter' may cause cycles or multiple cascade paths. 
            // Specify ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints.
            // Could not create constraint or index. See previous errors.
            modelBuilder.Entity<Rechter>().HasRequired(x => x.Rechtbank).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<Rechter>().HasRequired(x => x.Rechtbank).WithMany(x => x.Rechters).HasForeignKey(x => x.RechtbankID);
        }
    }
}
