namespace FixedAssets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Powiązanieasset_idwdepretiationCharges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DepreciationCharges", "Asset_Id", "dbo.Assets");
            DropIndex("dbo.DepreciationCharges", new[] { "Asset_Id" });
            AddColumn("dbo.DepreciationCharges", "Asset_Id1", c => c.Int());
            AlterColumn("dbo.DepreciationCharges", "Asset_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.DepreciationCharges", "Asset_Id1");
            AddForeignKey("dbo.DepreciationCharges", "Asset_Id1", "dbo.Assets", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DepreciationCharges", "Asset_Id1", "dbo.Assets");
            DropIndex("dbo.DepreciationCharges", new[] { "Asset_Id1" });
            AlterColumn("dbo.DepreciationCharges", "Asset_Id", c => c.Int());
            DropColumn("dbo.DepreciationCharges", "Asset_Id1");
            CreateIndex("dbo.DepreciationCharges", "Asset_Id");
            AddForeignKey("dbo.DepreciationCharges", "Asset_Id", "dbo.Assets", "Id");
        }
    }
}
