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
    public class SystemLanguageCodesController : Controller
    {
        private readonly CareerCloudContext _context;

        public SystemLanguageCodesController(CareerCloudContext context)
        {
            _context = context;
        }

        // GET: SystemLanguageCodes
        public async Task<IActionResult> Index()
        {
            return View(await _context.SystemLanguageCodes.ToListAsync());
        }

        // GET: SystemLanguageCodes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemLanguageCodePoco = await _context.SystemLanguageCodes
                .FirstOrDefaultAsync(m => m.LanguageID == id);
            if (systemLanguageCodePoco == null)
            {
                return NotFound();
            }

            return View(systemLanguageCodePoco);
        }

        // GET: SystemLanguageCodes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SystemLanguageCodes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LanguageID,Name,NativeName")] SystemLanguageCodePoco systemLanguageCodePoco)
        {
            if (ModelState.IsValid)
            {
                _context.Add(systemLanguageCodePoco);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(systemLanguageCodePoco);
        }

        // GET: SystemLanguageCodes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemLanguageCodePoco = await _context.SystemLanguageCodes.FindAsync(id);
            if (systemLanguageCodePoco == null)
            {
                return NotFound();
            }
            return View(systemLanguageCodePoco);
        }

        // POST: SystemLanguageCodes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("LanguageID,Name,NativeName")] SystemLanguageCodePoco systemLanguageCodePoco)
        {
            if (id != systemLanguageCodePoco.LanguageID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(systemLanguageCodePoco);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SystemLanguageCodePocoExists(systemLanguageCodePoco.LanguageID))
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
            return View(systemLanguageCodePoco);
        }

        // GET: SystemLanguageCodes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemLanguageCodePoco = await _context.SystemLanguageCodes
                .FirstOrDefaultAsync(m => m.LanguageID == id);
            if (systemLanguageCodePoco == null)
            {
                return NotFound();
            }

            return View(systemLanguageCodePoco);
        }

        // POST: SystemLanguageCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var systemLanguageCodePoco = await _context.SystemLanguageCodes.FindAsync(id);
            _context.SystemLanguageCodes.Remove(systemLanguageCodePoco);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SystemLanguageCodePocoExists(string id)
        {
            return _context.SystemLanguageCodes.Any(e => e.LanguageID == id);
        }
    }
}
