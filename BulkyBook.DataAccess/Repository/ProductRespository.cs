using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    internal class ProductRespository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;

        public ProductRespository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
        public void Update(Product product)
        {
            var productFromDb = _db.Products.FirstOrDefault(product => product.Id == product.Id);
            if (productFromDb != null)
            {
                productFromDb.Title = product.Title;
                productFromDb.Description = product.Description;
                productFromDb.Price = product.Price;
                productFromDb.ListPrice = product.ListPrice;
                productFromDb.Price50= product.Price50;
                productFromDb.Price100= product.Price100;
                productFromDb.Author = product.Author;
                productFromDb.CoverTypeId = product.CoverTypeId;   
                productFromDb.CategoryId = product.CategoryId;
                productFromDb.ISBN = product.ISBN;

                if (product.ImageUrl != null)
                {
                    productFromDb.ImageUrl = product.ImageUrl;
                }



            }
        }
    }
}
