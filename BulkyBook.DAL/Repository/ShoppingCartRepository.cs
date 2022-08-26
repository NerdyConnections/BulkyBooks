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
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private ApplicationDbContext _db;
        public ShoppingCartRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
       

        public int DecrementCount(ShoppingCart obj, int count)
        {
            obj.Count -= count;
            return obj.Count;
      
        }
        public int IncrementCount(ShoppingCart obj, int count)
        {
            obj.Count += count;
            return obj.Count;

        }
    }
}
