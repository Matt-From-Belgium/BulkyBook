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

            

            if (id != 0 && id!=null)
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

                    if (productVM.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(fullFileName,FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = fullFileName;
                }

                if (productVM.Product.Id != 0)
                {
                    var productFromDb = _unitOfWork.Product.GetFirstOrDefault(p=>p.Id==productVM.Product.Id);
                    productFromDb.Title = productVM.Product.Title;
                    productFromDb.Description = productVM.Product.Description;
                    productFromDb.Price100 = productVM.Product.Price100;
                    productFromDb.ListPrice = productVM.Product.ListPrice;
                    productFromDb.ISBN = productVM.Product.ISBN;
                    productFromDb.Price = productVM.Product.Price;
                    productFromDb.Price50 = productVM.Product.Price50;
                    productFromDb.Author = productVM.Product.Author;
                    productFromDb.CategoryId = productVM.Product.CategoryId;
                    productFromDb.CoverTypeId = productVM.Product.CoverTypeId;

                    if(productVM.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productFromDb.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }

                        productFromDb.ImageUrl = productVM.Product.ImageUrl;
                    }

                    _unitOfWork.Product.Update(productFromDb);

                }
                else
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
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
