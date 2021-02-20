using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ExpenseManager_WafaM.Models;
using ExpenseManager_WafaM.Models.ViewModels;
using System.Diagnostics;

namespace ExpenseManager_WafaM.Controllers
{
    public class CategoryController : Controller
    {
        //Connect to the webAPI controller
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static CategoryController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44314/api/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: Category/List
        public ActionResult List()
        {
            string url = "CategoryData/GetCategories";
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                IEnumerable<CategoryDto> Categories = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;
                return View(Categories);
            } 
            else
            {
                return RedirectToAction("Error");
            }
            
        }

        // GET: Category/Details/5
        public ActionResult Details(int id)
        {
            //having issues with viewmodel (ShowCategory): results:: because Models folder didnt have a ViewModels for "ShowCategory" for one to many relationship defined.
            //Pseudo code: to get a list of expenses find the category by id then list the expenses with the category's FK in the expenses database

            //created Find category is categoryData controller to support get expenses by category. (this method is for internal use only)
            ShowCategory ViewModel = new ShowCategory();
            string url = "CategoryData/FindCategory/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Pseudo code: if the category we are looking for is there then show the expenses listed under that category
            if(response.IsSuccessStatusCode)
            {
                //get the category selected
                CategoryDto SelectedCategory = response.Content.ReadAsAsync<CategoryDto>().Result;
                ViewModel.Category = SelectedCategory;

                //now show the expenses under that category Id
                url = "CategoryData/GetExpensesForCategory/" + id;
                response = client.GetAsync(url).Result;
                IEnumerable<ExpenseDto> SelectedExpense = response.Content.ReadAsAsync<IEnumerable<ExpenseDto>>().Result;
                ViewModel.Expenses = SelectedExpense;

                return View(ViewModel);

            }
            else
            {
                return RedirectToAction("Error");

            }
            
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(Category Category)
        {
            Debug.WriteLine(Category.CategoryName);
            string url = "CategoryData/AddCategory";
            Debug.WriteLine(jss.Serialize(Category));
            HttpContent content = new StringContent(jss.Serialize(Category));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                int Categoryid = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = Categoryid });
            }
            else
            {
                return RedirectToAction("Error");
            }

        }


        //Using the Find method in CategoryDataController to get the categoryId to delete

        // GET: Category/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "CategoryData/FindCategory/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            
            if (response.IsSuccessStatusCode)
            {
                CategoryDto SelectedCategory = response.Content.ReadAsAsync<CategoryDto>().Result;
                return View(SelectedCategory);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Category/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "CategoryData/DeleteCategory/" + id;
 
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult Error()
        {
            return View();
        }
    
    }
}
