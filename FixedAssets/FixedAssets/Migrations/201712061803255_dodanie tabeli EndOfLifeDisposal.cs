namespace FixedAssets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dodanietabeliEndOfLifeDisposal : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EndOfLifeDisposals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        No = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        DisposalDate = c.DateTime(nullable: false),
                        EndOfLifeReason = c.String(),
                        CreatedBy = c.String(),
                        DisposalCompany = c.String(),
                        DisposedOfBy = c.String(),
                        Asset_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assets", t => t.Asset_Id)
                .Index(t => t.Asset_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EndOfLifeDisposals", "Asset_Id", "dbo.Assets");
            DropIndex("dbo.EndOfLifeDisposals", new[] { "Asset_Id" });
            DropTable("dbo.EndOfLifeDisposals");
        }
    }
}
