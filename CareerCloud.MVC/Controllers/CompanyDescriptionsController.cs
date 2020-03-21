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
    public class CompanyDescriptionsController : Controller
    {
        private readonly CareerCloudContext _context;

        public CompanyDescriptionsController(CareerCloudContext context)
        {
            _context = context;
        }

        // GET: CompanyDescriptions
        public async Task<IActionResult> Index()
        {
            var careerCloudContext = _context.CompanyDescriptions.Include(c => c.CompanyProfilePoco).Include(c => c.SystemLanguageCodePoco);
            return View(await careerCloudContext.ToListAsync());
        }

        // GET: CompanyDescriptions/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyDescriptionPoco = await _context.CompanyDescriptions
                .Include(c => c.CompanyProfilePoco)
                .Include(c => c.SystemLanguageCodePoco)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (companyDescriptionPoco == null)
            {
                return NotFound();
            }

            return View(companyDescriptionPoco);
        }

        // GET: CompanyDescriptions/Create
        public IActionResult Create()
        {
            ViewData["Company"] = new SelectList(_context.CompanyProfiles, "Id", "Id");
            ViewData["LanguageId"] = new SelectList(_context.SystemLanguageCodes, "LanguageID", "LanguageID");
            return View();
        }

        // POST: CompanyDescriptions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Company,LanguageId,CompanyName,CompanyDescription")] CompanyDescriptionPoco companyDescriptionPoco)
        {
            if (ModelState.IsValid)
            {
                companyDescriptionPoco.Id = Guid.NewGuid();
                _context.Add(companyDescriptionPoco);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Company"] = new SelectList(_context.CompanyProfiles, "Id", "Id", companyDescriptionPoco.Company);
            ViewData["LanguageId"] = new SelectList(_context.SystemLanguageCodes, "LanguageID", "LanguageID", companyDescriptionPoco.LanguageId);
            return View(companyDescriptionPoco);
        }

        // GET: CompanyDescriptions/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyDescriptionPoco = await _context.CompanyDescriptions.FindAsync(id);
            if (companyDescriptionPoco == null)
            {
                return NotFound();
            }
            ViewData["Company"] = new SelectList(_context.CompanyProfiles, "Id", "Id", companyDescriptionPoco.Company);
            ViewData["LanguageId"] = new SelectList(_context.SystemLanguageCodes, "LanguageID", "LanguageID", companyDescriptionPoco.LanguageId);
            return View(companyDescriptionPoco);
        }

        // POST: CompanyDescriptions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Company,LanguageId,CompanyName,CompanyDescription")] CompanyDescriptionPoco companyDescriptionPoco)
        {
            if (id != companyDescriptionPoco.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(companyDescriptionPoco);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyDescriptionPocoExists(companyDescriptionPoco.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Company"] = new SelectList(_context.CompanyProfiles, "Id", "Id", companyDescriptionPoco.Company);
            ViewData["LanguageId"] = new SelectList(_context.SystemLanguageCodes, "LanguageID", "LanguageID", companyDescriptionPoco.LanguageId);
            return View(companyDescriptionPoco);
        }

        // GET: CompanyDescriptions/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyDescriptionPoco = await _context.CompanyDescriptions
                .Include(c => c.CompanyProfilePoco)
                .Include(c => c.SystemLanguageCodePoco)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (companyDescriptionPoco == null)
            {
                return NotFound();
            }

            return View(companyDescriptionPoco);
        }

        // POST: CompanyDescriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var companyDescriptionPoco = await _context.CompanyDescriptions.FindAsync(id);
            _context.CompanyDescriptions.Remove(companyDescriptionPoco);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyDescriptionPocoExists(Guid id)
        {
            return _context.CompanyDescriptions.Any(e => e.Id == id);
        }
    }
}
