
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleMake
{
    public class Make
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Abrv { get; set; }
    }

   

    
}