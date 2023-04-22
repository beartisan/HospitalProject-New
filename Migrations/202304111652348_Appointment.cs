namespace hospital_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Appointment : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PhysicianDepartments", newName: "DepartmentPhysicians");
            DropPrimaryKey("dbo.DepartmentPhysicians");
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        appointment_id = c.Int(nullable: false, identity: true),
                        appointment_name = c.String(),
                        appointment_date = c.DateTime(nullable: false),
                        patient_id = c.Int(nullable: false),
                        physician_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.appointment_id)
                .ForeignKey("dbo.Patients", t => t.patient_id, cascadeDelete: true)
                .ForeignKey("dbo.Physicians", t => t.physician_id, cascadeDelete: true)
                .Index(t => t.patient_id)
                .Index(t => t.physician_id);
            
            AddPrimaryKey("dbo.DepartmentPhysicians", new[] { "Department_department_id", "Physician_physician_id" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "physician_id", "dbo.Physicians");
            DropForeignKey("dbo.Appointments", "patient_id", "dbo.Patients");
            DropIndex("dbo.Appointments", new[] { "physician_id" });
            DropIndex("dbo.Appointments", new[] { "patient_id" });
            DropPrimaryKey("dbo.DepartmentPhysicians");
            DropTable("dbo.Appointments");
            AddPrimaryKey("dbo.DepartmentPhysicians", new[] { "Physician_physician_id", "Department_department_id" });
            RenameTable(name: "dbo.DepartmentPhysicians", newName: "PhysicianDepartments");
        }
    }
}
