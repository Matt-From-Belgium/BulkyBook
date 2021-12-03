using BulkyBook.DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class CoverTypeController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var ctList = _unitOfWork.CoverType.getAll();
            return View(ctList);
        }
    }
}
