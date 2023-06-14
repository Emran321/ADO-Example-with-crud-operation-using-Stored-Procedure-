using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ADO_Example1.DAL;
using ADO_Example1.Models;

namespace ADO_Example1.Controllers
{
    public class ProductController : Controller
    {
        ProductDAL productDAL = new ProductDAL();
        // GET: Product
        public ActionResult Index()
        {
            var ProductList = productDAL.GetAllProducts();
            if (ProductList.Count == 0)
            {
                TempData["infoMessage"] = "Currently There are no Products available in the Database.";
            }
            return View(ProductList);
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var product = productDAL.GetProductsByID(id).FirstOrDefault();
                if (product == null)
                {
                    TempData["infoMessage"] = "Products not available in the Database" + id.ToString();
                    return RedirectToAction("Index");
                }
                return View(product);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        // GET: Product/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(Product product)
        {
            bool IsInserted = false;
            try
            {
                if (ModelState.IsValid)
                {
                    IsInserted = productDAL.InsertProduct(product);
                    if (IsInserted)
                    {
                        TempData["SuccessMessage"] = "Product Details saved Successfully";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Product is Already Exists / Unable to save product";
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            var products = productDAL.GetProductsByID(id).FirstOrDefault();
            if (products == null)
            {
                TempData["infoMessage"] = "Products not available in the Database" + id.ToString();
                return RedirectToAction("Index");
            }
            return View(products);
        }

        // POST: Product/Edit/5
        [HttpPost, ActionName("Edit")]
        public ActionResult UpdateProduct(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool Isupdated = productDAL.UpdatetProduct(product);
                    if (Isupdated)
                    {
                        TempData["SuccessMessage"] = "Product Updated Successfully";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Unable to Updated product";
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            var product = productDAL.GetProductsByID(id).FirstOrDefault();

            try
            {
                if (product == null)
                {
                    TempData["infoMessage"] = "Products not available in the Database" + id.ToString();
                    return RedirectToAction("Index");
                }
                return View(product);
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmation(int id)
        {
            try
            {
                string result = productDAL.DeleteProduct(id);
                if (result.Contains("Deleted"))
                {
                    TempData["SuccessMessage"] = result;
                }
                else
                {
                    TempData["ErrorMessage"] = result;
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }
    }
}
