using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjeBerber.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        // Hangi berber
        [Required]
        public int BarberId { get; set; }
        [ForeignKey("BarberId")]
        public Barber Barber { get; set; }

        // Hangi kullanıcı
        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        // Hangi hizmet
        [Required]
        public int ServiceId { get; set; }
        [ForeignKey("ServiceId")]
        public Service Service { get; set; }

        // Admin onayı
        public bool IsApproved { get; set; } = false;
    }
}
