namespace AdsCommon.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ads",
                c => new
                    {
                        AdId = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 100),
                        Price = c.Int(nullable: false),
                        Description = c.String(maxLength: 1000),
                        ImageURL = c.String(maxLength: 1000),
                        ThumbnailURL = c.String(maxLength: 1000),
                        PostedDate = c.DateTime(nullable: false),
                        Category = c.Int(),
                        Phone = c.String(maxLength: 12),
                    })
                .PrimaryKey(t => t.AdId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Ads");
        }
    }
}
