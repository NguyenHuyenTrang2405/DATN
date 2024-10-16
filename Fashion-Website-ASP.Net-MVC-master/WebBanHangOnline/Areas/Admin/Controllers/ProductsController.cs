﻿using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Web.Mvc;
using System.IO;
using System.Net;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Employee")]
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Products
        public ActionResult Index(int? page)
        {
            var products = db.Products
                     .Include(p => p.ProductQuantities)  // Bao gồm thông tin về ProductQuantities
                     .OrderByDescending(x => x.id)
                     .ToList();

            // Tính toán tổng số lượng cho mỗi sản phẩm
            foreach (var product in products)
            {
                product.Quantity = product.ProductQuantities.Sum(pq => pq.QuantityProduct);
            }
            IEnumerable<Product> items = db.Products.OrderByDescending(x => x.id).ToList();
            var pageSize = 50;
            if (page == null)
            {
                page = 1;
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }


        public ActionResult Add()
        {
           
            ViewBag.ProductCategory = new SelectList(db.ProductCategories.ToList(), "id", "Title");
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Product model, List<string> Images, List<int> rDefault)
        {

            if (Images == null || Images.Count == 0)
            {
                TempData["ErrorMessage"] = "Vui lòng thêm ít nhất một hình ảnh cho sản phẩm.";
                ViewBag.ProductCategory = new SelectList(db.ProductCategories.ToList(), "id", "Title");
                return View(model);
            }
            if (ModelState.IsValid)
            {
                for (int i = 0; i < Images.Count; i++)
                {
                    if (i + 1 == rDefault[0])
                    {
                        model.Image = Images[i];

                        model.ProductImage.Add(new ProductImage
                        {
                            ProductId = model.id,
                            Image = Images[i],
                            IsDefault = true
                        });
                    }
                    else
                    {
                        model.ProductImage.Add(new ProductImage
                        {
                            ProductId = model.id,
                            Image = Images[i],
                            IsDefault = false
                        });
                    }

                }
                model.CreatedDate = DateTime.Now;
                model.Modifieddate = DateTime.Now;

                if (string.IsNullOrEmpty(model.SeoTitle))
                {
                    model.SeoTitle = model.Title;
                }
                if (string.IsNullOrEmpty(model.Alias))
                {
                    model.Alias = WebBanHangOnline.Models.Common.Filter.FilterChar(model.Title);
                }

                db.Products.Add(model);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Thêm sản phẩm thành công!";
                return RedirectToAction("Index");

            }
            ViewBag.ProductCategory = new SelectList(db.ProductCategories.ToList(), "id", "Title");

            return View(model);
        }
        public ActionResult Edit(int id)
        {
            ViewBag.ProductCategory = new SelectList(db.ProductCategories.ToList(), "id", "Title");
            
            var item = db.Products.Find(id);
          
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Product model)
        {
            if (ModelState.IsValid)
            {
                model.Modifieddate = DateTime.Now;

                model.Alias = WebBanHangOnline.Models.Common.Filter.FilterChar(model.Title);
                db.Products.Attach(model);
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "Có lỗi xảy ra khi cập nhật sản phẩm. Vui lòng kiểm tra lại!";
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                db.Products.Remove(item);
                db.SaveChanges();
                return Json(new { success = true, message = "Xóa thành công !" });
            }
            return Json(new { success = false, message = "Xóa thất bại!" });
        }
        public ActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var obj = db.Products.Find(Convert.ToInt32(item));
                        db.Products.Remove(obj);
                        db.SaveChanges();
                    }
                }
                return Json(new { success = true, message = "Xóa thành công !" });
            }
            return Json(new { success = false, message = "Xóa thất bại!" });
        }
        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, isActive = item.IsActive });
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public ActionResult IsHome(int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                item.IsHome = !item.IsHome;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, isHome = item.IsHome });
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public ActionResult IsSale(int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                item.IsSale = !item.IsSale;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, isSale = item.IsSale });
            }
            return Json(new { success = false });
        }
  

    }
}