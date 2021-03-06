﻿using System;
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
    public class CompanyLocationsController : Controller
    {
        private readonly CareerCloudContext _context;

        public CompanyLocationsController(CareerCloudContext context)
        {
            _context = context;
        }

        // GET: CompanyLocations
        public async Task<IActionResult> Index(Guid? id)
        {
            var careerCloudContext = _context.CompanyLocations
                .Where(c => c.Company == id);
            if(careerCloudContext.Count() == 0)
            {
                ViewData["Company"] = id;
                return View("~/Views/CompanyLocations/AddNewLocation.cshtml");
            }
            return View(await careerCloudContext.ToListAsync());
        }

        // GET: CompanyLocations/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyLocationPoco = await _context.CompanyLocations
                .Include(c => c.CompanyProfilePoco)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (companyLocationPoco == null)
            {
                return NotFound();
            }

            return View(companyLocationPoco);
        }

        // GET: CompanyLocations/Create
        public IActionResult Create()
        {
            ViewData["Company"] = new SelectList(_context.CompanyProfiles, "Id", "Id");
            return View();
        }

        // POST: CompanyLocations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Company,CountryCode,Province,Street,City,PostalCode")] CompanyLocationPoco companyLocationPoco)
        {
            if (ModelState.IsValid)
            {
                companyLocationPoco.Id = Guid.NewGuid();
                _context.Add(companyLocationPoco);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Company"] = new SelectList(_context.CompanyProfiles, "Id", "Id", companyLocationPoco.Company);
            return View(companyLocationPoco);
        }

        // GET: CompanyLocations/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyLocationPoco = await _context.CompanyLocations.FindAsync(id);
            if (companyLocationPoco == null)
            {
                return NotFound();
            }
            ViewData["Company"] = new SelectList(_context.CompanyProfiles, "Id", "Id", companyLocationPoco.Company);
            return View(companyLocationPoco);
        }

        // POST: CompanyLocations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Company,CountryCode,Province,Street,City,PostalCode")] CompanyLocationPoco companyLocationPoco)
        {
            if (id != companyLocationPoco.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(companyLocationPoco);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyLocationPocoExists(companyLocationPoco.Id))
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
            ViewData["Company"] = new SelectList(_context.CompanyProfiles, "Id", "Id", companyLocationPoco.Company);
            return View(companyLocationPoco);
        }

        // GET: CompanyLocations/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyLocationPoco = await _context.CompanyLocations
                .Include(c => c.CompanyProfilePoco)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (companyLocationPoco == null)
            {
                return NotFound();
            }

            return View(companyLocationPoco);
        }

        // POST: CompanyLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var companyLocationPoco = await _context.CompanyLocations.FindAsync(id);
            _context.CompanyLocations.Remove(companyLocationPoco);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyLocationPocoExists(Guid id)
        {
            return _context.CompanyLocations.Any(e => e.Id == id);
        }
    }
}
