using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExpenseManager_WafaM.Models
{
    //database set up for the expense sheet
    public class Expense
    {
        [Key]
        public int ItemId { get; set; }

       //[Required]-->taken out to manually update Db values
        [Display(Name = "Expense Name")]
        public string ItemName { get; set; }

        public decimal Amount { get; set; }

        //FK to relate expense to thier appropiate categories
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        //only date shown no time
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy")]
        public DateTime ExpenseDate { get; set; }

        

    }

    public class ExpenseDto
    {
        public int ItemId { get; set; }

        public string ItemName { get; set; }

        public decimal Amount { get; set; }

        public int CategoryId { get; set; }

        public DateTime ExpenseDate { get; set; }

    }

}