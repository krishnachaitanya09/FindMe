namespace GPSServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial2 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.SharedLocationSessions");
            AddColumn("dbo.SharedLocationSessions", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.SharedLocationSessions", "SessionID", c => c.String());
            AddPrimaryKey("dbo.SharedLocationSessions", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.SharedLocationSessions");
            AlterColumn("dbo.SharedLocationSessions", "SessionID", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.SharedLocationSessions", "Id");
            AddPrimaryKey("dbo.SharedLocationSessions", "SessionID");
        }
    }
}
