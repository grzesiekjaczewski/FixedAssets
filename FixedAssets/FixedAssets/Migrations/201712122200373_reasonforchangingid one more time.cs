namespace FixedAssets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reasonforchangingidonemoretime : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ChangeInValues", "ReasonForChanging_Id", "dbo.ReasonForChangings");
            DropIndex("dbo.ChangeInValues", new[] { "ReasonForChanging_Id" });
            RenameColumn(table: "dbo.ChangeInValues", name: "ReasonForChanging_Id", newName: "ReasonForChangingId");
            AlterColumn("dbo.ChangeInValues", "ReasonForChangingId", c => c.Int(nullable: false));
            CreateIndex("dbo.ChangeInValues", "ReasonForChangingId");
            AddForeignKey("dbo.ChangeInValues", "ReasonForChangingId", "dbo.ReasonForChangings", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChangeInValues", "ReasonForChangingId", "dbo.ReasonForChangings");
            DropIndex("dbo.ChangeInValues", new[] { "ReasonForChangingId" });
            AlterColumn("dbo.ChangeInValues", "ReasonForChangingId", c => c.Int());
            RenameColumn(table: "dbo.ChangeInValues", name: "ReasonForChangingId", newName: "ReasonForChanging_Id");
            CreateIndex("dbo.ChangeInValues", "ReasonForChanging_Id");
            AddForeignKey("dbo.ChangeInValues", "ReasonForChanging_Id", "dbo.ReasonForChangings", "Id");
        }
    }
}
