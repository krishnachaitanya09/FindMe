namespace GPSServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SharedLocationSessions",
                c => new
                    {
                        SessionID = c.String(nullable: false, maxLength: 128),
                        SharedWithUser = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.SessionID)
                .ForeignKey("dbo.Users", t => t.SharedWithUser)
                .Index(t => t.SharedWithUser);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SharedLocationSessions", "SharedWithUser", "dbo.Users");
            DropIndex("dbo.SharedLocationSessions", new[] { "SharedWithUser" });
            DropTable("dbo.SharedLocationSessions");
        }
    }
}
