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
    //[Authorize]
    public class CategoryDataController : ApiController
    {
        private ExpensesDbContext db = new ExpensesDbContext();

        /// <summary>
        /// Gets a list of categories. This list can only be accessed when the user is logged in
        /// Code reference: varsity_w_auth
        /// </summary>
        /// <returns>A list of categories including thier ID</returns>
        /// <example>
        ///GET: api/CategoryData/GetCategories
        /// </example>

        [ResponseType(typeof(IEnumerable<CategoryDto>))]
        public IHttpActionResult GetCategories()
        {
            List<Category> Categories = db.Categories.ToList();
            List<CategoryDto> CategoryDtos = new List<CategoryDto> { };

            //Listing the info we would like to show using a foreach loop
            foreach (var Category in Categories)
            {
                //for each category in the DB these are the columns that will be displayed on the user end
                CategoryDto NewCategory = new CategoryDto
                {
                    CategoryId = Category.CategoryId,
                    CategoryName = Category.CategoryName

                };


                CategoryDtos.Add(NewCategory);
            }

            return Ok(CategoryDtos);
        }

        ///<summary>
        ///INTERNAL USE ONLY
        ///Finds the category with the id provided. This is in support to finding expenses related to a particular category
        ///</summary>
        ///<param name="id">Category id</param>
        ///<returns>category name by Id</returns>
        ///<example>
        ///GET: api/CategoryData/FindCategory/{id}
        ///</example>
        [HttpGet]
        [ResponseType(typeof(CategoryDto))]
        public IHttpActionResult FindCategory(int id)
        {
            Category Category = db.Categories.Find(id);

            if (Category == null)
            {
                return NotFound();
            }

            CategoryDto CategoryDto = new CategoryDto
            {
                CategoryId = Category.CategoryId,
                CategoryName = Category.CategoryName
            };

            return Ok(CategoryDto);
        }

        

        /// <summary>
        /// Gets a list of expenses. This list can only be accessed when the user is logged in
        /// Code reference: varsity_w_auth
        /// </summary>
        /// <param name="id">input categpry id</param>
        /// <returns>A list of expenses associated with the category Id</returns>
        /// <example>
        ///GET: api/CategoryData/GetExpensesForCategory/{id}
        /// </example>


        [ResponseType(typeof(IEnumerable<ExpenseDto>))]
        public IHttpActionResult GetExpensesForCategory(int id)
        {
            //Pseudo code:: for each expense in category db output the itemId, itemname, amount and date for the particular category
            List<Expense> Expenses = db.Expenses.Where(e => e.CategoryId == id).ToList();
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
        }


        /// <summary>
        /// Adding more categories to the categories table as life can be very unexpected
        /// Code reference: varsity_w_auth
        /// </summary>
        /// <param name="CategoryName"> POST request form data</param>
        /// <returns>status code of 200 if successful or 400 if not successful</returns>
        /// <example>
        ///POST: api/CategoryData/AddCategory
        /// </example>

        [ResponseType(typeof(Category))]
        [HttpPost]
        public IHttpActionResult AddCategory([FromBody] Category Category)
        {
            //this will validated to see in the data annotation mataches. User will be entering a string of categoryName. CategoryId is auto generated. 
            //Pseudo code:: if value entered is NOT VALID return a 400 code if it is valid then save the changes
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Categories.Add(Category);
            db.SaveChanges();

            return Ok(Category.CategoryName);
        }

        /// <summary>
        /// Deleting a Catregory from the database.
        /// Most of the code was already created in the template
        /// </summary>
        /// <param name=id> the id of the category being deleted</param>
        /// <returns>status code of 200 if successful or 400 if not successful</returns>
        /// <example>
        ///GET: api/CategoryData/DeleteCategory/{id}
        /// </example>

        [HttpPost]
        public IHttpActionResult DeleteCategory(int id)
        {
            //Pseudo code:: if the category doesnt exists return not found. if there is a category with the corresponding id remove and save changes
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            db.Categories.Remove(category);
            db.SaveChanges();

            return Ok(category);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}