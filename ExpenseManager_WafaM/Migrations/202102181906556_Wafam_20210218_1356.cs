namespace ExpenseManager_WafaM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Wafam_20210218_1356 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "CategoryName", c => c.String());
            DropColumn("dbo.Categories", "CateoryName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Categories", "CateoryName", c => c.String());
            DropColumn("dbo.Categories", "CategoryName");
        }
    }
}
