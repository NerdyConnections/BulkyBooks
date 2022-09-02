using BulkyBook.DAL.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DAL.Repository
{

    //it gets most implementaton from the Repository<Category> except update
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private ApplicationDbContext _db;
        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(OrderHeader obj)
        {
            _db.OrderHeaders.Update(obj);
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var OrderHeader = _db.OrderHeaders.FirstOrDefault(x => x.Id == id);
            if (OrderHeader != null)
            {
                OrderHeader.OrderStatus = orderStatus;
                if (paymentStatus != null)
                {
                    OrderHeader.PaymentStatus = paymentStatus;
                }
            }
        }
        public void UpdateStripePaymentID(int OrderHeaderId, string sessionId, string PaymentIntentId)
        {
            var OrderHeader = _db.OrderHeaders.FirstOrDefault(x => x.Id == OrderHeaderId);
            if (OrderHeader != null)
            {
                OrderHeader.PaymentDate = DateTime.Now;
                OrderHeader.SessionId = sessionId;
                OrderHeader.PaymentIntentId = PaymentIntentId;
            }
        }
    }
}
