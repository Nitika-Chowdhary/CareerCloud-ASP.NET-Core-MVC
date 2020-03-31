using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CareerCloud.EntityFrameworkDataAccess;
using CareerCloud.Pocos;

namespace CareerCloud.MVC.Controllers
{
    public class ApplicantProfilesController : Controller
    {
        private readonly CareerCloudContext _context;

        public ApplicantProfilesController(CareerCloudContext context)
        {
            _context = context;
        }

        // GET: ApplicantProfiles
        public async Task<IActionResult> Index(Guid? login, Guid? id)
        {
            if (login is null)
            {
                var careerCloudContext1 = await _context.ApplicantProfiles.Where(a => a.Id == id).ToListAsync();
                if (careerCloudContext1.Count() == 0)
                {
                    ViewData["Country"] = new SelectList(_context.SystemCountryCodes, "Code", "Code");
                    return View("~/Views/ApplicantProfiles/AddNewProfile.cshtml");
                }
                return View(careerCloudContext1);
            }
            if (id is null)
            {
                var careerCloudContext2 = await _context.ApplicantProfiles.Where(a => a.Login == login).ToListAsync();
                if (careerCloudContext2.Count() == 0)
                {
                    ViewData["Login"] = login;
                    ViewData["Country"] = new SelectList(_context.SystemCountryCodes, "Code", "Code");
                    return View("~/Views/ApplicantProfiles/AddNewProfile.cshtml");
                }
                return View(careerCloudContext2);
            }
            return View();

            
        }

        // GET: ApplicantProfiles/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicantProfilePoco = await _context.ApplicantProfiles
                .Include(a => a.ApplicantSkills)
                .Include(a => a.ApplicantResumes)
                .Include(a => a.ApplicantEducation)
                .Include(a => a.ApplicantWorkHistoryPocos)
                .Include(a => a.SecurityLogin)
                .Include(a => a.SystemCountryCodePoco)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicantProfilePoco == null)
            {
                return NotFound();
            }

            return View(applicantProfilePoco);
        }

        // GET: ApplicantProfiles/Create
        public IActionResult Create()
        {
            ViewData["Login"] = new SelectList(_context.SecurityLogins, "Id", "Id");
            ViewData["Country"] = new SelectList(_context.SystemCountryCodes, "Code", "Code");
            return View();
        }

        // POST: ApplicantProfiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Login,CurrentSalary,CurrentRate,Currency,Country,Province,Street,City,PostalCode")] ApplicantProfilePoco applicantProfilePoco)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    applicantProfilePoco.Id = Guid.NewGuid();
                    _context.Add(applicantProfilePoco);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { id = applicantProfilePoco.Id });
                }
            }
            catch(DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists. " +
                    "See your system administrator.");
            }

            ViewData["Login"] = new SelectList(_context.SecurityLogins, "Id", "Id", applicantProfilePoco.Login);
            ViewData["Country"] = new SelectList(_context.SystemCountryCodes, "Code", "Code", applicantProfilePoco.Country);
            return View(applicantProfilePoco);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAndNavigate([Bind("Login,CurrentSalary,CurrentRate,Currency,Country,Province,Street,City,PostalCode")] ApplicantProfilePoco applicantProfilePoco)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    applicantProfilePoco.Id = Guid.NewGuid();
                    _context.Add(applicantProfilePoco);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), "ApplicantSkills", new {id = applicantProfilePoco.Id});
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists. " +
                    "See your system administrator.");
            }

            ViewData["Login"] = new SelectList(_context.SecurityLogins, "Id", "Id", applicantProfilePoco.Login);
            ViewData["Country"] = new SelectList(_context.SystemCountryCodes, "Code", "Code", applicantProfilePoco.Country);
            return View(applicantProfilePoco);
        }

        // GET: ApplicantProfiles/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicantProfilePoco = await _context.ApplicantProfiles.FindAsync(id);
            if (applicantProfilePoco == null)
            {
                return NotFound();
            }
            ViewData["Login"] = new SelectList(_context.SecurityLogins, "Id", "Id", applicantProfilePoco.Login);
            ViewData["Country"] = new SelectList(_context.SystemCountryCodes, "Code", "Code", applicantProfilePoco.Country);
            return View(applicantProfilePoco);
        }

        // POST: ApplicantProfiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Login,CurrentSalary,CurrentRate,Currency,Country,Province,Street,City,PostalCode")] ApplicantProfilePoco applicantProfilePoco)
        {
            if (id != applicantProfilePoco.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicantProfilePoco);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicantProfilePocoExists(applicantProfilePoco.Id))
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
            ViewData["Login"] = new SelectList(_context.SecurityLogins, "Id", "Id", applicantProfilePoco.Login);
            ViewData["Country"] = new SelectList(_context.SystemCountryCodes, "Code", "Code", applicantProfilePoco.Country);
            return View(applicantProfilePoco);
        }

        // GET: ApplicantProfiles/Delete/5
        public async Task<IActionResult> Delete(Guid? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicantProfilePoco = await _context.ApplicantProfiles
                .Include(a => a.SecurityLogin)
                .Include(a => a.SystemCountryCodePoco)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicantProfilePoco == null)
            {
                return NotFound();
            }

            if(saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists, " +
                    "contact system administrator.";
            }

            return View(applicantProfilePoco);
        }

        // POST: ApplicantProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try {

                ApplicantProfilePoco applicantToDelete = new ApplicantProfilePoco() { Id = id };
                _context.Entry(applicantToDelete).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool ApplicantProfilePocoExists(Guid id)
        {
            return _context.ApplicantProfiles.Any(e => e.Id == id);
        }
    }
}
