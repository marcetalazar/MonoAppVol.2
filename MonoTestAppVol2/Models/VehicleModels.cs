using System.ComponentModel.DataAnnotations.Schema;
using MonoTestAppVol2.Makes;

namespace VehicleModels
{
    public class Vehicle
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required MonoTestAppVol2.Makes.VehicleMake VehicleMake { get; set; } 
        public required string Model { get; set; }
        public string? Abrv { get; set; }

        public int VehicleMakeId { get; set; } 
    }
}