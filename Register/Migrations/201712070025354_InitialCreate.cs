namespace Register.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Developers",
                c => new
                    {
                        DeveloperId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Phone = c.String(nullable: false),
                        DayOfBirth = c.DateTime(nullable: false),
                        YearsExperience = c.Int(nullable: false),
                        Comments = c.String(),
                    })
                .PrimaryKey(t => t.DeveloperId);
            
            CreateTable(
                "dbo.Stacks",
                c => new
                    {
                        StackId = c.Int(nullable: false, identity: true),
                        StackName = c.String(),
                    })
                .PrimaryKey(t => t.StackId);
            
            CreateTable(
                "dbo.Technologies",
                c => new
                    {
                        TechnologyId = c.Int(nullable: false, identity: true),
                        TechnologyName = c.String(),
                    })
                .PrimaryKey(t => t.TechnologyId);
            
            CreateTable(
                "dbo.StackDevelopers",
                c => new
                    {
                        Stack_StackId = c.Int(nullable: false),
                        Developer_DeveloperId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Stack_StackId, t.Developer_DeveloperId })
                .ForeignKey("dbo.Stacks", t => t.Stack_StackId, cascadeDelete: true)
                .ForeignKey("dbo.Developers", t => t.Developer_DeveloperId, cascadeDelete: true)
                .Index(t => t.Stack_StackId)
                .Index(t => t.Developer_DeveloperId);
            
            CreateTable(
                "dbo.TechnologyDevelopers",
                c => new
                    {
                        Technology_TechnologyId = c.Int(nullable: false),
                        Developer_DeveloperId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Technology_TechnologyId, t.Developer_DeveloperId })
                .ForeignKey("dbo.Technologies", t => t.Technology_TechnologyId, cascadeDelete: true)
                .ForeignKey("dbo.Developers", t => t.Developer_DeveloperId, cascadeDelete: true)
                .Index(t => t.Technology_TechnologyId)
                .Index(t => t.Developer_DeveloperId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TechnologyDevelopers", "Developer_DeveloperId", "dbo.Developers");
            DropForeignKey("dbo.TechnologyDevelopers", "Technology_TechnologyId", "dbo.Technologies");
            DropForeignKey("dbo.StackDevelopers", "Developer_DeveloperId", "dbo.Developers");
            DropForeignKey("dbo.StackDevelopers", "Stack_StackId", "dbo.Stacks");
            DropIndex("dbo.TechnologyDevelopers", new[] { "Developer_DeveloperId" });
            DropIndex("dbo.TechnologyDevelopers", new[] { "Technology_TechnologyId" });
            DropIndex("dbo.StackDevelopers", new[] { "Developer_DeveloperId" });
            DropIndex("dbo.StackDevelopers", new[] { "Stack_StackId" });
            DropTable("dbo.TechnologyDevelopers");
            DropTable("dbo.StackDevelopers");
            DropTable("dbo.Technologies");
            DropTable("dbo.Stacks");
            DropTable("dbo.Developers");
        }
    }
}
