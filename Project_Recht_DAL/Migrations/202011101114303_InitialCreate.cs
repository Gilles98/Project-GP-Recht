namespace Project_Recht_DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Aanklachten",
                c => new
                    {
                        AanklachtID = c.Int(nullable: false, identity: true),
                        BeklaagdeID = c.Int(nullable: false),
                        AanklagerID = c.Int(nullable: false),
                        RechtzaakID = c.Int(nullable: false),
                        OmschrijvingKlacht = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.AanklachtID)
                .ForeignKey("dbo.Aanklagers", t => t.AanklagerID, cascadeDelete: true)
                .ForeignKey("dbo.Beklaagdes", t => t.BeklaagdeID, cascadeDelete: true)
                .ForeignKey("dbo.Rechtzaken", t => t.RechtzaakID, cascadeDelete: true)
                .Index(t => new { t.BeklaagdeID, t.AanklagerID, t.RechtzaakID }, unique: true, name: "IX_BeklaagdeIDAanklagerIDRechtzaakID");
            
            CreateTable(
                "dbo.Aanklagers",
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
                "dbo.Beklaagdes",
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
            
            CreateTable(
                "dbo.Rechtzaken",
                c => new
                    {
                        RechtzaakID = c.Int(nullable: false, identity: true),
                        Moment = c.DateTime(nullable: false),
                        Code = c.String(),
                        RechtbankID = c.Int(nullable: false),
                        RechterID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RechtzaakID)
                .ForeignKey("dbo.Rechtbanken", t => t.RechtbankID, cascadeDelete: true)
                .ForeignKey("dbo.Rechter", t => t.RechterID, cascadeDelete: true)
                .Index(t => t.RechtbankID)
                .Index(t => t.RechterID);
            
            CreateTable(
                "dbo.Jurys",
                c => new
                    {
                        JuryID = c.Int(nullable: false, identity: true),
                        JurylidID = c.Int(nullable: false),
                        RechtzaakID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JuryID)
                .ForeignKey("dbo.Juryleden", t => t.JurylidID, cascadeDelete: true)
                .ForeignKey("dbo.Rechtzaken", t => t.RechtzaakID, cascadeDelete: true)
                .Index(t => new { t.JurylidID, t.RechtzaakID }, unique: true, name: "IX_JurylidIDRechtzaakID");
            
            CreateTable(
                "dbo.Juryleden",
                c => new
                    {
                        JurylidID = c.Int(nullable: false, identity: true),
                        Voornaam = c.String(nullable: false),
                        Achternaam = c.String(nullable: false),
                        Opgeroepen = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.JurylidID);
            
            CreateTable(
                "dbo.Rechtbanken",
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
                "dbo.Rechter",
                c => new
                    {
                        RechterID = c.Int(nullable: false, identity: true),
                        Voornaam = c.String(nullable: false),
                        Achternaam = c.String(nullable: false),
                        RechtbankID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RechterID)
                .ForeignKey("dbo.Rechtbanken", t => t.RechtbankID)
                .Index(t => t.RechtbankID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rechtzaken", "RechterID", "dbo.Rechter");
            DropForeignKey("dbo.Rechtzaken", "RechtbankID", "dbo.Rechtbanken");
            DropForeignKey("dbo.Rechter", "RechtbankID", "dbo.Rechtbanken");
            DropForeignKey("dbo.Jurys", "RechtzaakID", "dbo.Rechtzaken");
            DropForeignKey("dbo.Jurys", "JurylidID", "dbo.Juryleden");
            DropForeignKey("dbo.Aanklachten", "RechtzaakID", "dbo.Rechtzaken");
            DropForeignKey("dbo.Aanklachten", "BeklaagdeID", "dbo.Beklaagdes");
            DropForeignKey("dbo.Aanklachten", "AanklagerID", "dbo.Aanklagers");
            DropIndex("dbo.Rechter", new[] { "RechtbankID" });
            DropIndex("dbo.Jurys", "IX_JurylidIDRechtzaakID");
            DropIndex("dbo.Rechtzaken", new[] { "RechterID" });
            DropIndex("dbo.Rechtzaken", new[] { "RechtbankID" });
            DropIndex("dbo.Aanklachten", "IX_BeklaagdeIDAanklagerIDRechtzaakID");
            DropTable("dbo.Rechter");
            DropTable("dbo.Rechtbanken");
            DropTable("dbo.Juryleden");
            DropTable("dbo.Jurys");
            DropTable("dbo.Rechtzaken");
            DropTable("dbo.Beklaagdes");
            DropTable("dbo.Aanklagers");
            DropTable("dbo.Aanklachten");
        }
    }
}
