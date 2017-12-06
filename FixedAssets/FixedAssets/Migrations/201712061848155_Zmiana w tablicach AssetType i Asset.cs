namespace FixedAssets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianawtablicachAssetTypeiAsset : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AssetTypes", "Asset_Id", "dbo.Assets");
            DropForeignKey("dbo.DepreciationTypes", "Asset_Id", "dbo.Assets");
            DropIndex("dbo.AssetTypes", new[] { "Asset_Id" });
            DropIndex("dbo.DepreciationTypes", new[] { "Asset_Id" });
            AddColumn("dbo.Assets", "AssetType_Id", c => c.Int());
            AddColumn("dbo.Assets", "DepreciationType_Id", c => c.Int());
            AddColumn("dbo.AssetTypes", "LowValueAsset", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Assets", "AssetType_Id");
            CreateIndex("dbo.Assets", "DepreciationType_Id");
            AddForeignKey("dbo.Assets", "AssetType_Id", "dbo.AssetTypes", "Id");
            AddForeignKey("dbo.Assets", "DepreciationType_Id", "dbo.DepreciationTypes", "Id");
            DropColumn("dbo.AssetTypes", "Asset_Id");
            DropColumn("dbo.DepreciationTypes", "Asset_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DepreciationTypes", "Asset_Id", c => c.Int());
            AddColumn("dbo.AssetTypes", "Asset_Id", c => c.Int());
            DropForeignKey("dbo.Assets", "DepreciationType_Id", "dbo.DepreciationTypes");
            DropForeignKey("dbo.Assets", "AssetType_Id", "dbo.AssetTypes");
            DropIndex("dbo.Assets", new[] { "DepreciationType_Id" });
            DropIndex("dbo.Assets", new[] { "AssetType_Id" });
            DropColumn("dbo.AssetTypes", "LowValueAsset");
            DropColumn("dbo.Assets", "DepreciationType_Id");
            DropColumn("dbo.Assets", "AssetType_Id");
            CreateIndex("dbo.DepreciationTypes", "Asset_Id");
            CreateIndex("dbo.AssetTypes", "Asset_Id");
            AddForeignKey("dbo.DepreciationTypes", "Asset_Id", "dbo.Assets", "Id");
            AddForeignKey("dbo.AssetTypes", "Asset_Id", "dbo.Assets", "Id");
        }
    }
}
