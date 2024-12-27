using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace ProjeBerber.Models.ViewModels
{
    public class BookViewModel
    {
        // Formdan gelecek veriler:
        public DateTime SelectedDate { get; set; }
        public int SelectedBarberId { get; set; }
        public int SelectedServiceId { get; set; }

        // Dropdown listeleri:
        public List<SelectListItem> BarberList { get; set; }
        public List<SelectListItem> ServiceList { get; set; }

        // Bulduğumuz boş slotları burada tutalım
        public List<DateTime> AvailableSlots { get; set; } = new List<DateTime>();

        // İsterseniz başka alanlar da ekleyebilirsiniz
    }
}
