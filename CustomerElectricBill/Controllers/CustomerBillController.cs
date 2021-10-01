using Azure.Storage.Blobs;
using CustomerElectricBill.Data;
using CustomerElectricBill.Data.Repository;
using CustomerElectricBill.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CustomerElectricBill.Controllers
{
    public class CustomerBillController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CustomerBillController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(int? id)
        {
            return RedirectToAction("GetBill", new { id = id });
        }

        //Get-Bill
        public IActionResult GetBill(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var bill = _db.CustomerBills.ToList().Where(x => x.UserId == id);
            if (bill == null)
            {
                return NotFound();
            }
            return View(bill);
        }

        public IActionResult AddBillAndUpload()
        {
            return View();
        }

        //Post-Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBillAndUpload(BillingCreateModel bill)
        {
            ViewBag.id = bill.UserId;
            if (ModelState.IsValid)
            {
                if (bill.Document != null)
                {
                    if (DateTime.Now.Day < 28)
                    {
                        var url = await CustomerRepository.UploadToBlob(bill.Document);
                        Billing billing = new Billing()
                        {
                            UserId = bill.UserId,
                            readings = bill.readings,
                            DocumentUrl = url
                        };
                        _db.CustomerBills.Add(billing);
                        _db.SaveChanges();
                        return RedirectToAction("GetBill", new { id = bill.UserId });
                    }
                    else
                    {
                        ViewBag.ErrorMsg = $"Cannot Add bills before 28/{DateTime.Now.Month}/{DateTime.Now.Year}";
                        return View(bill);
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = $"Upload bill documents";
                    return View(bill);
                }
            }
            return View(bill);
        }

        //Get-Delete
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var bill = _db.CustomerBills.Find(id);
            if (bill == null)
            {
                return NotFound();
            }
            return View(bill);
        }

        //Post-Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteBill(int? txid)
        {

            var bill = _db.CustomerBills.Find(txid);
            if (bill == null)
            {
                return NotFound();
            }
            _db.CustomerBills.Remove(bill);
            _db.SaveChanges();
            return RedirectToAction("GetBill", new { id = bill.UserId });
        }


        //Get-Update
        public IActionResult Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var bill = _db.CustomerBills.Find(id);
            if (bill == null)
            {
                return NotFound();
            }
            return View(bill);
        }

        //Post-Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateBill(Billing bill)
        {
            ViewBag.id = bill.UserId;
            if (ModelState.IsValid)
            {
                _db.CustomerBills.Update(bill);
                _db.SaveChanges();
                return RedirectToAction("GetBill", new { id = bill.UserId });
            }
            return View(bill);
        }


        public IActionResult Upload(List<IFormFile> files)
        {
            return View();
        }

        private async Task<string> SaveAsync(IFormFile file)
        {
            try
            {
                CloudStorageAccount storageacc = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=wheelerbattery;AccountKey=udujsBjC4dy82H93IbEOc8IkzpZ6kibV6zoTMqkVFJzsQ0bNar5QgTvvKAdYdzDUAsVIH11eJZsgecY4tojk2Q==;EndpointSuffix=core.windows.net");
                CloudBlobClient blobClient = storageacc.CreateCloudBlobClient();
                CloudBlobContainer blobContainer = blobClient.GetContainerReference("test");
                await blobContainer.CreateIfNotExistsAsync();
                CloudBlockBlob blob = blobContainer.GetBlockBlobReference(file.FileName);
                var task = blob.UploadFromStreamAsync(file.OpenReadStream(), file.Length);
                return blob.Uri.ToString();
            }
            catch (Exception e)
            {

                return e.Message;
            }


        }

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFileAsync(List<IFormFile> files)
        {
            var size = files.Sum(f => f.Length);
            var filePaths = new List<string>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), formFile.FileName);
                    filePaths.Add(filePath);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        var url = await SaveAsync(formFile);
                    }
                }
            }
            return Ok(new { files.Count, size, filePaths });
        }
    }
}
