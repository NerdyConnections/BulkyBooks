using BulkyBook.DAL;
using BulkyBook.DAL.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ProductController(IUnitOfWork unitOfWork , IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> objProductList = _unitOfWork.Product.GetAll();
            return View(objProductList);
        }
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem 
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),

            };
          
            if (id == null || id == 0)
            {

                //create product
               
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);
               
                   
                    return View(productVM);
                
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            //,IFormFile? file
            if (ModelState.IsValid)
            {

                //save image
                string wwwRootPath = _hostEnvironment.WebRootPath;
               if (file != null)
               {
                    //file is uploaded
                    //make sure file has an unique name
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                   var extension = Path.GetExtension(file.FileName);
                    //there is an existing image it is an update and need to delete old image
                    //if it is a create ImageUrl is not set til after it is copy to images folder
                    if (obj.Product.ImageUrl!=null)
                    {
                        //delete the old image ImageUrl has an extra \ at the front need to trim it before combine with wwwroot
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                   using (var fileStreams = new FileStream(Path.Combine(uploads,fileName + extension),FileMode.Create))
                   {
                      file.CopyTo(fileStreams);
                    }
                    obj.Product.ImageUrl = @"\images\products\" + fileName + extension;
                    //images uploaded and imageurl is set

               }
               if (obj.Product.Id==0)
                {
                    _unitOfWork.Product.Add(obj.Product);
                }else
                {
                    _unitOfWork.Product.Update(obj.Product);
                }
               
                _unitOfWork.Save();
                TempData["success"] = "Product added successfully";
                return RedirectToAction("Index");

            }
            else
            {

            }
            return View(obj);
        }
          
      
       //API calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");
            return Json(new { data = productList });
        }

        [HttpDelete]
       
        public IActionResult Delete(int? id)
        {
            var objProductFromDB = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);
            if (objProductFromDB == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, objProductFromDB.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _unitOfWork.Product.Remove(objProductFromDB);//find primary key update all properties
            _unitOfWork.Save();


            return   Json(new { success = true, message = "Delete Successful" });
        }
    }
}
