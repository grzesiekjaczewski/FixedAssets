namespace FixedAssets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Modyfikacjasłowników : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssetLocations", "Name", c => c.String());
            AddColumn("dbo.AssetTypes", "Name", c => c.String());
            AddColumn("dbo.DepreciationTypes", "Name", c => c.String());
            DropColumn("dbo.AssetLocations", "LocationName");
            DropColumn("dbo.AssetTypes", "AssetTypeName");
            DropColumn("dbo.DepreciationTypes", "DepreciationTypeName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DepreciationTypes", "DepreciationTypeName", c => c.String());
            AddColumn("dbo.AssetTypes", "AssetTypeName", c => c.String());
            AddColumn("dbo.AssetLocations", "LocationName", c => c.String());
            DropColumn("dbo.DepreciationTypes", "Name");
            DropColumn("dbo.AssetTypes", "Name");
            DropColumn("dbo.AssetLocations", "Name");
        }
    }
}
