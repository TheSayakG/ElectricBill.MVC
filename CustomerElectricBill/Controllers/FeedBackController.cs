using CustomerElectricBill.Data;
using CustomerElectricBill.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerElectricBill.Controllers
{
    public class FeedBackController : Controller
    {
        private readonly ApplicationDbContext _db;
        public FeedBackController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddReview()
        {
            return View();
        }

        //Post-Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddReview(FeedbackModel feedback)
        {
            ViewBag.id = feedback.UserId;
            if (ModelState.IsValid)
            {
                _db.FeedBacks.Add(feedback);
                _db.SaveChanges();
                return Ok("Thanks For your feedback");
            }
            else
                return View(feedback);
        }
    }

}
