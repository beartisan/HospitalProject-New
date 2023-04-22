namespace hospital_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Patient : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Patients",
                c => new
                    {
                        patient_id = c.Int(nullable: false, identity: true),
                        healthcard_id = c.Int(nullable: false),
                        patient_fname = c.String(),
                        patient_surname = c.String(),
                        patient_birthday = c.DateTime(nullable: false),
                        patient_phoneNum = c.String(),
                        patient_condition = c.String(),
                    })
                .PrimaryKey(t => t.patient_id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Patients");
        }
    }
}
