using BulkyBook.DAL.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DAL.Repository
{
    public class UnitOfWork:IUnitOfWork
    {

        private readonly ApplicationDbContext _db;

       public ICategoryRepository Category { get; set; }

        public ICoverTypeRepository CoverType { get; set; }
        public IProductRepository Product { get; set; }
       
        public ICompanyRepository Company { get; set; }
        public IShoppingCartRepository ShoppingCart { get; set; }
        public IApplicationUserRepository ApplicationUser { get; set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            CoverType=new CoverTypeRepository(_db);
            Product = new ProductRepository(_db);
                 Company = new CompanyRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
        }
        public  void Save()
        {
            _db.SaveChanges();
        }
    }
}
