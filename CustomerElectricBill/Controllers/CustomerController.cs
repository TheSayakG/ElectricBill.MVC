using CustomerElectricBill.Data;
using CustomerElectricBill.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CustomerElectricBill.Controllers
{
    public class CustomerController : Controller
    {

        private readonly ApplicationDbContext _db;
        public CustomerController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<CustomerModel> customers = _db.Customers;
            return View(customers);
        }

        //GET-Create
        public IActionResult AddCustomers()
        {
            return View();
        }

        //Post-Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCustomers(CustomerModel customer)
        {
            if (ModelState.IsValid)
            {
                _db.Customers.Add(customer);
                _db.SaveChanges();
                return RedirectToAction("GetBill","CustomerBill", new { id = customer.UserId });
            }
            return View(customer);
        }

        //Get-Delete
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var customer = _db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        //Post-Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCustomers(int? UserId)
        {
            var customer = _db.Customers.Find(UserId);
            if (customer == null)
            {
                return NotFound();
            }
            _db.Customers.Remove(customer);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Get-Update
        public IActionResult Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var customer = _db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        //Post-Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateCustomer(CustomerModel customer)
        {
            if (ModelState.IsValid)
            {
                if (customer.Status == false)
                {
                    try
                    {
                        var password = "9933020286";
                        var sub = "Deactivation Alert";
                        var body = $@"Your Account has been deactivated.{Environment.NewLine}Please Share your feedback in the link https://localhost:44350/FeedBack/AddReview";
                        using (var mail = new MailMessage())
                        {
                            mail.From = new MailAddress("skghosh381@gmail.com");
                            mail.To.Add("skghosh380@gmail.com");
                            mail.Subject = sub;
                            mail.Body = body;
                            mail.IsBodyHtml = false;

                            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtpClient.Credentials = new NetworkCredential("skghosh381@gmail.com", password);
                                smtpClient.EnableSsl = true;
                                smtpClient.Send(mail);
                            }
                        }
                    }
                    catch (Exception e)
                    {

                        ViewBag.Error = e.Message;
                    }
                }
                _db.Customers.Update(customer);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }



    }
}
