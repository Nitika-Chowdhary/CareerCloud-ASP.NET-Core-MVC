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
    public class CompanyJobsController : Controller
    {
        private readonly CareerCloudContext _context;

        public CompanyJobsController(CareerCloudContext context)
        {
            _context = context;
        }

        // GET: CompanyJobs
        public async Task<IActionResult> Index(Guid? company, Guid? id)
        {
            if (company is null)
            {
                var careerCloudContext = await _context.CompanyJobs.Where(c => c.Id == id).ToListAsync();
                if (careerCloudContext.Count() == 0)
                {
                    ViewData["Company"] = id;
                    return View("~/Views/CompanyJobs/AddNewJob.cshtml");
                }
                return View(careerCloudContext);
            }
            if (id is null)
            {
                var careerCloudContext1 = await _context.CompanyJobs.Where(c => c.Company == company).ToListAsync();
                if (careerCloudContext1.Count() == 0)
                {
                    ViewData["Company"] = company;
                    return View("~/Views/CompanyJobs/AddNewJob.cshtml");
                }
                return View(careerCloudContext1);
            }
            return View(await _context.CompanyJobs.ToListAsync());
        }

        // GET: CompanyJobs/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyJobPoco = await _context.CompanyJobs
                .Include(c => c.CompanyProfilePoco)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (companyJobPoco == null)
            {
                return NotFound();
            }

            return View(companyJobPoco);
        }

        // GET: CompanyJobs/Create
        public IActionResult Create()
        {
            ViewData["Company"] = new SelectList(_context.CompanyProfiles, "Id", "Id");
            return View();
        }

        public async Task<IActionResult> ApplyJobs(Guid applicant)
        {
            List<CompanyJobPoco> companyJobs = await _context.CompanyJobs.ToListAsync();
            List<JobViewModel> jobViewModels = new List<JobViewModel>();
            foreach (CompanyJobPoco item in companyJobs)
            {
                CompanyJobDescriptionPoco poco = _context.CompanyJobDescriptions.Where(c => c.Job == item.Id).FirstOrDefault();
                JobViewModel jobViewModel = new JobViewModel();
                jobViewModel.companyJob = item;
                jobViewModel.companyJobDescription = poco;
                if (poco is null)
                {
                    continue;
                }
                jobViewModels.Add(jobViewModel);
            }

            ViewData["Applicant"] = applicant;
            ViewData["JobName"] = jobViewModels
                .Select(i => new SelectListItem()
                {
                    Text = i.companyJobDescription is null ? "" : (i.companyJobDescription.JobName + " -- " + i.companyJobDescription.JobDescriptions.Substring(0, i.companyJobDescription.JobDescriptions.Length > 36 ? 35 : i.companyJobDescription.JobDescriptions.Length)),
                    Value = i.companyJob.Id.ToString()
                });
            ViewData["aaa"] = ViewData["JobName"];
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplyNewJob([Bind("Id,Applicant,Job,ApplicationDate")] TempModel tempModel)
        {
            if (ModelState.IsValid)
            {
                ApplicantJobApplicationPoco applicantJobApplicationPoco = new ApplicantJobApplicationPoco();
                applicantJobApplicationPoco.Id = Guid.NewGuid();
                applicantJobApplicationPoco.Applicant = tempModel.Applicant;
                applicantJobApplicationPoco.Job = Guid.Parse(tempModel.Job);
                applicantJobApplicationPoco.ApplicationDate = tempModel.ApplicationDate;
                _context.Add(applicantJobApplicationPoco);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }


        // POST: CompanyJobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Company,ProfileCreated,IsInactive,IsCompanyHidden")] CompanyJobPoco companyJobPoco)
        {
            if (ModelState.IsValid)
            {
                companyJobPoco.Id = Guid.NewGuid();
                _context.Add(companyJobPoco);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Company"] = new SelectList(_context.CompanyProfiles, "Id", "Id", companyJobPoco.Company);
            return View(companyJobPoco);
        }

        // GET: CompanyJobs/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyJobPoco = await _context.CompanyJobs.FindAsync(id);
            if (companyJobPoco == null)
            {
                return NotFound();
            }
            ViewData["Company"] = new SelectList(_context.CompanyProfiles, "Id", "Id", companyJobPoco.Company);
            return View(companyJobPoco);
        }

        // POST: CompanyJobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Company,ProfileCreated,IsInactive,IsCompanyHidden")] CompanyJobPoco companyJobPoco)
        {
            if (id != companyJobPoco.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(companyJobPoco);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyJobPocoExists(companyJobPoco.Id))
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
            ViewData["Company"] = new SelectList(_context.CompanyProfiles, "Id", "Id", companyJobPoco.Company);
            return View(companyJobPoco);
        }

        // GET: CompanyJobs/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyJobPoco = await _context.CompanyJobs
                .Include(c => c.CompanyProfilePoco)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (companyJobPoco == null)
            {
                return NotFound();
            }

            return View(companyJobPoco);
        }

        // POST: CompanyJobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var companyJobPoco = await _context.CompanyJobs.FindAsync(id);
            _context.CompanyJobs.Remove(companyJobPoco);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyJobPocoExists(Guid id)
        {
            return _context.CompanyJobs.Any(e => e.Id == id);
        }
    }
}
