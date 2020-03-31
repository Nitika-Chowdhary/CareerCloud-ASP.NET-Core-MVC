using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CareerCloud.EntityFrameworkDataAccess;
using CareerCloud.Pocos;
using CareerCloud.MVC.Models;

namespace CareerCloud.MVC.Controllers
{
    public class CompanyProfilesController : Controller
    {
        private readonly CareerCloudContext _context;

        public CompanyProfilesController(CareerCloudContext context)
        {
            _context = context;
        }

        // GET: CompanyProfiles
        public async Task<IActionResult> Index(string sortOrder, string searchProfile)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["Filter"] = searchProfile;
            List<CompanyProfilePoco> companyProfiles = await _context.CompanyProfiles.ToListAsync();
            List<CompanyViewModel> companyViewModels = new List<CompanyViewModel>();
            foreach(CompanyProfilePoco item in companyProfiles)
            {
                CompanyDescriptionPoco poco = _context.CompanyDescriptions.Where(c => c.Company == item.Id).FirstOrDefault();
                CompanyViewModel companyViewModel = new CompanyViewModel();
                companyViewModel.companyDescription = poco;
                companyViewModel.companyProfile = item;
                companyViewModels.Add(companyViewModel);
            }
            if (!String.IsNullOrEmpty(searchProfile))
            {
                companyViewModels = companyViewModels.Where(s =>
                {
                    if(s.companyProfile is null || s.companyProfile.ContactName is null)
                    {
                        return false;
                    }
                    return s.companyProfile.ContactName.Contains(searchProfile);
                }).ToList();
            }
            else if (ViewData["NameSortParm"] is "")
            {
                companyViewModels.Sort(
                    (a, b) =>
                    {
                        if (a.companyDescription is null || b.companyDescription is null)
                        {
                            return 0;
                        }
                        return a.companyDescription.CompanyName.CompareTo(b.companyDescription.CompanyName);
                    }
                    );
            }
            return View(companyViewModels);
        }

        // GET: CompanyProfiles/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyProfilePoco = await _context.CompanyProfiles
                .Include(c => c.CompanyJobPocos)
                .Include(c => c.CompanyDescriptionPocos)
                .Include(c => c.CompanyLocationPocos)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (companyProfilePoco == null)
            {
                return NotFound();
            }

            return View(companyProfilePoco);
        }

        // GET: CompanyProfiles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CompanyProfiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RegistrationDate,CompanyWebsite,ContactPhone,ContactName,CompanyLogo")] CompanyProfilePoco companyProfilePoco)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    companyProfilePoco.Id = Guid.NewGuid();
                    _context.Add(companyProfilePoco);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists. " +
                    "See your system administrator.");
            }

            return View(companyProfilePoco);
        }

        // GET: CompanyProfiles/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyProfilePoco = await _context.CompanyProfiles.FindAsync(id);
            if (companyProfilePoco == null)
            {
                return NotFound();
            }
            return View(companyProfilePoco);
        }

        // POST: CompanyProfiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,RegistrationDate,CompanyWebsite,ContactPhone,ContactName,CompanyLogo")] CompanyProfilePoco companyProfilePoco)
        {
            if (id != companyProfilePoco.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(companyProfilePoco);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyProfilePocoExists(companyProfilePoco.Id))
                    {
                        ModelState.AddModelError("", "Unable to save changes. " +
                            "Try again, and if the problem persists, " +
                            "See your system administrator.");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(companyProfilePoco);
        }

        // GET: CompanyProfiles/Delete/5
        public async Task<IActionResult> Delete(Guid? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyProfilePoco = await _context.CompanyProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (companyProfilePoco == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists, " +
                    "contact system administrator.";
            }

            return View(companyProfilePoco);
        }

        // POST: CompanyProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                CompanyProfilePoco companyToDelete = new CompanyProfilePoco() { Id = id };
                _context.Entry(companyToDelete).State = EntityState.Deleted;
                _context.CompanyProfiles.Remove(companyToDelete);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch(DbUpdateException)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }            
        }

        private bool CompanyProfilePocoExists(Guid id)
        {
            return _context.CompanyProfiles.Any(e => e.Id == id);
        }
    }
}
