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
                .ForeignKey("dbo.Rechter", t => t.RechterID, cascadeDelete: true)
                .ForeignKey("dbo.Rechtbanken", t => t.RechtbankID, cascadeDelete: true)
                .Index(t => t.RechtbankID)
                .Index(t => t.RechterID);
            
            CreateTable(
                "dbo.Jury's",
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
                        Rechtbank_RechtbankID = c.Int(nullable: false),
                        Rechtbank_RechtbankID1 = c.Int(),
                    })
                .PrimaryKey(t => t.RechterID)
                .ForeignKey("dbo.Rechtbanken", t => t.Rechtbank_RechtbankID)
                .ForeignKey("dbo.Rechtbanken", t => t.Rechtbank_RechtbankID1)
                .Index(t => t.Rechtbank_RechtbankID)
                .Index(t => t.Rechtbank_RechtbankID1);
            
            CreateTable(
                "dbo.AanklagerAanklachts",
                c => new
                    {
                        Aanklager_AanklagerID = c.Int(nullable: false),
                        Aanklacht_AanklachtID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Aanklager_AanklagerID, t.Aanklacht_AanklachtID })
                .ForeignKey("dbo.Aanklagers", t => t.Aanklager_AanklagerID, cascadeDelete: true)
                .ForeignKey("dbo.Aanklachten", t => t.Aanklacht_AanklachtID, cascadeDelete: true)
                .Index(t => t.Aanklager_AanklagerID)
                .Index(t => t.Aanklacht_AanklachtID);
            
            CreateTable(
                "dbo.BeklaagdeAanklachts",
                c => new
                    {
                        Beklaagde_BeklaagdeID = c.Int(nullable: false),
                        Aanklacht_AanklachtID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Beklaagde_BeklaagdeID, t.Aanklacht_AanklachtID })
                .ForeignKey("dbo.Beklaagdes", t => t.Beklaagde_BeklaagdeID, cascadeDelete: true)
                .ForeignKey("dbo.Aanklachten", t => t.Aanklacht_AanklachtID, cascadeDelete: true)
                .Index(t => t.Beklaagde_BeklaagdeID)
                .Index(t => t.Aanklacht_AanklachtID);
            
            CreateTable(
                "dbo.RechtzaakAanklachts",
                c => new
                    {
                        Rechtzaak_RechtzaakID = c.Int(nullable: false),
                        Aanklacht_AanklachtID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Rechtzaak_RechtzaakID, t.Aanklacht_AanklachtID })
                .ForeignKey("dbo.Rechtzaken", t => t.Rechtzaak_RechtzaakID, cascadeDelete: true)
                .ForeignKey("dbo.Aanklachten", t => t.Aanklacht_AanklachtID, cascadeDelete: true)
                .Index(t => t.Rechtzaak_RechtzaakID)
                .Index(t => t.Aanklacht_AanklachtID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rechtzaken", "RechtbankID", "dbo.Rechtbanken");
            DropForeignKey("dbo.Rechter", "Rechtbank_RechtbankID1", "dbo.Rechtbanken");
            DropForeignKey("dbo.Rechtzaken", "RechterID", "dbo.Rechter");
            DropForeignKey("dbo.Rechter", "Rechtbank_RechtbankID", "dbo.Rechtbanken");
            DropForeignKey("dbo.Jury's", "RechtzaakID", "dbo.Rechtzaken");
            DropForeignKey("dbo.Jury's", "JurylidID", "dbo.Juryleden");
            DropForeignKey("dbo.RechtzaakAanklachts", "Aanklacht_AanklachtID", "dbo.Aanklachten");
            DropForeignKey("dbo.RechtzaakAanklachts", "Rechtzaak_RechtzaakID", "dbo.Rechtzaken");
            DropForeignKey("dbo.BeklaagdeAanklachts", "Aanklacht_AanklachtID", "dbo.Aanklachten");
            DropForeignKey("dbo.BeklaagdeAanklachts", "Beklaagde_BeklaagdeID", "dbo.Beklaagdes");
            DropForeignKey("dbo.AanklagerAanklachts", "Aanklacht_AanklachtID", "dbo.Aanklachten");
            DropForeignKey("dbo.AanklagerAanklachts", "Aanklager_AanklagerID", "dbo.Aanklagers");
            DropIndex("dbo.RechtzaakAanklachts", new[] { "Aanklacht_AanklachtID" });
            DropIndex("dbo.RechtzaakAanklachts", new[] { "Rechtzaak_RechtzaakID" });
            DropIndex("dbo.BeklaagdeAanklachts", new[] { "Aanklacht_AanklachtID" });
            DropIndex("dbo.BeklaagdeAanklachts", new[] { "Beklaagde_BeklaagdeID" });
            DropIndex("dbo.AanklagerAanklachts", new[] { "Aanklacht_AanklachtID" });
            DropIndex("dbo.AanklagerAanklachts", new[] { "Aanklager_AanklagerID" });
            DropIndex("dbo.Rechter", new[] { "Rechtbank_RechtbankID1" });
            DropIndex("dbo.Rechter", new[] { "Rechtbank_RechtbankID" });
            DropIndex("dbo.Jury's", "IX_JurylidIDRechtzaakID");
            DropIndex("dbo.Rechtzaken", new[] { "RechterID" });
            DropIndex("dbo.Rechtzaken", new[] { "RechtbankID" });
            DropIndex("dbo.Aanklachten", "IX_BeklaagdeIDAanklagerIDRechtzaakID");
            DropTable("dbo.RechtzaakAanklachts");
            DropTable("dbo.BeklaagdeAanklachts");
            DropTable("dbo.AanklagerAanklachts");
            DropTable("dbo.Rechter");
            DropTable("dbo.Rechtbanken");
            DropTable("dbo.Juryleden");
            DropTable("dbo.Jury's");
            DropTable("dbo.Rechtzaken");
            DropTable("dbo.Beklaagdes");
            DropTable("dbo.Aanklagers");
            DropTable("dbo.Aanklachten");
        }
    }
}
