
using MonoTestAppVol2.Data;
using System.Linq;
using MonoTestAppVol2.Controllers;
using VehicleModels;
using Microsoft.EntityFrameworkCore;
using VehicleMake;
using Azure;

namespace MonoTestAppVol2.Methods
{
    public class FilterSort
    {
        private readonly ApplicationDbContext _context;

        public FilterSort(ApplicationDbContext context)
        {
            _context = context;
        }
    
    //Filter Manufacturers by Model
    public async Task<List<Vehicle>> FilterVehicles(string searchString,int page=1,int pageSize=10 )
        {
            IQueryable<Vehicle> vehicles = _context.VehicleModels.Include(v => v.VehicleMake);

            if (!string.IsNullOrEmpty(searchString))
            {
                vehicles = vehicles.Where(v => v.Model.Contains(searchString));
            }
            return await vehicles.ToListAsync();
        }
    //Sort Manufacturers by Model, Abrv
    public async Task<List<Vehicle>>SortVehicles(string sortOrder,int pageNumber= 1,int pageSize=10)
        {
            IQueryable<Vehicle> vehicles = _context.VehicleModels.Include(v => v.VehicleMake);
            vehicles = sortOrder switch
            {
                //Sorting VehicleModel class    
                "Model" => vehicles.OrderBy(x => x.Model),
                "Model_desc" => vehicles.OrderByDescending(x => x.Model),                
                "Abrv" => vehicles.OrderBy(x => x.Abrv),
                "Abrv_desc" => vehicles.OrderByDescending(x => x.Abrv),

                
                //default sorting    
                _ => vehicles.OrderBy(v => v.Id),
            };
            return await vehicles.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }
        //Sort Manufacturers by Id, Name, Abrv

    public async Task<List<Make>>SortManufacturers(string sortOrder,int pageNumber= 1,int pageSize=10)
        {
            IQueryable<Make> manufacturers = _context.VehicleMakes;

            manufacturers = sortOrder switch
            {
                    //Sorting VehicleMake class
                    "Id" => manufacturers.OrderBy(x => x.Id),
                    "Id_desc" => manufacturers.OrderByDescending(x => x.Id),
                    "Name" => manufacturers.OrderBy(x => x.Name),
                    "Name_desc" => manufacturers.OrderByDescending(x => x.Name),
                    "Abrv" => manufacturers.OrderBy(x => x.Abrv),
                    "Abrv_desc" => manufacturers.OrderByDescending(x => x.Abrv),
                    //default sorting    
                    _ => manufacturers.OrderBy(v => v.Id),
            };
            return await manufacturers.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            
        }
        //Filter Manufacturers by Name
        public async Task<List<Make>> FilterManufacturers(string searchString,int page=1,int pageSize=10)
        {
            IQueryable<Make> manufacturers = _context.VehicleMakes;

            if (!string.IsNullOrEmpty(searchString))
            {
               manufacturers =manufacturers.Where(v => v.Name.Contains(searchString));
            }
            return await manufacturers.ToListAsync();
        }
    }
}


  