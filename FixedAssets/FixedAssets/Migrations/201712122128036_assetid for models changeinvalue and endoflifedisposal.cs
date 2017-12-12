namespace FixedAssets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class assetidformodelschangeinvalueandendoflifedisposal : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ChangeInValues", "Asset_Id", "dbo.Assets");
            DropForeignKey("dbo.EndOfLifeDisposals", "Asset_Id", "dbo.Assets");
            DropIndex("dbo.ChangeInValues", new[] { "Asset_Id" });
            DropIndex("dbo.EndOfLifeDisposals", new[] { "Asset_Id" });
            RenameColumn(table: "dbo.ChangeInValues", name: "Asset_Id", newName: "AssetId");
            RenameColumn(table: "dbo.EndOfLifeDisposals", name: "Asset_Id", newName: "AssetId");
            AlterColumn("dbo.ChangeInValues", "AssetId", c => c.Int(nullable: false));
            AlterColumn("dbo.EndOfLifeDisposals", "AssetId", c => c.Int(nullable: false));
            CreateIndex("dbo.ChangeInValues", "AssetId");
            CreateIndex("dbo.EndOfLifeDisposals", "AssetId");
            AddForeignKey("dbo.ChangeInValues", "AssetId", "dbo.Assets", "Id", cascadeDelete: true);
            AddForeignKey("dbo.EndOfLifeDisposals", "AssetId", "dbo.Assets", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EndOfLifeDisposals", "AssetId", "dbo.Assets");
            DropForeignKey("dbo.ChangeInValues", "AssetId", "dbo.Assets");
            DropIndex("dbo.EndOfLifeDisposals", new[] { "AssetId" });
            DropIndex("dbo.ChangeInValues", new[] { "AssetId" });
            AlterColumn("dbo.EndOfLifeDisposals", "AssetId", c => c.Int());
            AlterColumn("dbo.ChangeInValues", "AssetId", c => c.Int());
            RenameColumn(table: "dbo.EndOfLifeDisposals", name: "AssetId", newName: "Asset_Id");
            RenameColumn(table: "dbo.ChangeInValues", name: "AssetId", newName: "Asset_Id");
            CreateIndex("dbo.EndOfLifeDisposals", "Asset_Id");
            CreateIndex("dbo.ChangeInValues", "Asset_Id");
            AddForeignKey("dbo.EndOfLifeDisposals", "Asset_Id", "dbo.Assets", "Id");
            AddForeignKey("dbo.ChangeInValues", "Asset_Id", "dbo.Assets", "Id");
        }
    }
}
