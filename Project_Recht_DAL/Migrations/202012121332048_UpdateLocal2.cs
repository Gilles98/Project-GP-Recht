namespace Project_Recht_DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateLocal2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Recht.Juryleden", "Voornaam", c => c.String());
            AlterColumn("Recht.Juryleden", "Achternaam", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("Recht.Juryleden", "Achternaam", c => c.String(nullable: false));
            AlterColumn("Recht.Juryleden", "Voornaam", c => c.String(nullable: false));
        }
    }
}
