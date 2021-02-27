using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ExpenseManager_WafaM.Models;

namespace ExpenseManager_WafaM.Controllers
{
    public class ExpenseDataController : ApiController
    {
        //[Authorize]
        private ExpensesDbContext db = new ExpensesDbContext();

        /// <summary>
        /// Gets a list of expenses. This list can only be accessed when the user is logged in
        /// Code reference: varsity_w_auth
        /// </summary>
        /// <returns>A list of expenses including thier ID and categories there are related to</returns>
        /// <example>
        ///GET: api/ExpenseData/GetExpenses
        /// </example>

        [ResponseType(typeof(IEnumerable<ExpenseDto>))]
        [Route("api/ExpenseData/GetExpenses")]
        public IHttpActionResult GetExpenses()
        {
            List<Expense> Expenses = db.Expenses.ToList();
            List<ExpenseDto> ExpenseDtos = new List<ExpenseDto> { };

            //Listing the info we would like to show using a foreach loop
            foreach (var Expense in Expenses)
            {
                //for each expense in the DB these are the columns that will be displayed on the user end
                ExpenseDto NewExpense = new ExpenseDto
                {
                    ItemId = Expense.ItemId,
                    ItemName = Expense.ItemName,
                    Amount = Expense.Amount,
                    ExpenseDate = Expense.ExpenseDate,
                    //I messed up here and realized I added category id as the fk instead of using category name...
                    CategoryId = Expense.CategoryId

                };


                ExpenseDtos.Add(NewExpense);
            }

            return Ok(ExpenseDtos);
        }


        /// <summary>
        /// Gets expense by itemid.
        /// Code reference: varsity_w_auth
        /// </summary>
        /// <param name="ItemId">input expense id</param>
        /// <returns>A list of expenses associated by item name</returns>
        /// <example>
        ///GET: api/ExpenseData/FindExpense/{id}
        /// </example>
        [HttpGet]
        [ResponseType(typeof(ExpenseDto))]
        public IHttpActionResult FindExpense(int id)
        {
            Expense Expense = db.Expenses.Find(id);

            if (Expense == null)
            {
                return NotFound();

            }
            ExpenseDto ExpenseDto = new ExpenseDto
            {
                ItemId = Expense.ItemId,
                ItemName = Expense.ItemName,
                Amount = Expense.Amount,
                ExpenseDate = Expense.ExpenseDate,
                CategoryId = Expense.CategoryId
            };

                return Ok(ExpenseDto);
        }

        /// <summary>
        /// update expenses in the database with the given information  
        /// Code reference: varsity_w_auth
        /// </summary>
        /// <param name="id">itemId as input</param>
        /// <param name="name">itemId as input</param>
        /// <returns></returns>
        /// <example>
        ///POST:api/ExpenseData/UpdateExpense/{id}
        /// </example>

        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateExpense(int id, [FromBody] Expense Expense)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Expense.ItemId)
            {
                return BadRequest();
            }

            db.Entry(Expense).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException) 
            { 
                if (!ExpenseExists(id))
                {
                    return NotFound();

                } else
                {

                    throw;

                }
            }


            return StatusCode(HttpStatusCode.NoContent);


        }

        /// <summary>
        /// Adds expense to the database 
        /// Code reference: varsity_w_auth
        /// </summary>
        /// <param name="Expense">itemId as input</param>
        /// <returns>status code 200 if sucessful else status code 400 if unsuccessful</returns>
        /// <example>
        ///POST:api/ExpenseData/AddExpense
        /// </example>
        [ResponseType(typeof(Expense))]
        [HttpPost]
        public IHttpActionResult AddExpense([FromBody] Expense Expense)
        {
            //Will ensure valid annotation are inputed as defined in the Model 
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Expenses.Add(Expense);
            db.SaveChanges();

            return Ok(Expense.ItemId);
        }

        /// <summary>
        /// Deletes expense from the database
        /// </summary>
        /// <param name="id">itemId as input</param>
        /// <returns></returns>
        /// <example>
        ///POST:api/ExpenseData/DeleteExpense
        /// </example>
        [HttpPost]
        public IHttpActionResult DeleteExpense(int id)
        {
            Expense expense = db.Expenses.Find(id);
            if (expense == null)
            {
                return NotFound();
            }

            db.Expenses.Remove(expense);
            db.SaveChanges();

            return Ok();
        }

        ///<summary>
        ///Finds expense in the system 
        ///</summary>
        ///<param name="id">Expnese id</param>
        ///<returns>True if expense exists </returns>
        private bool ExpenseExists(int id)
        {
            return db.Expenses.Count(e => e.ItemId == id) > 0;
        }
    }

}

/// <summary>
/// Gets a list of expenses by itemname. This list can only be accessed when the user is logged in this code is not working 
/// Code reference: varsity_w_auth
/// </summary>
/// <param name="ItemName">input expense name</param>
/// <returns>A list of expenses associated by item name</returns>
/// <example>
///GET: api/CategoryData/GetExpensesByName/{ItemName}
/// </example>

//not working!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
/*[ResponseType(typeof(IEnumerable<ExpenseDto>))]
public IHttpActionResult GetExpensesByName(string ItemName)
{
    //Pseudo code:: for each expense in expense db output the itemId, itemname, amount and category for the particular expense date
    List<Expense> Expenses = db.Expenses.Where(e => e.ItemName == ItemName).ToList();
    List<ExpenseDto> ExpenseDtos = new List<ExpenseDto> { };

    //This is where we are able to pick what information will be displayed
    foreach (var Expense in Expenses)
    {
        ExpenseDto NewExpense = new ExpenseDto
        {
            ItemId = Expense.ItemId,
            ItemName = Expense.ItemName,
            Amount = Expense.Amount,
            ExpenseDate = Expense.ExpenseDate,
            CategoryId = Expense.CategoryId
        };

        ExpenseDtos.Add(NewExpense);
    }

    return Ok(ExpenseDtos);
}*/