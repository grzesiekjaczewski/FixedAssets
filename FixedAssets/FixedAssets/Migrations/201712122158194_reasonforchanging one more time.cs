namespace FixedAssets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reasonforchangingonemoretime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DepreciationCharges", "ReasonForChangingId", c => c.Int(nullable: false));
            DropColumn("dbo.ReasonForChangings", "ReasonForChangingId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ReasonForChangings", "ReasonForChangingId", c => c.Int(nullable: false));
            DropColumn("dbo.DepreciationCharges", "ReasonForChangingId");
        }
    }
}
