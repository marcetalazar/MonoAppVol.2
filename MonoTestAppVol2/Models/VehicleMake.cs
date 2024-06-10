
using System.ComponentModel.DataAnnotations.Schema;

namespace MonoTestAppVol2.Makes
{
    public class VehicleMake
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Abrv { get; set; }
    }

   

    
}