using Chnage.Models;
using Chnage.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chnage.Repository
{
    public class ProductRepository : IProduct
    {
        private MyECODBContext db;

        public ProductRepository(MyECODBContext _db)
        {
            db = _db;

        }
        public IEnumerable<Product> GetProducts => db.Products;



        public void Add(Product _Product)
        {

            db.Products.Add(_Product);
            db.SaveChanges();

        }

        public Product GetProduct(int? ID)
        {
            Product dbEntity = db.Products.SingleOrDefault(i => i.Id == ID);
            return dbEntity;
        }

        public void Remove(int? ID)
        {
            Product dbEntity = db.Products.Find(ID);
            db.Products.Remove(dbEntity);
            db.SaveChanges();
        }

        public void Update(Product _Product)
        {
            db.Entry(_Product).State = EntityState.Modified;
            db.SaveChanges();

        }
    }
}
