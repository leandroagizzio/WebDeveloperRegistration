namespace Register.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataAnnotationsMetaData : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Developers", "FirstName", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Developers", "LastName", c => c.String(nullable: false, maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Developers", "LastName", c => c.String(nullable: false));
            AlterColumn("dbo.Developers", "FirstName", c => c.String(nullable: false));
        }
    }
}
