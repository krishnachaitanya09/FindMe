namespace GPSServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial8 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Locations", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Locations", "UserId");
            AddForeignKey("dbo.Locations", "UserId", "dbo.Users", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Locations", "UserId", "dbo.Users");
            DropIndex("dbo.Locations", new[] { "UserId" });
            AlterColumn("dbo.Locations", "UserId", c => c.String());
        }
    }
}
