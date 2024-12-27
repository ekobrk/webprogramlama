using System.ComponentModel.DataAnnotations;

namespace ProjeBerber.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Hizmet adı zorunludur.")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Hizmet süresi zorunludur.")]
        public int DurationMinutes { get; set; } // Örnek: 30 dk, 60 dk vs.

        [Required(ErrorMessage = "Hizmet ücreti zorunludur.")]
        public decimal Price { get; set; }
    }
}
