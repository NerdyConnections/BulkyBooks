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
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
       

        public void Update(Category obj)
        {
            _db.Categories.Update(obj);
        }
    }
}
