namespace hospital_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class patientupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Patients", "physician_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Patients", "physician_id");
            AddForeignKey("dbo.Patients", "physician_id", "dbo.Physicians", "physician_id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Patients", "physician_id", "dbo.Physicians");
            DropIndex("dbo.Patients", new[] { "physician_id" });
            DropColumn("dbo.Patients", "physician_id");
        }
    }
}
