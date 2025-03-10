using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Template.Models;
namespace Template.Controllers
{
    public class HomeController : Controller
    {
       PharmacyEntities1 pharmacy = new PharmacyEntities1();
        public ActionResult Index(string error = null)
        {
            ViewBag.medicines = pharmacy.Medicines.ToList();
            ViewBag.patients = pharmacy.Patients.ToList();
            ViewBag.error = error;
            List<Sale> sales = pharmacy.Sales.ToList();
            return View(sales);
        }
        public ActionResult Login() 
        {
            ViewBag.error = null;
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            User loginUser = pharmacy.Users.Where(a => a.Username.Equals(username) && a.Password.Equals(password) && a.Isactive == true).FirstOrDefault();
            if (loginUser != null)
                return RedirectToAction("Index");
            ViewBag.error = "Hatalı giriş";
            return View();
        }
        [HttpPost]
        public ActionResult Add(int PatientID, int MedicineID, int amount)
        {
            List<Sale> sales = pharmacy.Sales.Where(a=> a.PatientID == PatientID ).ToList();
            if(sales.Count >=3)
            {
                return RedirectToAction("Index", new {error = "A patient can purchase a maximum of 3 medications." });
            }
            else
            {
                Sale selection = pharmacy.Sales.Where(a => a.PatientID == PatientID && a.MedicineID == MedicineID).FirstOrDefault();
                if (selection == null)
                {
                    Sale newSale = new Sale();
                    newSale.PatientID = PatientID;
                    newSale.MedicineID = MedicineID;
                    newSale.amount = amount;
                    pharmacy.Sales.Add(newSale);
                    pharmacy.SaveChanges();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index", new { error = "This medicine is already taken" });

            }

        }
        public ActionResult Remove(int SalesID)
        {
            Sale deleted = pharmacy.Sales.Find(SalesID);
            pharmacy.Sales.Remove(deleted);
            pharmacy.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}