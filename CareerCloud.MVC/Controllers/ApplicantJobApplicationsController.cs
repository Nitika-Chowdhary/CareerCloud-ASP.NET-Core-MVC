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
    public class ApplicantJobApplicationsController : Controller
    {
        private readonly CareerCloudContext _context;

        public ApplicantJobApplicationsController(CareerCloudContext context)
        {
            _context = context;
        }

        // GET: ApplicantJobApplications
        public async Task<IActionResult> Index(Guid? applicant)
        {
            if (applicant is null)
            {
                var careerCloudContext = _context.ApplicantJobApplications
                .Include(a => a.ApplicantProfilePoco)
                .Include(a => a.CompanyJobPoco);
                return View(await careerCloudContext.ToListAsync());
            }
            var careerCloudContext1 = _context.ApplicantJobApplications.Where(a => a.Applicant == applicant)
                .Include(a => a.ApplicantProfilePoco)
                .Include(a => a.CompanyJobPoco)
                    .ThenInclude(a => a.CompanyJobDescriptionPocos)
                .Include(a => a.CompanyJobPoco)
                    .ThenInclude(a => a.CompanyProfilePoco);
            return View(await careerCloudContext1.ToListAsync());
        }

        // GET: ApplicantJobApplications/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicantJobApplicationPoco = await _context.ApplicantJobApplications
                .Include(a => a.ApplicantProfilePoco)
                .Include(a => a.CompanyJobPoco)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicantJobApplicationPoco == null)
            {
                return NotFound();
            }

            return View(applicantJobApplicationPoco);
        }

        // GET: ApplicantJobApplications/Create
        public IActionResult Create()
        {
            ViewData["Applicant"] = new SelectList(_context.ApplicantProfiles, "Id", "Id");
            ViewData["Job"] = new SelectList(_context.CompanyJobs, "Id", "Id");
            return View();
        }

        // POST: ApplicantJobApplications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Applicant,Job,ApplicationDate")] ApplicantJobApplicationPoco applicantJobApplicationPoco)
        {
            if (ModelState.IsValid)
            {
                applicantJobApplicationPoco.Id = Guid.NewGuid();
                _context.Add(applicantJobApplicationPoco);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Applicant"] = new SelectList(_context.ApplicantProfiles, "Id", "Id", applicantJobApplicationPoco.Applicant);
            ViewData["Job"] = new SelectList(_context.CompanyJobs, "Id", "Id", applicantJobApplicationPoco.Job);
            return View(applicantJobApplicationPoco);
        }

        // GET: ApplicantJobApplications/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicantJobApplicationPoco = await _context.ApplicantJobApplications.FindAsync(id);
            if (applicantJobApplicationPoco == null)
            {
                return NotFound();
            }
            ViewData["Applicant"] = new SelectList(_context.ApplicantProfiles, "Id", "Id", applicantJobApplicationPoco.Applicant);
            ViewData["Job"] = new SelectList(_context.CompanyJobs, "Id", "Id", applicantJobApplicationPoco.Job);
            return View(applicantJobApplicationPoco);
        }

        // POST: ApplicantJobApplications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Applicant,Job,ApplicationDate")] ApplicantJobApplicationPoco applicantJobApplicationPoco)
        {
            if (id != applicantJobApplicationPoco.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicantJobApplicationPoco);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicantJobApplicationPocoExists(applicantJobApplicationPoco.Id))
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
            ViewData["Applicant"] = new SelectList(_context.ApplicantProfiles, "Id", "Id", applicantJobApplicationPoco.Applicant);
            ViewData["Job"] = new SelectList(_context.CompanyJobs, "Id", "Id", applicantJobApplicationPoco.Job);
            return View(applicantJobApplicationPoco);
        }

        // GET: ApplicantJobApplications/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicantJobApplicationPoco = await _context.ApplicantJobApplications
                .Include(a => a.ApplicantProfilePoco)
                .Include(a => a.CompanyJobPoco)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicantJobApplicationPoco == null)
            {
                return NotFound();
            }

            return View(applicantJobApplicationPoco);
        }

        // POST: ApplicantJobApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var applicantJobApplicationPoco = await _context.ApplicantJobApplications.FindAsync(id);
            _context.ApplicantJobApplications.Remove(applicantJobApplicationPoco);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicantJobApplicationPocoExists(Guid id)
        {
            return _context.ApplicantJobApplications.Any(e => e.Id == id);
        }
    }
}
