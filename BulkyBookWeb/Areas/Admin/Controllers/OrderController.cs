using BulkyBook.DAL.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private IUnitOfWork _unitOfWork;
        [BindProperty]
        public OrderVM OrderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details(int orderid)
        {
            IEnumerable<OrderDetail> orderDetails;

            OrderVM = new OrderVM()
            {
                OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(x => x.Id == orderid, includeProperties: "ApplicationUser"),
                OrderDetails = _unitOfWork.OrderDetail.GetAll(x => x.OrderId == orderid, includeProperties: "Product")


        };
            

            return View();
        }
        //API calls
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> orderHeaders;
            if (User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
            {
                orderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser");
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var applicationUserId = claim.Value;
                orderHeaders = _unitOfWork.OrderHeader.GetAll(x=>x.ApplicationUserId== applicationUserId, includeProperties: "ApplicationUser");

            }
            if (status == "pending")
            {
                orderHeaders = orderHeaders.Where(x => x.PaymentStatus == SD.PaymentStatusDelayedPayment);
            }
            else if (status == "inprocess")
            {
                orderHeaders = _unitOfWork.OrderHeader.GetAll(x => x.OrderStatus == SD.StatusInProcess);
            }
            else if (status=="completed")
            {
                orderHeaders = _unitOfWork.OrderHeader.GetAll(x => x.OrderStatus == SD.StatusShipped);
            }
            else if (status == "approved")
            {
                orderHeaders = _unitOfWork.OrderHeader.GetAll(x => x.OrderStatus == SD.StatusApproved);
            }
            else if (status=="all")
            {
               //return all orders do nothing
            }
          

            return Json(new { data = orderHeaders });
        }
    }
}
