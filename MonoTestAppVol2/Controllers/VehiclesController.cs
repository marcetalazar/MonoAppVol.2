using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MonoTestAppVol2.Data;
using VehicleModels;
using MonoTestAppVol2.Methods;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using Microsoft.Identity.Client;
using System.Windows.Markup;


namespace MonoTestAppVol2.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        public int pageSize = 10;
        public int page = 1;  

        public VehiclesController(ApplicationDbContext context)
        {
            _context = context; 
        }

        // GET: Vehicles
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10,string sortOrder = "Id")

        {   
            FilterSort filterSort = new FilterSort(_context);
            var vehicleModels = await filterSort.SortVehicles(sortOrder, page, pageSize);
            
            ViewData["NumOfPages"] = (await _context.VehicleModels.CountAsync() + pageSize - 1) / pageSize;
            var applicationDbContext = _context.VehicleModels.Include(v => v.VehicleMake);
            int totalItems = await _context.VehicleModels.CountAsync();
            int totalPages = (totalItems + pageSize - 1) / pageSize;
            ViewBag.SortOrder = sortOrder;
           
            return View(vehicleModels);  
    
        }


        

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.VehicleModels
                .Include(v => v.VehicleMake)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            ViewData["VehicleMakeId"] = new SelectList(_context.VehicleMakes, "Id", "Abrv");
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VehicleMakeId,Model,Abrv")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VehicleMakeId"] = new SelectList(_context.VehicleMakes, "Id", "Id", vehicle.VehicleMakeId);
            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.VehicleModels.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            ViewData["VehicleMakeId"] = new SelectList(_context.VehicleMakes, "Id", "Name", vehicle.VehicleMakeId);
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VehicleMakeId,Model,Abrv")] Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.Id))
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
            ViewData["VehicleMakeId"] = new SelectList(_context.VehicleMakes, "Id", "Id", vehicle.VehicleMakeId);
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.VehicleModels
                .Include(v => v.VehicleMake)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await _context.VehicleModels.FindAsync(id);
            if (vehicle != null)
            {
                _context.VehicleModels.Remove(vehicle);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleExists(int id)
        {
            return _context.VehicleModels.Any(e => e.Id == id);
        }
        [HttpGet]
        //Filter Vehicles from the list by Model
        public async Task<IActionResult> FilterVehicles(string searchString,int page=1,int pageSize=10 )
        {
            FilterSort FilterSort = new FilterSort(_context);

            var filterView=await FilterSort.FilterVehicles(searchString,page,pageSize);
            int totalItems = filterView.Count();
            ViewData["NumOfPages"] = Pagination.Pagination.GetNumOfPages(filterView, pageSize);
            int totalPages = (totalItems + pageSize - 1) / pageSize;
            var paginatedVehicles = Pagination.Pagination.Paginate(filterView, page, pageSize);
            return View("Index", paginatedVehicles); 
        }
        [HttpGet]
        //Sort Vehicles from the list by Id, Model or Abrv	
        public async Task<IActionResult> SortVehicles(string sortOrder)
        {
            var SortVehicles = await _context.VehicleModels.ToListAsync();

            FilterSort FilterSort = new FilterSort(_context);

            var sortView = await FilterSort.SortVehicles(sortOrder);

            ViewData["NumOfPages"] = Pagination.Pagination.GetNumOfPages(sortView, pageSize);
            var paginatedVehicles = Pagination.Pagination.Paginate(sortView, page, pageSize);   

            return View("Index", paginatedVehicles  );
        }
    }
}
