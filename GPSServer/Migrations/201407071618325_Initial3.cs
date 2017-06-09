namespace GPSServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SharedLocationSessions", "SharedWithUser", "dbo.Users");
            DropIndex("dbo.SharedLocationSessions", new[] { "SharedWithUser" });
            CreateTable(
                "dbo.Sessions",
                c => new
                    {
                        SessionID = c.String(nullable: false, maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SessionID);
            
            AlterColumn("dbo.Locations", "SessionID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Locations", "SessionID");
            AddForeignKey("dbo.Locations", "SessionID", "dbo.Sessions", "SessionID");
            DropTable("dbo.SharedLocationSessions");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SharedLocationSessions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SessionID = c.String(),
                        SharedWithUser = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Locations", "SessionID", "dbo.Sessions");
            DropIndex("dbo.Locations", new[] { "SessionID" });
            AlterColumn("dbo.Locations", "SessionID", c => c.String());
            DropTable("dbo.Sessions");
            CreateIndex("dbo.SharedLocationSessions", "SharedWithUser");
            AddForeignKey("dbo.SharedLocationSessions", "SharedWithUser", "dbo.Users", "UserId");
        }
    }
}
