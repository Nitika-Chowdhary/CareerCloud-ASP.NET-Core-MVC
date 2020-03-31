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
    public class SecurityLoginsController : Controller
    {
        private readonly CareerCloudContext _context;

        public SecurityLoginsController(CareerCloudContext context)
        {
            _context = context;
        }

        // GET: SecurityLogins
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentOrder"] = sortOrder;
            
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["Filter"] = searchString;            
            List<SecurityLoginPoco> securityLoginPocos = await _context.SecurityLogins.ToListAsync();
            if (!String.IsNullOrEmpty(searchString))
            {
                securityLoginPocos = securityLoginPocos
                    .Where(s => s.FullName.Contains(searchString)).ToList();
            }
            if (ViewData["CurrentOrder"] is "name_desc")
            {
                securityLoginPocos = securityLoginPocos.OrderBy(o => o.FullName).ToList();
            }
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            int pageSize = 3;
            return View(PaginatedList<SecurityLoginPoco>.Create(securityLoginPocos, pageNumber ?? 1, pageSize));
        }

        // GET: SecurityLogins/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var securityLoginPoco = await _context.SecurityLogins
                .FirstOrDefaultAsync(m => m.Id == id);
            if (securityLoginPoco == null)
            {
                return NotFound();
            }

            return View(securityLoginPoco);
        }

        // GET: SecurityLogins/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SecurityLogins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Login,Password,Created,PasswordUpdate,AgreementAccepted,IsLocked,IsInactive,EmailAddress,PhoneNumber,FullName,ForceChangePassword,PrefferredLanguage")] SecurityLoginPoco securityLoginPoco)
        {
            if (ModelState.IsValid)
            {
                securityLoginPoco.Id = Guid.NewGuid();
                _context.Add(securityLoginPoco);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(securityLoginPoco);
        }

        // GET: SecurityLogins/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var securityLoginPoco = await _context.SecurityLogins.FindAsync(id);
            if (securityLoginPoco == null)
            {
                return NotFound();
            }
            return View(securityLoginPoco);
        }

        // POST: SecurityLogins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Login,Password,Created,PasswordUpdate,AgreementAccepted,IsLocked,IsInactive,EmailAddress,PhoneNumber,FullName,ForceChangePassword,PrefferredLanguage")] SecurityLoginPoco securityLoginPoco)
        {
            if (id != securityLoginPoco.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(securityLoginPoco);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SecurityLoginPocoExists(securityLoginPoco.Id))
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
            return View(securityLoginPoco);
        }

        // GET: SecurityLogins/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var securityLoginPoco = await _context.SecurityLogins
                .FirstOrDefaultAsync(m => m.Id == id);
            if (securityLoginPoco == null)
            {
                return NotFound();
            }

            return View(securityLoginPoco);
        }

        // POST: SecurityLogins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var securityLoginPoco = await _context.SecurityLogins.FindAsync(id);
            _context.SecurityLogins.Remove(securityLoginPoco);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SecurityLoginPocoExists(Guid id)
        {
            return _context.SecurityLogins.Any(e => e.Id == id);
        }
    }
}
