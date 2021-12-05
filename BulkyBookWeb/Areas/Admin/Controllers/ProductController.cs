using BulkyBook.DataAccess.Repository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOrWork)
        {
            _unitOfWork = unitOrWork;   
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            var CategoryList = _unitOfWork.Category.getAll().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            var CoverTypeList = _unitOfWork.CoverType.getAll().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            ViewBag.CategoryList = CategoryList;
            ViewData["CoverTypeList"] = CoverTypeList;

            if (id == 0)
            {
                //Create product
                Product product = new Product();
                return View(product);
            }

            return View();

            
        }

        [HttpPost]
        public IActionResult Upsert(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            if(product != null)
            {
                if (product.Id == 0)
                    _unitOfWork.Product.Add(product);
                else
                    _unitOfWork.Product.Update(product);

                _unitOfWork.Save();
                    
            }

            return RedirectToAction("Index");

        }
    }
}
