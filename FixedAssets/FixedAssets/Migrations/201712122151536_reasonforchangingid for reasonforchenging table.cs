namespace FixedAssets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reasonforchangingidforreasonforchengingtable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReasonForChangings", "ReasonForChangingId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReasonForChangings", "ReasonForChangingId");
        }
    }
}
