using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpenseManager_WafaM.Models.ViewModels
{
    public class UpdateExpense
    {
        public ExpenseDto Expense { get; set; }

        public IEnumerable<CategoryDto> Categories { get; set; }
    }
}
