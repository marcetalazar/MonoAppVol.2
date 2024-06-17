using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MonoTestAppVol2.Data;
using VehicleMake;
using MonoTestAppVol2.Methods;
using Azure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace MonoTestAppVol2.Controllers
{
    public class VehicleMakesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private int pageSize=10;
        public int page=1;  
         

        public VehicleMakesController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        // GET: VehicleMakes
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string sortOrder = "Id")
        {
            FilterSort filterSort = new FilterSort(_context); 
            var vehicleMakes = await filterSort.SortManufacturers(sortOrder, page, pageSize);

            ViewData["NumOfPages"] = (await _context.VehicleMakes.CountAsync() + pageSize - 1) / pageSize;
            int totalItems = await _context.VehicleMakes.CountAsync();
            int totalPages = (totalItems + pageSize - 1) / pageSize;
            ViewBag.SortOrder = sortOrder;
            return View(vehicleMakes);
        }

        // GET: VehicleMakes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var make = await _context.VehicleMakes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (make == null)
            {
                return NotFound();
            }

            return View(make);
        }

        // GET: VehicleMakes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VehicleMakes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Abrv")] Make make)
        {
            if (ModelState.IsValid)
            {
                _context.Add(make);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(make);
        }

        // GET: VehicleMakes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var make = await _context.VehicleMakes.FindAsync(id);
            if (make == null)
            {
                return NotFound();
            }
            return View(make);
        }

        // POST: VehicleMakes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Abrv")] Make make)
        {
            if (id != make.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(make);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MakeExists(make.Id))
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
            return View(make);
        }

        // GET: VehicleMakes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var make = await _context.VehicleMakes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (make == null)
            {
                return NotFound();
            }

            return View(make);
        }

        // POST: VehicleMakes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var make = await _context.VehicleMakes.FindAsync(id);
            if (make != null)
            {
                _context.VehicleMakes.Remove(make);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MakeExists(int id)
        {
            return _context.VehicleMakes.Any(e => e.Id == id);
        }
       // Sorting Manufacturers
        public async Task<IActionResult> SortManufacturers(string sortOrder)
        {
            var vehicleMakes = await _context.VehicleMakes.ToListAsync();

            FilterSort FilterSort = new FilterSort(_context);

            var sortView = await FilterSort.SortManufacturers(sortOrder);


            ViewData["NumOfPages"] = Pagination.Pagination.GetNumOfPages(sortView, pageSize);

            var paginatedVehicleMakes =  Pagination.Pagination.Paginate(sortView, page, pageSize);

        

            return View("Index", paginatedVehicleMakes);
        }
        public async Task<IActionResult> FilterManufacturers(string searchString,int page=1,int pageSize=10)
        {
            FilterSort FilterSort = new FilterSort(_context);

            var filterView = await FilterSort.FilterManufacturers(searchString,page,pageSize);
            int totalItems = filterView.Count();
            ViewData["NumOfPages"] = Pagination.Pagination.GetNumOfPages(filterView, pageSize);
            int totalPages = (totalItems + pageSize - 1) / pageSize;
            var paginatedManufacturers = Pagination.Pagination.Paginate(filterView, page, pageSize);


            return View("Index", paginatedManufacturers);
        }
        
    }

}
