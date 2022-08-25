using BulkyBook.DAL;
using BulkyBook.DAL.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public CompanyController(IUnitOfWork unitOfWork , IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Company> objCompanyList = _unitOfWork.Company.GetAll();
            return View(objCompanyList);
        }
        public IActionResult Upsert(int? id)
        {
            Company obj = new Company();
          
            if (id == null || id == 0)
            {

                //create product
               
                return View(obj);
            }
            else
            {
                //update
                obj = _unitOfWork.Company.GetFirstOrDefault(x => x.Id == id);
               
                   
                    return View(obj);
                
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj)
        {
            //,IFormFile? file
            if (ModelState.IsValid)
            {

                //save image
               
                    //file is uploaded
                    //make sure file has an unique name
                
               }
               if (obj.Id==0)
                {
                    _unitOfWork.Company.Add(obj);
                }else
                {
                    _unitOfWork.Company.Update(obj);
                }
               
                _unitOfWork.Save();
                TempData["success"] = "Company added successfully";
                return RedirectToAction("Index");

          
            return View(obj);
        }
          
      
       //API calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var companyList = _unitOfWork.Company.GetAll();
            return Json(new { data = companyList });
        }

        [HttpDelete]
       
        public IActionResult Delete(int? id)
        {
            var objCompanyFromDB = _unitOfWork.Company.GetFirstOrDefault(x => x.Id == id);
            if (objCompanyFromDB == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
         
            _unitOfWork.Company.Remove(objCompanyFromDB);//find primary key update all properties
            _unitOfWork.Save();


            return   Json(new { success = true, message = "Delete Successful" });
        }
    }
}
