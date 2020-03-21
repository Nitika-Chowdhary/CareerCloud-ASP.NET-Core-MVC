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
    public class ApplicantSkillsController : Controller
    {
        private readonly CareerCloudContext _context;

        public ApplicantSkillsController(CareerCloudContext context)
        {
            _context = context;
        }

        // GET: ApplicantSkills
        public async Task<IActionResult> Index(Guid? id)
        {
            if (id == null)
                return BadRequest();

            var careerCloudContext = _context.ApplicantSkills.Where(a => a.Applicant == id);
            if (careerCloudContext.Count() == 0)
                return NotFound();

            return View(await careerCloudContext.ToListAsync());
        }

        // GET: ApplicantSkills/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicantSkillPoco = await _context.ApplicantSkills
                .Include(a => a.ApplicantProfilePoco)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicantSkillPoco == null)
            {
                return NotFound();
            }

            return View(applicantSkillPoco);
        }

        // GET: ApplicantSkills/Create
        public IActionResult Create()
        {
            ViewData["Applicant"] = new SelectList(_context.ApplicantProfiles, "Id", "Id");
            return View();
        }

        // POST: ApplicantSkills/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Applicant,Skill,SkillLevel,StartMonth,StartYear,EndMonth,EndYear")] ApplicantSkillPoco applicantSkillPoco)
        {
            if (ModelState.IsValid)
            {
                applicantSkillPoco.Id = Guid.NewGuid();
                _context.Add(applicantSkillPoco);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Applicant"] = new SelectList(_context.ApplicantProfiles, "Id", "Id", applicantSkillPoco.Applicant);
            return View(applicantSkillPoco);
        }

        // GET: ApplicantSkills/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicantSkillPoco = await _context.ApplicantSkills.FindAsync(id);
            if (applicantSkillPoco == null)
            {
                return NotFound();
            }
            ViewData["Applicant"] = new SelectList(_context.ApplicantProfiles, "Id", "Id", applicantSkillPoco.Applicant);
            return View(applicantSkillPoco);
        }

        // POST: ApplicantSkills/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Applicant,Skill,SkillLevel,StartMonth,StartYear,EndMonth,EndYear")] ApplicantSkillPoco applicantSkillPoco)
        {
            if (id != applicantSkillPoco.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicantSkillPoco);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicantSkillPocoExists(applicantSkillPoco.Id))
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
            ViewData["Applicant"] = new SelectList(_context.ApplicantProfiles, "Id", "Id", applicantSkillPoco.Applicant);
            return View(applicantSkillPoco);
        }

        // GET: ApplicantSkills/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicantSkillPoco = await _context.ApplicantSkills
                .Include(a => a.ApplicantProfilePoco)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicantSkillPoco == null)
            {
                return NotFound();
            }

            return View(applicantSkillPoco);
        }

        // POST: ApplicantSkills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var applicantSkillPoco = await _context.ApplicantSkills.FindAsync(id);
            _context.ApplicantSkills.Remove(applicantSkillPoco);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicantSkillPocoExists(Guid id)
        {
            return _context.ApplicantSkills.Any(e => e.Id == id);
        }
    }
}
