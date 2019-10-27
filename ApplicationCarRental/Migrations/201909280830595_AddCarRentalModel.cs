namespace ApplicationCarRental.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCarRentalModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CarRents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false),
                        CarId = c.Int(nullable: false),
                        StartDate = c.DateTime(),
                        ActualEndDate = c.DateTime(),
                        ScheduledEndDate = c.DateTime(),
                        AdditionalCharge = c.Double(),
                        RentalPrice = c.Double(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CarRents");
        }
    }
}
