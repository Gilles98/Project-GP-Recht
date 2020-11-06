namespace Project_Recht_DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AanklagerAanklachts", "Aanklager_AanklagerID", "dbo.Aanklagers");
            DropForeignKey("dbo.AanklagerAanklachts", "Aanklacht_AanklachtID", "dbo.Aanklachten");
            DropForeignKey("dbo.BeklaagdeAanklachts", "Beklaagde_BeklaagdeID", "dbo.Beklaagdes");
            DropForeignKey("dbo.BeklaagdeAanklachts", "Aanklacht_AanklachtID", "dbo.Aanklachten");
            DropIndex("dbo.AanklagerAanklachts", new[] { "Aanklager_AanklagerID" });
            DropIndex("dbo.AanklagerAanklachts", new[] { "Aanklacht_AanklachtID" });
            DropIndex("dbo.BeklaagdeAanklachts", new[] { "Beklaagde_BeklaagdeID" });
            DropIndex("dbo.BeklaagdeAanklachts", new[] { "Aanklacht_AanklachtID" });
            AddForeignKey("dbo.Aanklachten", "AanklagerID", "dbo.Aanklagers", "AanklagerID", cascadeDelete: true);
            AddForeignKey("dbo.Aanklachten", "BeklaagdeID", "dbo.Beklaagdes", "BeklaagdeID", cascadeDelete: true);
            DropTable("dbo.AanklagerAanklachts");
            DropTable("dbo.BeklaagdeAanklachts");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.BeklaagdeAanklachts",
                c => new
                    {
                        Beklaagde_BeklaagdeID = c.Int(nullable: false),
                        Aanklacht_AanklachtID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Beklaagde_BeklaagdeID, t.Aanklacht_AanklachtID });
            
            CreateTable(
                "dbo.AanklagerAanklachts",
                c => new
                    {
                        Aanklager_AanklagerID = c.Int(nullable: false),
                        Aanklacht_AanklachtID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Aanklager_AanklagerID, t.Aanklacht_AanklachtID });
            
            DropForeignKey("dbo.Aanklachten", "BeklaagdeID", "dbo.Beklaagdes");
            DropForeignKey("dbo.Aanklachten", "AanklagerID", "dbo.Aanklagers");
            CreateIndex("dbo.BeklaagdeAanklachts", "Aanklacht_AanklachtID");
            CreateIndex("dbo.BeklaagdeAanklachts", "Beklaagde_BeklaagdeID");
            CreateIndex("dbo.AanklagerAanklachts", "Aanklacht_AanklachtID");
            CreateIndex("dbo.AanklagerAanklachts", "Aanklager_AanklagerID");
            AddForeignKey("dbo.BeklaagdeAanklachts", "Aanklacht_AanklachtID", "dbo.Aanklachten", "AanklachtID", cascadeDelete: true);
            AddForeignKey("dbo.BeklaagdeAanklachts", "Beklaagde_BeklaagdeID", "dbo.Beklaagdes", "BeklaagdeID", cascadeDelete: true);
            AddForeignKey("dbo.AanklagerAanklachts", "Aanklacht_AanklachtID", "dbo.Aanklachten", "AanklachtID", cascadeDelete: true);
            AddForeignKey("dbo.AanklagerAanklachts", "Aanklager_AanklagerID", "dbo.Aanklagers", "AanklagerID", cascadeDelete: true);
        }
    }
}
