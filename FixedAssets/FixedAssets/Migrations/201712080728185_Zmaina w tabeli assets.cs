namespace FixedAssets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Zmainawtabeliassets : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AssetLocations", "Asset_Id", "dbo.Assets");
            DropIndex("dbo.AssetLocations", new[] { "Asset_Id" });
            AddColumn("dbo.Assets", "AssetLocation_Id", c => c.Int());
            CreateIndex("dbo.Assets", "AssetLocation_Id");
            AddForeignKey("dbo.Assets", "AssetLocation_Id", "dbo.AssetLocations", "Id");
            DropColumn("dbo.AssetLocations", "Asset_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AssetLocations", "Asset_Id", c => c.Int());
            DropForeignKey("dbo.Assets", "AssetLocation_Id", "dbo.AssetLocations");
            DropIndex("dbo.Assets", new[] { "AssetLocation_Id" });
            DropColumn("dbo.Assets", "AssetLocation_Id");
            CreateIndex("dbo.AssetLocations", "Asset_Id");
            AddForeignKey("dbo.AssetLocations", "Asset_Id", "dbo.Assets", "Id");
        }
    }
}
