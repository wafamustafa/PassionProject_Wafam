using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpenseManager_WafaM.Models.ViewModels
{
    //created after Categorycontroller was coded to show expenses when category is selected
    public class ShowCategory
    {
        //info about the category
        public CategoryDto Category { get; set; }

        public IEnumerable<ExpenseDto> Expenses { get; set; }
    }
}