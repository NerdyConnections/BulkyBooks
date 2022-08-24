using BulkyBook.DAL;
using BulkyBook.DAL.Repository.IRepository;
using BulkyBook.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> objProductList = _unitOfWork.Product.GetAll();
            return View(objProductList);
        }
        public IActionResult Upsert(int? id)
        {
            Product product = new Product();
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(
                u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }
            );
            IEnumerable<SelectListItem> CoverTypeList = _unitOfWork.CoverType.GetAll().Select(
                u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }
            );
            if (id == null || id == 0)
            {

                //create product
                ViewBag.CategoryList = CategoryList;
                ViewBag.CoverTypeList = CoverTypeList;
                return View(product);
            }
            else
            {
                //update
                var productFromDb = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);
                if (productFromDb == null)
                {
                    return NotFound();
                }
                else
                {
                   
                    return View(productFromDb);
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Product product)
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
