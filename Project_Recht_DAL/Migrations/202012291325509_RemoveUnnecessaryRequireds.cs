namespace Project_Recht_DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveUnnecessaryRequireds : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Recht.Aanklagers", "Voornaam", c => c.String());
            AlterColumn("Recht.Aanklagers", "Achternaam", c => c.String());
            AlterColumn("Recht.Aanklagers", "Gemeente", c => c.String());
            AlterColumn("Recht.Aanklagers", "Straat", c => c.String());
            AlterColumn("Recht.Aanklagers", "HuisNr", c => c.String());
            AlterColumn("Recht.Rechtbanken", "Naam", c => c.String());
            AlterColumn("Recht.Rechtbanken", "Gemeente", c => c.String());
            AlterColumn("Recht.Rechtbanken", "Straat", c => c.String());
            AlterColumn("Recht.Rechtbanken", "HuisNr", c => c.String());
            AlterColumn("Recht.Rechter", "Voornaam", c => c.String());
            AlterColumn("Recht.Rechter", "Achternaam", c => c.String());
            AlterColumn("Recht.Beklaagdes", "Voornaam", c => c.String());
            AlterColumn("Recht.Beklaagdes", "Achternaam", c => c.String());
            AlterColumn("Recht.Beklaagdes", "Gemeente", c => c.String());
            AlterColumn("Recht.Beklaagdes", "Straat", c => c.String());
            AlterColumn("Recht.Beklaagdes", "HuisNr", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("Recht.Beklaagdes", "HuisNr", c => c.String(nullable: false));
            AlterColumn("Recht.Beklaagdes", "Straat", c => c.String(nullable: false));
            AlterColumn("Recht.Beklaagdes", "Gemeente", c => c.String(nullable: false));
            AlterColumn("Recht.Beklaagdes", "Achternaam", c => c.String(nullable: false));
            AlterColumn("Recht.Beklaagdes", "Voornaam", c => c.String(nullable: false));
            AlterColumn("Recht.Rechter", "Achternaam", c => c.String(nullable: false));
            AlterColumn("Recht.Rechter", "Voornaam", c => c.String(nullable: false));
            AlterColumn("Recht.Rechtbanken", "HuisNr", c => c.String(nullable: false));
            AlterColumn("Recht.Rechtbanken", "Straat", c => c.String(nullable: false));
            AlterColumn("Recht.Rechtbanken", "Gemeente", c => c.String(nullable: false));
            AlterColumn("Recht.Rechtbanken", "Naam", c => c.String(nullable: false));
            AlterColumn("Recht.Aanklagers", "HuisNr", c => c.String(nullable: false));
            AlterColumn("Recht.Aanklagers", "Straat", c => c.String(nullable: false));
            AlterColumn("Recht.Aanklagers", "Gemeente", c => c.String(nullable: false));
            AlterColumn("Recht.Aanklagers", "Achternaam", c => c.String(nullable: false));
            AlterColumn("Recht.Aanklagers", "Voornaam", c => c.String(nullable: false));
        }
    }
}
