namespace MvcAffableBean.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newtable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserEmail = c.String(),
                        UserPassword = c.String(),
                        UserRole = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AppUsers");
        }
    }
}
