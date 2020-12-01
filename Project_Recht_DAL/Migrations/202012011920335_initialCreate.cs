namespace Project_Recht_DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Recht.Aanklagers",
                c => new
                    {
                        AanklagerID = c.Int(nullable: false, identity: true),
                        Voornaam = c.String(nullable: false),
                        Achternaam = c.String(nullable: false),
                        Gemeente = c.String(nullable: false),
                        Straat = c.String(nullable: false),
                        HuisNr = c.String(nullable: false),
                        Postcode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AanklagerID);
            
            CreateTable(
                "Recht.RechtzaakAanklager",
                c => new
                    {
                        RechtzaakAanklagerID = c.Int(nullable: false, identity: true),
                        AanklagerID = c.Int(nullable: false),
                        RechtzaakID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RechtzaakAanklagerID)
                .ForeignKey("Recht.Aanklagers", t => t.AanklagerID, cascadeDelete: true)
                .ForeignKey("Recht.Rechtzaken", t => t.RechtzaakID, cascadeDelete: true)
                .Index(t => new { t.AanklagerID, t.RechtzaakID }, unique: true, name: "IX_AanklagerIDRechtzaakID");
            
            CreateTable(
                "Recht.Rechtzaken",
                c => new
                    {
                        RechtzaakID = c.Int(nullable: false, identity: true),
                        Moment = c.DateTime(nullable: false),
                        Code = c.String(),
                        RechtbankID = c.Int(nullable: false),
                        OmschrijvingKlacht = c.String(),
                        RechterID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RechtzaakID)
                .ForeignKey("Recht.Rechter", t => t.RechterID, cascadeDelete: true)
                .ForeignKey("Recht.Rechtbanken", t => t.RechtbankID, cascadeDelete: true)
                .Index(t => t.RechtbankID)
                .Index(t => t.RechterID);
            
            CreateTable(
                "Recht.Jurys",
                c => new
                    {
                        JuryID = c.Int(nullable: false, identity: true),
                        JurylidID = c.Int(nullable: false),
                        RechtzaakID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JuryID)
                .ForeignKey("Recht.Juryleden", t => t.JurylidID, cascadeDelete: true)
                .ForeignKey("Recht.Rechtzaken", t => t.RechtzaakID, cascadeDelete: true)
                .Index(t => new { t.JurylidID, t.RechtzaakID }, unique: true, name: "IX_JurylidIDRechtzaakID");
            
            CreateTable(
                "Recht.Juryleden",
                c => new
                    {
                        JurylidID = c.Int(nullable: false, identity: true),
                        Voornaam = c.String(nullable: false),
                        Achternaam = c.String(nullable: false),
                        Opgeroepen = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.JurylidID);
            
            CreateTable(
                "Recht.Rechtbanken",
                c => new
                    {
                        RechtbankID = c.Int(nullable: false, identity: true),
                        Naam = c.String(nullable: false),
                        Gemeente = c.String(nullable: false),
                        Straat = c.String(nullable: false),
                        HuisNr = c.String(nullable: false),
                        Postcode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RechtbankID);
            
            CreateTable(
                "Recht.Rechter",
                c => new
                    {
                        RechterID = c.Int(nullable: false, identity: true),
                        Voornaam = c.String(nullable: false),
                        Achternaam = c.String(nullable: false),
                        RechtbankID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RechterID)
                .ForeignKey("Recht.Rechtbanken", t => t.RechtbankID)
                .Index(t => t.RechtbankID);
            
            CreateTable(
                "Recht.RechtzaakBeklaagde",
                c => new
                    {
                        RechtzaakBeklaagdeID = c.Int(nullable: false, identity: true),
                        BeklaagdeID = c.Int(nullable: false),
                        RechtzaakID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RechtzaakBeklaagdeID)
                .ForeignKey("Recht.Beklaagdes", t => t.BeklaagdeID, cascadeDelete: true)
                .ForeignKey("Recht.Rechtzaken", t => t.RechtzaakID, cascadeDelete: true)
                .Index(t => new { t.BeklaagdeID, t.RechtzaakID }, unique: true, name: "IX_BeklaagdeIDRechtzaakID");
            
            CreateTable(
                "Recht.Beklaagdes",
                c => new
                    {
                        BeklaagdeID = c.Int(nullable: false, identity: true),
                        Voornaam = c.String(nullable: false),
                        Achternaam = c.String(nullable: false),
                        Gemeente = c.String(nullable: false),
                        Straat = c.String(nullable: false),
                        HuisNr = c.String(nullable: false),
                        Postcode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BeklaagdeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Recht.RechtzaakBeklaagde", "RechtzaakID", "Recht.Rechtzaken");
            DropForeignKey("Recht.RechtzaakBeklaagde", "BeklaagdeID", "Recht.Beklaagdes");
            DropForeignKey("Recht.RechtzaakAanklager", "RechtzaakID", "Recht.Rechtzaken");
            DropForeignKey("Recht.Rechtzaken", "RechtbankID", "Recht.Rechtbanken");
            DropForeignKey("Recht.Rechtzaken", "RechterID", "Recht.Rechter");
            DropForeignKey("Recht.Rechter", "RechtbankID", "Recht.Rechtbanken");
            DropForeignKey("Recht.Jurys", "RechtzaakID", "Recht.Rechtzaken");
            DropForeignKey("Recht.Jurys", "JurylidID", "Recht.Juryleden");
            DropForeignKey("Recht.RechtzaakAanklager", "AanklagerID", "Recht.Aanklagers");
            DropIndex("Recht.RechtzaakBeklaagde", "IX_BeklaagdeIDRechtzaakID");
            DropIndex("Recht.Rechter", new[] { "RechtbankID" });
            DropIndex("Recht.Jurys", "IX_JurylidIDRechtzaakID");
            DropIndex("Recht.Rechtzaken", new[] { "RechterID" });
            DropIndex("Recht.Rechtzaken", new[] { "RechtbankID" });
            DropIndex("Recht.RechtzaakAanklager", "IX_AanklagerIDRechtzaakID");
            DropTable("Recht.Beklaagdes");
            DropTable("Recht.RechtzaakBeklaagde");
            DropTable("Recht.Rechter");
            DropTable("Recht.Rechtbanken");
            DropTable("Recht.Juryleden");
            DropTable("Recht.Jurys");
            DropTable("Recht.Rechtzaken");
            DropTable("Recht.RechtzaakAanklager");
            DropTable("Recht.Aanklagers");
        }
    }
}
