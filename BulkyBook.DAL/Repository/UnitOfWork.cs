using BulkyBook.DAL.Repository.IRepository;
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
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            CoverType=new CoverTypeRepository(_db);
            Product = new ProductRepository(_db);
                 Company = new CompanyRepository(_db);
        }
        public  void Save()
        {
            _db.SaveChanges();
        }
    }
}
