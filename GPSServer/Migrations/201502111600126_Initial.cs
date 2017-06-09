namespace GPSServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
           
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ThreatLocations", "ThreatId", "dbo.Threats");
            DropForeignKey("dbo.ThreatLocations", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Locations", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserClaims", "UserId", "dbo.Users");
            DropForeignKey("dbo.Blocks", "UserId", "dbo.Users");
            DropIndex("dbo.ThreatLocations", new[] { "LocationId" });
            DropIndex("dbo.ThreatLocations", new[] { "ThreatId" });
            DropIndex("dbo.Roles", "RoleNameIndex");
            DropIndex("dbo.Locations", new[] { "UserId" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.UserLogins", new[] { "UserId" });
            DropIndex("dbo.UserClaims", new[] { "UserId" });
            DropIndex("dbo.Users", "UserNameIndex");
            DropIndex("dbo.Blocks", new[] { "UserId" });
            DropTable("dbo.Threats");
            DropTable("dbo.ThreatLocations");
            DropTable("dbo.Roles");
            DropTable("dbo.Locations");
            DropTable("dbo.UserRoles");
            DropTable("dbo.UserLogins");
            DropTable("dbo.UserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.Blocks");
        }
    }
}
