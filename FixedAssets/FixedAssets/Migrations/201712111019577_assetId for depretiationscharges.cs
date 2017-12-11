namespace FixedAssets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class assetIdfordepretiationscharges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DepreciationCharges", "Asset_Id1", "dbo.Assets");
            DropIndex("dbo.DepreciationCharges", new[] { "Asset_Id1" });
            RenameColumn(table: "dbo.DepreciationCharges", name: "Asset_Id1", newName: "AssetId");
            AlterColumn("dbo.DepreciationCharges", "AssetId", c => c.Int(nullable: false));
            CreateIndex("dbo.DepreciationCharges", "AssetId");
            AddForeignKey("dbo.DepreciationCharges", "AssetId", "dbo.Assets", "Id", cascadeDelete: true);
            DropColumn("dbo.DepreciationCharges", "Asset_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DepreciationCharges", "Asset_Id", c => c.Int(nullable: false));
            DropForeignKey("dbo.DepreciationCharges", "AssetId", "dbo.Assets");
            DropIndex("dbo.DepreciationCharges", new[] { "AssetId" });
            AlterColumn("dbo.DepreciationCharges", "AssetId", c => c.Int());
            RenameColumn(table: "dbo.DepreciationCharges", name: "AssetId", newName: "Asset_Id1");
            CreateIndex("dbo.DepreciationCharges", "Asset_Id1");
            AddForeignKey("dbo.DepreciationCharges", "Asset_Id1", "dbo.Assets", "Id");
        }
    }
}
