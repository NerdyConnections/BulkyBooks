using BulkyBook.DAL;
using BulkyBook.DAL.Repository.IRepository;
using BulkyBook.Models;

using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        
        private readonly IUnitOfWork _unitOfWork;
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<CoverType> objCoverTypeList = _unitOfWork.CoverType.GetAll();
            return View(objCoverTypeList);
        }
        public IActionResult Create()
        {
            
            return View();
        }
[HttpPost]
[ValidateAntiForgeryToken]
        public IActionResult Create(CoverType obj)
        {
           
            if (!ModelState.IsValid)
            {
                return View(obj);
            }
            _unitOfWork.CoverType.Add(obj);
            _unitOfWork.Save();
            TempData["success"] = "Cover Type created successfully";

            return RedirectToAction("Index");
        }
        public IActionResult Edit(int? id)
        {
            if(id==null || id ==0)
            {
                return NotFound();
            }
            var coverTypeFromDb = _unitOfWork.CoverType.GetFirstOrDefault(x=>x.Id==id);
            if (coverTypeFromDb == null)
            {
                return NotFound();
            }
          
            return View(coverTypeFromDb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType obj)
        {
           
            if (!ModelState.IsValid)
            {
                return View(obj);
            }
            _unitOfWork.CoverType.Update(obj);//find primary key update all properties
            _unitOfWork.Save();

            TempData["success"] = "CoverType updated successfully";
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var ctFromDb = _unitOfWork.CoverType.GetFirstOrDefault(x => x.Id == id);
            if (ctFromDb == null)
            {
                return NotFound();
            }

            return View(ctFromDb);
        }
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
           var objCTFromDB = _unitOfWork.CoverType.GetFirstOrDefault(x => x.Id == id);
            _unitOfWork.CoverType.Remove(objCTFromDB);//find primary key update all properties
            _unitOfWork.Save();

             TempData["success"] = "CoverType deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
