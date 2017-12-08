namespace FixedAssets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class foreignkeysdomodeluassets : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Assets", "AssetLocation_Id", "dbo.AssetLocations");
            DropForeignKey("dbo.Assets", "AssetType_Id", "dbo.AssetTypes");
            DropForeignKey("dbo.Assets", "DepreciationType_Id", "dbo.DepreciationTypes");
            DropIndex("dbo.Assets", new[] { "AssetLocation_Id" });
            DropIndex("dbo.Assets", new[] { "AssetType_Id" });
            DropIndex("dbo.Assets", new[] { "DepreciationType_Id" });
            RenameColumn(table: "dbo.Assets", name: "AssetLocation_Id", newName: "AssetLocationId");
            RenameColumn(table: "dbo.Assets", name: "AssetType_Id", newName: "AssetTypeId");
            RenameColumn(table: "dbo.Assets", name: "DepreciationType_Id", newName: "DepreciationTypeId");
            AlterColumn("dbo.Assets", "AssetLocationId", c => c.Int(nullable: false));
            AlterColumn("dbo.Assets", "AssetTypeId", c => c.Int(nullable: false));
            AlterColumn("dbo.Assets", "DepreciationTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Assets", "DepreciationTypeId");
            CreateIndex("dbo.Assets", "AssetTypeId");
            CreateIndex("dbo.Assets", "AssetLocationId");
            AddForeignKey("dbo.Assets", "AssetLocationId", "dbo.AssetLocations", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Assets", "AssetTypeId", "dbo.AssetTypes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Assets", "DepreciationTypeId", "dbo.DepreciationTypes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Assets", "DepreciationTypeId", "dbo.DepreciationTypes");
            DropForeignKey("dbo.Assets", "AssetTypeId", "dbo.AssetTypes");
            DropForeignKey("dbo.Assets", "AssetLocationId", "dbo.AssetLocations");
            DropIndex("dbo.Assets", new[] { "AssetLocationId" });
            DropIndex("dbo.Assets", new[] { "AssetTypeId" });
            DropIndex("dbo.Assets", new[] { "DepreciationTypeId" });
            AlterColumn("dbo.Assets", "DepreciationTypeId", c => c.Int());
            AlterColumn("dbo.Assets", "AssetTypeId", c => c.Int());
            AlterColumn("dbo.Assets", "AssetLocationId", c => c.Int());
            RenameColumn(table: "dbo.Assets", name: "DepreciationTypeId", newName: "DepreciationType_Id");
            RenameColumn(table: "dbo.Assets", name: "AssetTypeId", newName: "AssetType_Id");
            RenameColumn(table: "dbo.Assets", name: "AssetLocationId", newName: "AssetLocation_Id");
            CreateIndex("dbo.Assets", "DepreciationType_Id");
            CreateIndex("dbo.Assets", "AssetType_Id");
            CreateIndex("dbo.Assets", "AssetLocation_Id");
            AddForeignKey("dbo.Assets", "DepreciationType_Id", "dbo.DepreciationTypes", "Id");
            AddForeignKey("dbo.Assets", "AssetType_Id", "dbo.AssetTypes", "Id");
            AddForeignKey("dbo.Assets", "AssetLocation_Id", "dbo.AssetLocations", "Id");
        }
    }
}
