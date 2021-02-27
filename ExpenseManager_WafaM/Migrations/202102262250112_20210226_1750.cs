namespace ExpenseManager_WafaM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20210226_1750 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Expenses", "CategoryId", "dbo.Categories");
            AddColumn("dbo.Categories", "Expense_ItemId", c => c.Int());
            AddColumn("dbo.Expenses", "Category_CategoryId", c => c.Int());
            CreateIndex("dbo.Categories", "Expense_ItemId");
            CreateIndex("dbo.Expenses", "Category_CategoryId");
            AddForeignKey("dbo.Categories", "Expense_ItemId", "dbo.Expenses", "ItemId");
            AddForeignKey("dbo.Expenses", "Category_CategoryId", "dbo.Categories", "CategoryId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Expenses", "Category_CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Categories", "Expense_ItemId", "dbo.Expenses");
            DropIndex("dbo.Expenses", new[] { "Category_CategoryId" });
            DropIndex("dbo.Categories", new[] { "Expense_ItemId" });
            DropColumn("dbo.Expenses", "Category_CategoryId");
            DropColumn("dbo.Categories", "Expense_ItemId");
            AddForeignKey("dbo.Expenses", "CategoryId", "dbo.Categories", "CategoryId", cascadeDelete: true);
        }
    }
}
