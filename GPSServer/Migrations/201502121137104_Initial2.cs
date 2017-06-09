namespace GPSServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Threats", "Type", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Threats", "Type", c => c.String(nullable: false));
        }
    }
}
