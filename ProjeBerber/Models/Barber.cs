using System;
using System.ComponentModel.DataAnnotations;

namespace ProjeBerber.Models
{
    public class Barber
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "İsim zorunludur.")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyisim zorunludur.")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Çalışma başlangıç saati zorunludur.")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "Çalışma bitiş saati zorunludur.")]
        public TimeSpan EndTime { get; set; }

        // Yeni eklenen alan
        [StringLength(200, ErrorMessage = "Uzmanlık alanları en fazla 200 karakter olabilir.")]
        public string Specialties { get; set; }
    }
}
