using Farma.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace Farma.Controllers
{
    public class MachineController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.TrenutniSort = sortOrder;
            ViewBag.NazivSort = sortOrder == "Naziv" ? "Naziv_desc" : "Naziv";
            ViewBag.MarkaSort = sortOrder == "Marka" ? "Marka_desc" : "Marka";
            ViewBag.ModelSort = sortOrder == "Model" ? "Model_desc" : "Model";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.TrenutniFilter = searchString;

            var strojevi = from s in db.Strojevi select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                strojevi = strojevi.Where(s => s.Naziv.Contains(searchString) || 
                                          s.Marka.Contains(searchString) || 
                                          s.Model.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "Naziv":
                    strojevi = strojevi.OrderBy(s => s.Naziv);
                    break;
                case "Naziv_desc":
                    strojevi = strojevi.OrderByDescending(s => s.Naziv);
                    break;
                case "Marka":
                    strojevi = strojevi.OrderBy(s => s.Marka);
                    break;
                case "Marka_desc":
                    strojevi = strojevi.OrderByDescending(s => s.Marka);
                    break;
                case "Model":
                    strojevi = strojevi.OrderBy(s => s.Model);
                    break;
                case "Model_desc":
                    strojevi = strojevi.OrderByDescending(s => s.Model);
                    break;
                default:
                    strojevi = strojevi.OrderBy(s => s.ID);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(strojevi.ToPagedList(pageNumber, pageSize));
        }

        [Authorize]
        public ActionResult OtvoriFormu()
        {
            return View("Forma");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OtvoriFormu([Bind(Include = "ID,Naziv,Marka,Model,Svrha,GodisnjeDoba,Korisnik")]Stroj stroj)
        {
            if (ModelState.IsValid)
            {
                db.Strojevi.Add(stroj);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Index", db.Strojevi.ToList());
        }

        [Authorize]
        public ActionResult UrediStroj(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var odabraniStroj = db.Strojevi.Find(id);
            if (odabraniStroj == null)
            {
                return HttpNotFound();
            }
            return View(odabraniStroj);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UrediStroj([Bind(Include = "ID,Naziv,Marka,Model,Svrha,GodisnjeDoba")]Stroj stroj)
        {
            if (ModelState.IsValid)
            {
                db.Set<Stroj>().AddOrUpdate(stroj);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(stroj);
        }

        [Authorize]
        public ActionResult PrikaziDetalje(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var odabraniStroj = db.Strojevi.Find(id);
            if (odabraniStroj == null)
            {
                return HttpNotFound();
            }
            return View(odabraniStroj);
        }

        [Authorize]
        public ActionResult ObrisiStroj(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var odabraniStroj = db.Strojevi.Find(id);
            if (odabraniStroj == null)
            {
                return HttpNotFound();
            }
            return View(odabraniStroj);
        }

        [Authorize]
        [HttpPost, ActionName("ObrisiStroj")]
        [ValidateAntiForgeryToken]
        public ActionResult PotvrdiBrisanje(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var odabraniStroj = db.Strojevi.Find(id);
            if (odabraniStroj == null)
            {
                return HttpNotFound();
            }
            db.Strojevi.Remove(odabraniStroj);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult KoristiStroj(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var odabraniStroj = db.Strojevi.Find(id);
            if (odabraniStroj == null)
            {
                return HttpNotFound();
            }
            return View(odabraniStroj);
        }

        [Authorize]
        [HttpPost, ActionName("KoristiStroj")]
        [ValidateAntiForgeryToken]
        public ActionResult PotvrdiKoristenje(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var odabraniStroj = db.Strojevi.Find(id);
            if (odabraniStroj == null)
            {
                return HttpNotFound();
            }
            if (odabraniStroj.Upotreba)
            {
                odabraniStroj.Upotreba = false;
                odabraniStroj.Korisnik = "/";
            }
            else
            {
                odabraniStroj.Upotreba = true;
                odabraniStroj.Korisnik = User.Identity.Name;
            }
            db.Set<Stroj>().AddOrUpdate(odabraniStroj);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}