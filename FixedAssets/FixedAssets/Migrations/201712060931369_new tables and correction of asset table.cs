namespace FixedAssets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newtablesandcorrectionofassettable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChangeInValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ChangingDate = c.DateTime(nullable: false),
                        Asset_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assets", t => t.Asset_Id)
                .Index(t => t.Asset_Id);
            
            CreateTable(
                "dbo.ReasonForChangings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        ChangeInValue_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChangeInValues", t => t.ChangeInValue_Id)
                .Index(t => t.ChangeInValue_Id);
            
            CreateTable(
                "dbo.DepreciationCharges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        No = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        Description = c.String(),
                        CurrentCharge = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CumulativelyCharge = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RemainingAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Asset_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assets", t => t.Asset_Id)
                .Index(t => t.Asset_Id);
            
            AddColumn("dbo.Assets", "AmortisedValue", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Assets", "Depreciated", c => c.Boolean(nullable: false));
            AddColumn("dbo.DepreciationTypes", "DepreciationRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Assets", "Depreciationeted");
            DropColumn("dbo.DepreciationTypes", "Percent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DepreciationTypes", "Percent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Assets", "Depreciationeted", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.DepreciationCharges", "Asset_Id", "dbo.Assets");
            DropForeignKey("dbo.ChangeInValues", "Asset_Id", "dbo.Assets");
            DropForeignKey("dbo.ReasonForChangings", "ChangeInValue_Id", "dbo.ChangeInValues");
            DropIndex("dbo.DepreciationCharges", new[] { "Asset_Id" });
            DropIndex("dbo.ReasonForChangings", new[] { "ChangeInValue_Id" });
            DropIndex("dbo.ChangeInValues", new[] { "Asset_Id" });
            DropColumn("dbo.DepreciationTypes", "DepreciationRate");
            DropColumn("dbo.Assets", "Depreciated");
            DropColumn("dbo.Assets", "AmortisedValue");
            DropTable("dbo.DepreciationCharges");
            DropTable("dbo.ReasonForChangings");
            DropTable("dbo.ChangeInValues");
        }
    }
}
