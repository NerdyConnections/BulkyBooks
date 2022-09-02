using BulkyBook.DAL.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
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


            OrderVM = new OrderVM()
            {
                OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(x => x.Id == orderid, includeProperties: "ApplicationUser"),
                OrderDetails = _unitOfWork.OrderDetail.GetAll(x => x.OrderId == orderid, includeProperties: "Product")
            };
            return View(OrderVM);

        }
       [HttpPost]
       [ValidateAntiForgeryToken]
        public IActionResult UpdateOrderDetails()
            {

            var orderHeaderFromDB = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);
            orderHeaderFromDB.Name = OrderVM.OrderHeader.Name;
            orderHeaderFromDB.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
            orderHeaderFromDB.StreetAddress = OrderVM.OrderHeader.StreetAddress;
            orderHeaderFromDB.City = OrderVM.OrderHeader.City;
            orderHeaderFromDB.State = OrderVM.OrderHeader.State;
            orderHeaderFromDB.PostalCode = OrderVM.OrderHeader.PostalCode;
            if (OrderVM.OrderHeader.Carrier!=null)
            {
                orderHeaderFromDB.Carrier= OrderVM.OrderHeader.Carrier;
            }
            if (OrderVM.OrderHeader.TrackingNumber != null)
            {
                orderHeaderFromDB.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            }

            _unitOfWork.OrderHeader.Update(orderHeaderFromDB);
            _unitOfWork.Save();
            TempData["Success"] = "Order Details Updated Successfully.";
            return RedirectToAction("Details", "Order", new { orderid = orderHeaderFromDB.Id });


           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult StartProcessing()
        {

            _unitOfWork.OrderHeader.UpdateStatus(OrderVM.OrderHeader.Id, SD.StatusInProcess);
            _unitOfWork.Save();
         

        
            TempData["Success"] = "Order Details Updated Successfully.";
            return RedirectToAction("Details", "Order", new { orderid = OrderVM.OrderHeader.Id });



        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult ShipOrder()
        {
            var orderHeaderFromDB = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);
            orderHeaderFromDB.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            orderHeaderFromDB.Carrier = OrderVM.OrderHeader.Carrier;
            orderHeaderFromDB.OrderStatus = SD.StatusShipped;
            orderHeaderFromDB.ShippingDate = DateTime.Now;
            if (orderHeaderFromDB.PaymentStatus==SD.PaymentStatusDelayedPayment)
            {
                orderHeaderFromDB.PaymentDueDate = DateTime.Now.AddDays(30);
            }
            _unitOfWork.OrderHeader.Update(orderHeaderFromDB);
           
            _unitOfWork.Save();
            TempData["Success"] = "Order Shipped Successfully.";
            return RedirectToAction("Details", "Order", new { orderid = OrderVM.OrderHeader.Id });



        }
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        [ValidateAntiForgeryToken]
        public IActionResult CancelOrder()
        {
            var orderHeaderFromDB = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);
           if (orderHeaderFromDB.PaymentStatus==SD.PaymentStatusApproved)
                {
                //need to refund
                var options = new RefundCreateOptions
                {

                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeaderFromDB.PaymentIntentId
                    //by default the exact amount that is charged is refunded


                };
                var service = new RefundService();
                Refund refund = service.Create(options);
                _unitOfWork.OrderHeader.UpdateStatus(orderHeaderFromDB.Id, SD.StatusCancelled, SD.StatusRefunded);

            }
            else
            {
                _unitOfWork.OrderHeader.UpdateStatus(orderHeaderFromDB.Id, SD.StatusCancelled, SD.StatusCancelled);
            }
            _unitOfWork.Save();

          
            TempData["Success"] = "Order Cancelled Successfully.";
            return RedirectToAction("Details", "Order", new { orderid = OrderVM.OrderHeader.Id });



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
