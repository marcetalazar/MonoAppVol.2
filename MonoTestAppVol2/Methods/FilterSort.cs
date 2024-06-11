
using MonoTestAppVol2.Data;
using System.Linq;
using MonoTestAppVol2.Controllers;
using VehicleModels;
using Microsoft.EntityFrameworkCore;

namespace MonoTestAppVol2.Methods
{
    public class FilterSort
    {
        private readonly ApplicationDbContext _context;

        public FilterSort(ApplicationDbContext context)
        {
            _context = context;
        }
    

    public async Task<List<Vehicle>> FilterVehicles(string searchString)
        {
            IQueryable<Vehicle> vehicles = _context.VehicleModels.Include(v => v.VehicleMake);

            if (!string.IsNullOrEmpty(searchString))
            {
                vehicles = vehicles.Where(v => v.Model.Contains(searchString));
            }
            return await vehicles.ToListAsync();
        }
    public async Task<List<Vehicle>>SortVehicles(string sortOrder)
        {
            IQueryable<Vehicle> vehicles = _context.VehicleModels.Include(v => v.VehicleMake);

            vehicles = sortOrder switch
            {
                "Model" => vehicles.OrderBy(x => x.Model),
                "Model_desc" => vehicles.OrderByDescending(x => x.Model),
                "VehicleMake" => vehicles.OrderBy(x => x.VehicleMake),
                "VehicleMake_desc" => vehicles.OrderByDescending(x => x.VehicleMake),
                "abrv" => vehicles.OrderBy(x => x.Abrv),
                "abrv_desc" => vehicles.OrderByDescending(x => x.Abrv),

                _ => vehicles.OrderBy(v => v.Id),
            };
            return await vehicles.ToListAsync();
        }
        
    }
    
}


