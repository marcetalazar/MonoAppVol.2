using System.ComponentModel.DataAnnotations.Schema;
using VehicleMake;

namespace VehicleModels
{
    public class Vehicle
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [ForeignKey("VehicleMakeId")]
        public  VehicleMake.Make? VehicleMake { get; set; } 
        public int VehicleMakeId { get; set; } 
        public required string Model { get; set; }
        public string? Abrv { get; set; }

    }
}