namespace FixedAssets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dodaniekolejnychtabelorazpoprawkawassetichangeinvalue : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ReasonForChangings", "ChangeInValue_Id", "dbo.ChangeInValues");
            DropIndex("dbo.ReasonForChangings", new[] { "ChangeInValue_Id" });
            AddColumn("dbo.ChangeInValues", "ValueOfChange", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ChangeInValues", "ValueAfterChange", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ChangeInValues", "ReasonForChanging_Id", c => c.Int());
            CreateIndex("dbo.ChangeInValues", "ReasonForChanging_Id");
            AddForeignKey("dbo.ChangeInValues", "ReasonForChanging_Id", "dbo.ReasonForChangings", "Id");
            DropColumn("dbo.ReasonForChangings", "ChangeInValue_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ReasonForChangings", "ChangeInValue_Id", c => c.Int());
            DropForeignKey("dbo.ChangeInValues", "ReasonForChanging_Id", "dbo.ReasonForChangings");
            DropIndex("dbo.ChangeInValues", new[] { "ReasonForChanging_Id" });
            DropColumn("dbo.ChangeInValues", "ReasonForChanging_Id");
            DropColumn("dbo.ChangeInValues", "ValueAfterChange");
            DropColumn("dbo.ChangeInValues", "ValueOfChange");
            CreateIndex("dbo.ReasonForChangings", "ChangeInValue_Id");
            AddForeignKey("dbo.ReasonForChangings", "ChangeInValue_Id", "dbo.ChangeInValues", "Id");
        }
    }
}
