namespace FixedAssets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migracja1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Assets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AssetName = c.String(),
                        IsUsed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AssetTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AssetTypeName = c.String(),
                        Asset_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assets", t => t.Asset_Id)
                .Index(t => t.Asset_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssetTypes", "Asset_Id", "dbo.Assets");
            DropIndex("dbo.AssetTypes", new[] { "Asset_Id" });
            DropTable("dbo.AssetTypes");
            DropTable("dbo.Assets");
        }
    }
}
