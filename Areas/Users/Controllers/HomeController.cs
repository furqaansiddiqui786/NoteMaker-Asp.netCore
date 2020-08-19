using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NoteMaker.Data;
using NoteMaker.Models;

namespace NoteMaker.Controllers
{
    [Area("Users")]
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _db;
        

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Notes()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var notes = await _db.Notes.Where(n => n.UserId == claim.Value).ToListAsync();

            return View(notes);
        }

        [HttpGet]
        public async Task<IActionResult> Notes(string search)
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ViewData["GetResult"] = search;
            var query = from x in _db.Notes.Where(c=>c.UserId == claim.Value) select x;
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Title.Contains(search) && x.UserId == claim.Value);
            }
            return View(await query.AsNoTracking().ToListAsync());
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Notes usernotes)
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                usernotes.UserId = claim.Value;
                _db.Notes.Add(usernotes);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Notes));
            }
            return View(usernotes);
        }

        public async Task<IActionResult> View(int id)
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var view = await _db.Notes.FirstOrDefaultAsync(c => c.Id == id && c.UserId == claim.Value);

            if (view != null)
            {
                return View(view);
            }

            else
            {
                return RedirectToAction(nameof(Notes));
            }

        }

        public async Task<IActionResult> Edit(int id)
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var notes = await _db.Notes.FirstOrDefaultAsync(c => c.Id == id && c.UserId == claim.Value);

            if (notes != null)
            {
                return View(notes);
            }

            else
            {
                return RedirectToAction(nameof(Notes));
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Notes notes)
        {
            if (ModelState.IsValid)
            {
                _db.Notes.Update(notes);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Notes));
            }
            return RedirectToAction("Notes");
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                var notes = await _db.Notes.FindAsync(id);
                if(notes != null)
                {
                    _db.Notes.Remove(notes);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Notes));
                }
            }
            return View();
        }

    }
}
