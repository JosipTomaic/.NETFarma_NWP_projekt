using Farma.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Farma.Controllers
{
    public class AnimalController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();
        [Authorize]
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.TrenutniSort = sortOrder;
            ViewBag.VrstaSort = sortOrder == "Vrsta" ? "Vrsta_desc" : "Vrsta";
            ViewBag.StarostSort = sortOrder == "Starost" ? "Starost_desc" : "Starost";
            ViewBag.TezinaSort = sortOrder == "Tezina" ? "Tezina_desc" : "Tezina";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.TrenutniFilter = searchString;

            var zivotinje = from z in db.Zivotinje select z;

            if (!String.IsNullOrEmpty(searchString))
            {
                zivotinje = zivotinje.Where(z => z.Vrsta.Contains(searchString) ||
                                          z.Starost.ToString().Equals(searchString) ||
                                          z.Tezina.ToString().Equals(searchString));
            }

            switch (sortOrder)
            {
                case "Vrsta":
                    zivotinje = zivotinje.OrderBy(s => s.Vrsta);
                    break;
                case "Vrsta_desc":
                    zivotinje = zivotinje.OrderByDescending(s => s.Vrsta);
                    break;
                case "Starost":
                    zivotinje = zivotinje.OrderBy(s => s.Starost);
                    break;
                case "Starost_desc":
                    zivotinje = zivotinje.OrderByDescending(s => s.Starost);
                    break;
                case "Tezina":
                    zivotinje = zivotinje.OrderBy(s => s.Tezina);
                    break;
                case "Tezina_desc":
                    zivotinje = zivotinje.OrderByDescending(s => s.Tezina);
                    break;
                default:
                    zivotinje = zivotinje.OrderBy(s => s.ID);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(zivotinje.ToPagedList(pageNumber, pageSize));
        }

        [Authorize]
        public ActionResult OtvoriFormu()
        {
            return View("Forma");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OtvoriFormu([Bind(Include = "ID,Vrsta,Tezina,Starost,Boja,Nadimak")] Stoka zivotinja)
        {
            if (ModelState.IsValid)
            {
                db.Zivotinje.Add(zivotinja);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Index", db.Zivotinje.ToList());
        }

        [Authorize]
        public ActionResult UrediZivotinju(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var odabranaZivotinja = db.Zivotinje.Find(id);
            if (odabranaZivotinja == null)
            {
                return HttpNotFound();
            }
            return View(odabranaZivotinja);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UrediZivotinju([Bind(Include = "ID,Vrsta,Starost,Tezina,Nadimak,Boja")] Stoka zivotinja)
        {
            if (ModelState.IsValid)
            {
                db.Set<Stoka>().AddOrUpdate(zivotinja);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(zivotinja);
        }

        [Authorize]
        public ActionResult PrikaziDetalje(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var odabranaZivotinja = db.Zivotinje.Find(id);
            if (odabranaZivotinja == null)
            {
                return HttpNotFound();
            }
            return View(odabranaZivotinja);
        }

        [Authorize]
        public ActionResult ObrisiZivotinju(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var odabranaZivotinja = db.Zivotinje.Find(id);
            if (odabranaZivotinja == null)
            {
                return HttpNotFound();
            }
            return View(odabranaZivotinja);
        }

        [Authorize]
        [HttpPost, ActionName("ObrisiZivotinju")]
        [ValidateAntiForgeryToken]
        public ActionResult PotvrdiBrisanje(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var odabranaZivotinja = db.Zivotinje.Find(id);
            if (odabranaZivotinja == null)
            {
                return HttpNotFound();
            }
            db.Zivotinje.Remove(odabranaZivotinja);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}