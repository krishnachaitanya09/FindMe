namespace GPSServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial7 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Connections", "ApplicationUser_Id", "dbo.Users");
            DropForeignKey("dbo.Locations", "UserId", "dbo.Users");
            DropIndex("dbo.Connections", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Locations", new[] { "UserId" });
            AlterColumn("dbo.Locations", "UserId", c => c.String());
            DropTable("dbo.Connections");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Connections",
                c => new
                    {
                        ConnectionID = c.String(nullable: false, maxLength: 128),
                        UserAgent = c.String(),
                        Connected = c.Boolean(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ConnectionID);
            
            AlterColumn("dbo.Locations", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Locations", "UserId");
            CreateIndex("dbo.Connections", "ApplicationUser_Id");
            AddForeignKey("dbo.Locations", "UserId", "dbo.Users", "UserId");
            AddForeignKey("dbo.Connections", "ApplicationUser_Id", "dbo.Users", "UserId");
        }
    }
}
