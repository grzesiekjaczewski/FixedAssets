namespace FixedAssets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ijeszczerazassetid : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DepreciationCharges", "AssetId", "dbo.Assets");
            DropIndex("dbo.DepreciationCharges", new[] { "AssetId" });
            DropColumn("dbo.DepreciationCharges", "AssetId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DepreciationCharges", "AssetId", c => c.Int(nullable: false));
            CreateIndex("dbo.DepreciationCharges", "AssetId");
            AddForeignKey("dbo.DepreciationCharges", "AssetId", "dbo.Assets", "Id", cascadeDelete: true);
        }
    }
}
