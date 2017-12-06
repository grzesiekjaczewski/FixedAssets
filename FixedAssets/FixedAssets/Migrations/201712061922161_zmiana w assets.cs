namespace FixedAssets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zmianawassets : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Assets", "AssetName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Assets", "AssetName", c => c.String());
        }
    }
}
