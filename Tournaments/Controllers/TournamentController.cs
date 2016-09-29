using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Tournaments.Models;
using PagedList;

namespace Tournaments.Controllers
{
    public class TournamentController : Controller
    {
        private const int PageSize = 5;

        private ApplicationDbContext db = System.Web.HttpContext.Current
            .GetOwinContext().Get<ApplicationDbContext>();

        // GET: /Tournament/
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.errorMessage = string.Empty;
            ViewBag.currentUserId = User.Identity.GetUserId();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewBag.DateSortParam = String.IsNullOrEmpty(sortOrder) ? "" : "date_desc";
            ViewBag.SportSortParam = sortOrder == "sport_asc" ? "sport_desc" : "sport_asc";
            ViewBag.OwnerSortParam = sortOrder == "owner_asc" ? "owner_desc" : "owner_asc";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;
            
            var tournaments = from s in db.Tournaments
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                tournaments = tournaments.Where(s => s.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    tournaments = tournaments.OrderByDescending(s => s.Name);
                    break;
                case "name_asc":
                    tournaments = tournaments.OrderBy(s => s.Name);
                    break;
                case "date_desc":
                    tournaments = tournaments.OrderByDescending(s => s.Date);
                    break;
                case "sport_asc":
                    tournaments = tournaments.OrderBy(s => s.Sport);
                    break;
                case "sport_desc":
                    tournaments = tournaments.OrderByDescending(s => s.Sport);
                    break;
                case "owner_asc":
                    tournaments = tournaments.OrderBy(s => s.Owner.UserName);
                    break;
                case "owner_desc":
                    tournaments = tournaments.OrderByDescending(s => s.Owner.UserName);
                    break;
                default:
                    tournaments = tournaments.OrderBy(s => s.Date);
                    break;
            }

            return View(tournaments.ToPagedList(page ?? 1, PageSize));
        }

        // GET: /Tournament/Details/5
        [Authorize]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = db.Tournaments.Find(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }

            ViewBag.errorMessage = string.Empty;
            ViewBag.currentUserId = User.Identity.GetUserId();
            return View(tournament);
        }

        // GET: /Tournament/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Tournament/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "ID,Name,Sport,Date,RegistrationDeadline,Address,MaxPlayersNumber")] Tournament tournament)
        {
            if (ModelState.IsValid)
            {
                tournament.Owner = System.Web.HttpContext.Current.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId());
                db.Tournaments.Add(tournament);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tournament);
        }

        // GET: /Tournament/Edit/5
        [Authorize]
        [HandleError]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = db.Tournaments.Find(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }

            if (tournament.Owner.Id != User.Identity.GetUserId())
            {
                ViewBag.errorMessage = "Insufficient privileges to edit the tournament. Only owners can edit their tournaments.";
                return View("Error");
            }

            return View(tournament);
        }

        // POST: /Tournament/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "ID,Name,Sport,Date,RegistrationDeadline,Address,MaxPlayersNumber")] Tournament tournament)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tournament).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tournament);
        }

        // GET: /Tournament/Delete/5
        [Authorize]
        [HandleError]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = db.Tournaments.Find(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }

            if (tournament.Owner.Id != User.Identity.GetUserId())
            {
                ViewBag.errorMessage = "Insufficient privileges to delete the tournament. Only owners can delete their tournaments.";
                return View("Error");
            }

            return View(tournament);
        }

        // POST: /Tournament/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(long id)
        {
            Tournament tournament = db.Tournaments.Find(id);
            db.Tournaments.Remove(tournament);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
