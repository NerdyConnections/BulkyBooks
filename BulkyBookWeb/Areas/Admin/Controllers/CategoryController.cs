using BulkyBook.DAL;
using BulkyBook.DAL.Repository.IRepository;
using BulkyBook.Models;

using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();
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
            _unitOfWork.Category.Add(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category created successfully";

            return RedirectToAction("Index");
        }
        public IActionResult Edit(int? id)
        {
            if(id==null || id ==0)
            {
                return NotFound();
            }
            var categoryFromDb = _unitOfWork.Category.GetFirstOrDefault(x=>x.Id==id);
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
            _unitOfWork.Category.Update(obj);//find primary key update all properties
            _unitOfWork.Save();

            TempData["success"] = "Category updated successfully";
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryFromDb = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
           var objCategoryFromDB = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);
            _unitOfWork.Category.Remove(objCategoryFromDB);//find primary key update all properties
            _unitOfWork.Save();

             TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
