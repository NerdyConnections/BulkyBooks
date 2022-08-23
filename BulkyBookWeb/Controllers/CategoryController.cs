using BulkyBook.DAL;
using BulkyBook.Models;

using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList=_db.Categories.ToList();
            return View(objCategoryList);
        }
        public IActionResult Create()
        {
            
            return View();
        }
[HttpPost]
[ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (obj.Name==obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Custom", "The DisplayOrder and Name cannot be same");
                //ModelState.AddModelError("Name", "The DisplayOrder and Name cannot be same");

            }
            if (!ModelState.IsValid)
            {
                return View(obj);
            }
            _db.Categories.Add(obj);
            _db.SaveChanges();
            TempData["success"] = "Category created successfully";

            return RedirectToAction("Index");
        }
        public IActionResult Edit(int? id)
        {
            if(id==null || id ==0)
            {
                return NotFound();
            }
            var categoryFromDb = _db.Categories.Find(id);
            if (categoryFromDb==null)
            {
                return NotFound();
            }
          
            return View(categoryFromDb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Custom", "The DisplayOrder and Name cannot be same");
                //ModelState.AddModelError("Name", "The DisplayOrder and Name cannot be same");

            }
            if (!ModelState.IsValid)
            {
                return View(obj);
            }
            _db.Categories.Update(obj);//find primary key update all properties
            _db.SaveChanges();

            TempData["success"] = "Category updated successfully";
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryFromDb = _db.Categories.Find(id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Category obj)
        {
           var objCategoryFromDB = _db.Categories.Find(obj.Id);
            _db.Categories.Remove(objCategoryFromDB);//find primary key update all properties
            _db.SaveChanges();

            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
