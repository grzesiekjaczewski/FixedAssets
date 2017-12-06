namespace FixedAssets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModyfikacjatabeliAssets : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "InventoryNo", c => c.String());
            AddColumn("dbo.Assets", "ProofOfPurchase", c => c.String());
            DropColumn("dbo.Assets", "Quantity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Assets", "Quantity", c => c.Int(nullable: false));
            DropColumn("dbo.Assets", "ProofOfPurchase");
            DropColumn("dbo.Assets", "InventoryNo");
        }
    }
}
