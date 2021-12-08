using BulkyBook.DataAccess.Repository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfWork unitOrWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOrWork;   
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVm = new();
            productVm.CategoryList = _unitOfWork.Category.getAll().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            productVm.CoverTypeList = _unitOfWork.CoverType.getAll().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            

            if (id != 0)
            {
                var productFromDb = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == id);

                if(productFromDb == null)
                {
                    return NotFound();
                }

                productVm.Product = productFromDb;
                    
                
            }

            return View(productVm);

            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                return View(productVM);
            }

            if(productVM != null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);
                    var fullFileName = Path.Combine(uploads, fileName + extension);

                    using (var fileStream = new FileStream(fullFileName,FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = fullFileName;
                }

                
                _unitOfWork.Product.Add(productVM.Product);
                TempData["Success"] = "Product added succesfully";
                

                _unitOfWork.Save();
                    
            }

            return RedirectToAction("Index");

        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Product.getAll(includeProperties:"Category,CoverType");
            return Json(new { Data = productList }) ; 
    
        }
        #endregion
    }
}
