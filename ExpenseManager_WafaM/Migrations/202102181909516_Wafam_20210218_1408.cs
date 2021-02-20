namespace ExpenseManager_WafaM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Wafam_20210218_1408 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Expenses", "CategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.Expenses", "CategoryId");
            AddForeignKey("dbo.Expenses", "CategoryId", "dbo.Categories", "CategoryId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Expenses", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Expenses", new[] { "CategoryId" });
            DropColumn("dbo.Expenses", "CategoryId");
        }
    }
}
