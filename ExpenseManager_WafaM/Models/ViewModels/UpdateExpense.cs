using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpenseManager_WafaM.Models.ViewModels
{
    public class UpdateExpense
    {
        public ExpenseDto Expense { get; set; }

        public string ItemName { get; set; }

        public decimal Amount { get; set; }

        public int CategoryId { get; set; }

        public DateTime ExpenseDate { get; set; }

        public IEnumerable<CategoryDto> Categories { get; set; }

        public string CategoryName { get; set; }
    }
}
