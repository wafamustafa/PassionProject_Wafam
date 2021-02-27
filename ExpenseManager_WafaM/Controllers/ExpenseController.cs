
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using ExpenseManager_WafaM.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Web.Script.Serialization;
using ExpenseManager_WafaM.Models.ViewModels;

namespace ExpenseManager_WafaM.Controllers
{
    public class ExpenseController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static ExpenseController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };

            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44314/api/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


        }

        // GET: Expense/List
        public ActionResult List()
        {
            string url = "ExpenseData/GetExpenses";
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                IEnumerable<ExpenseDto> Expenses = response.Content.ReadAsAsync<IEnumerable<ExpenseDto>>().Result;
                return View(Expenses);
            }
            else
            {
                return RedirectToAction("Error");
            }

        }




        //to create new expense user will be able to pick from category 
        // GET: Expense/Create
        public ActionResult Create()
        {
            UpdateExpense ViewModel = new UpdateExpense();
            string url = "CategoryData/GetCategories";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<CategoryDto> AllCategories =response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;
            ViewModel.Categories = AllCategories;


            return View(ViewModel);
        }
        // POST: Expense/Create
        [HttpPost]
        public ActionResult Create(Expense expense)
        {
            //getting lost in this chunck of code
            string url = "ExpenseData/AddExpense";
            HttpContent content = new StringContent(jss.Serialize(expense));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if(response.IsSuccessStatusCode)
            {
                int ItemId = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = ItemId });

            } else
            {
                return RedirectToAction("Error");
            }
        }




        //first find the expense then edit
        //this code was copied from varsity_w_auth while watching the video to understand what requests were being sent and recieved 
        // GET: Expense/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            UpdateExpense ViewModel = new UpdateExpense();
            string url = "ExpenseData/FindExpense" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                ExpenseDto SelectedExpense = response.Content.ReadAsAsync<ExpenseDto>().Result;
                ViewModel.Expense = SelectedExpense;

                url = "CategoryData/GetCategory";
                response = client.GetAsync(url).Result;
                IEnumerable<CategoryDto> PickedCategory = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;
                ViewModel.Categories = PickedCategory;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        // POST: Expense/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Update(int id)
        {
            string url = "ExpenseData/UpdateExpense/" + id;

            HttpContent content = new StringContent(jss.Serialize(id));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("List", new { id = id });

            }
            else
            {
                return RedirectToAction("Error");
            }
        }

            // GET: Expense/Delete/5
            public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Expense/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
