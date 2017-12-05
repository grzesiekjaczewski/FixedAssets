namespace FixedAssets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Newtabelsassetlocationdepretiationtype : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssetLocations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LocationName = c.String(),
                        Asset_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assets", t => t.Asset_Id)
                .Index(t => t.Asset_Id);
            
            CreateTable(
                "dbo.DepreciationTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DepreciationTypeName = c.String(),
                        Percent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Asset_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assets", t => t.Asset_Id)
                .Index(t => t.Asset_Id);
            
            AddColumn("dbo.Assets", "Quantity", c => c.Int(nullable: false));
            AddColumn("dbo.Assets", "StartUsingDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Assets", "InitialValue", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Assets", "Depreciationeted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DepreciationTypes", "Asset_Id", "dbo.Assets");
            DropForeignKey("dbo.AssetLocations", "Asset_Id", "dbo.Assets");
            DropIndex("dbo.DepreciationTypes", new[] { "Asset_Id" });
            DropIndex("dbo.AssetLocations", new[] { "Asset_Id" });
            DropColumn("dbo.Assets", "Depreciationeted");
            DropColumn("dbo.Assets", "InitialValue");
            DropColumn("dbo.Assets", "StartUsingDate");
            DropColumn("dbo.Assets", "Quantity");
            DropTable("dbo.DepreciationTypes");
            DropTable("dbo.AssetLocations");
        }
    }
}
