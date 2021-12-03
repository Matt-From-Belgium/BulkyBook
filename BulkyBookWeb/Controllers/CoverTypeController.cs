﻿using BulkyBook.DataAccess.Repository;
using BulkyBook.Models;
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CoverType newCoverType)
        {
            if (!ModelState.IsValid)
            {
                return View(new CoverType());
            }

            _unitOfWork.CoverType.Add(newCoverType);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var coverTypeFromDb = _unitOfWork.CoverType.GetFirstOrDefault(c=>c.Id==id);
            if (coverTypeFromDb == null)
            {
                return NotFound();
            }

            return View(coverTypeFromDb);
        }

        [HttpPost]
        public IActionResult Edit(CoverType updatedCoverType)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedCoverType);
            }

            _unitOfWork.CoverType.Update(updatedCoverType);
            _unitOfWork.Save();

            return RedirectToAction("Index");
        }
    }
}
