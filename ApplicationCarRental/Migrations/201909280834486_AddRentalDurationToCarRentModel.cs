namespace ApplicationCarRental.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRentalDurationToCarRentModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CarRents", "RentalDuration", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CarRents", "RentalDuration");
        }
    }
}
