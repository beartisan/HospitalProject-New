namespace hospital_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FullMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Availabilities",
                c => new
                    {
                        availability_id = c.Int(nullable: false, identity: true),
                        physician_id = c.Int(nullable: false),
                        department_id = c.Int(nullable: false),
                        availability_dates = c.String(),
                    })
                .PrimaryKey(t => t.availability_id)
                .ForeignKey("dbo.Departments", t => t.department_id, cascadeDelete: true)
                .ForeignKey("dbo.Physicians", t => t.physician_id, cascadeDelete: true)
                .Index(t => t.physician_id)
                .Index(t => t.department_id);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        department_id = c.Int(nullable: false, identity: true),
                        department_name = c.String(),
                    })
                .PrimaryKey(t => t.department_id);
            
            CreateTable(
                "dbo.Physicians",
                c => new
                    {
                        physician_id = c.Int(nullable: false, identity: true),
                        first_name = c.String(),
                        last_name = c.String(),
                        email = c.String(),
                    })
                .PrimaryKey(t => t.physician_id);
            
            CreateTable(
                "dbo.Labs",
                c => new
                    {
                        LabId = c.Int(nullable: false, identity: true),
                        LabName = c.String(),
                        Project_ProjectId = c.Int(),
                    })
                .PrimaryKey(t => t.LabId)
                .ForeignKey("dbo.Projects", t => t.Project_ProjectId)
                .Index(t => t.Project_ProjectId);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ProjectId = c.Int(nullable: false, identity: true),
                        ProjectName = c.String(),
                        LabId = c.Int(nullable: false),
                        Lab_LabId = c.Int(),
                    })
                .PrimaryKey(t => t.ProjectId)
                .ForeignKey("dbo.Labs", t => t.LabId, cascadeDelete: true)
                .ForeignKey("dbo.Labs", t => t.Lab_LabId)
                .Index(t => t.LabId)
                .Index(t => t.Lab_LabId);
            
            CreateTable(
                "dbo.Researchers",
                c => new
                    {
                        ResearcherId = c.Int(nullable: false, identity: true),
                        ResearcherName = c.String(),
                        ResearchLeader = c.Boolean(nullable: false),
                        ProjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ResearcherId)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Volunteers",
                c => new
                    {
                        volunteer_id = c.Int(nullable: false, identity: true),
                        first_name = c.String(),
                        last_name = c.String(),
                        email = c.String(),
                    })
                .PrimaryKey(t => t.volunteer_id);
            
            CreateTable(
                "dbo.PhysicianDepartments",
                c => new
                    {
                        Physician_physician_id = c.Int(nullable: false),
                        Department_department_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Physician_physician_id, t.Department_department_id })
                .ForeignKey("dbo.Physicians", t => t.Physician_physician_id, cascadeDelete: true)
                .ForeignKey("dbo.Departments", t => t.Department_department_id, cascadeDelete: true)
                .Index(t => t.Physician_physician_id)
                .Index(t => t.Department_department_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Researchers", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Projects", "Lab_LabId", "dbo.Labs");
            DropForeignKey("dbo.Projects", "LabId", "dbo.Labs");
            DropForeignKey("dbo.Labs", "Project_ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Availabilities", "physician_id", "dbo.Physicians");
            DropForeignKey("dbo.Availabilities", "department_id", "dbo.Departments");
            DropForeignKey("dbo.PhysicianDepartments", "Department_department_id", "dbo.Departments");
            DropForeignKey("dbo.PhysicianDepartments", "Physician_physician_id", "dbo.Physicians");
            DropIndex("dbo.PhysicianDepartments", new[] { "Department_department_id" });
            DropIndex("dbo.PhysicianDepartments", new[] { "Physician_physician_id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Researchers", new[] { "ProjectId" });
            DropIndex("dbo.Projects", new[] { "Lab_LabId" });
            DropIndex("dbo.Projects", new[] { "LabId" });
            DropIndex("dbo.Labs", new[] { "Project_ProjectId" });
            DropIndex("dbo.Availabilities", new[] { "department_id" });
            DropIndex("dbo.Availabilities", new[] { "physician_id" });
            DropTable("dbo.PhysicianDepartments");
            DropTable("dbo.Volunteers");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Researchers");
            DropTable("dbo.Projects");
            DropTable("dbo.Labs");
            DropTable("dbo.Physicians");
            DropTable("dbo.Departments");
            DropTable("dbo.Availabilities");
        }
    }
}
