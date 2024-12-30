using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjeBerber.Data;
using ProjeBerber.Models;
using ProjeBerber.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ProjeBerber.Controllers
{
    [Authorize(Roles = "User")]
    public class AppointmentController : Controller
    {
        private readonly AppDbContext _context;

        public AppointmentController(AppDbContext context)
        {
            _context = context;
        }

        // Randevu Al Sayfası (GET)
        [HttpGet]
        public IActionResult Book()
        {
            var model = new BookViewModel
            {
                BarberList = _context.Barbers
                    .Select(b => new SelectListItem
                    {
                        Value = b.Id.ToString(),
                        Text = b.FirstName + " " + b.LastName
                    })
                    .ToList(),
                ServiceList = _context.Services
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = $"{s.Name} ({s.DurationMinutes} dk - {s.Price} TL)"
                    })
                    .ToList(),
                SelectedDate = DateTime.Today
            };

            return View(model);
        }

        // Randevu Al İşlemi (POST)
        [HttpPost]
        public IActionResult Book(BookViewModel model)
        {
            if (model.SelectedBarberId == 0 || model.SelectedServiceId == 0)
            {
                TempData["Error"] = "Lütfen bir berber ve hizmet seçiniz.";
                return View(model);
            }

            var barber = _context.Barbers.Find(model.SelectedBarberId);
            var service = _context.Services.Find(model.SelectedServiceId);

            if (barber == null || service == null)
            {
                TempData["Error"] = "Geçersiz berber veya hizmet seçimi.";
                return View(model);
            }

            // Mevcut randevuları al
            var sameDayAppointments = _context.Appointments
                .Where(a => a.BarberId == barber.Id && a.AppointmentDate.Date == model.SelectedDate.Date)
                .ToList();

            // Slot hesaplama
            var availableSlots = CalculateAvailableSlots(barber, service, sameDayAppointments, model.SelectedDate);

            model.AvailableSlots = availableSlots;
            model.BarberList = _context.Barbers
                .Select(b => new SelectListItem
                {
                    Value = b.Id.ToString(),
                    Text = b.FirstName + " " + b.LastName
                })
                .ToList();
            model.ServiceList = _context.Services
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = $"{s.Name} ({s.DurationMinutes} dk - {s.Price} TL)"
                })
                .ToList();

            return View(model);
        }

        // Boş slotları hesapla
        private List<DateTime> CalculateAvailableSlots(Barber barber, Service service, List<Appointment> sameDayAppointments, DateTime selectedDate)
        {
            var availableSlots = new List<DateTime>();
            var slotDuration = TimeSpan.FromMinutes(30);
            var startTime = barber.StartTime;
            var endTime = barber.EndTime;

            for (var time = startTime; time < endTime; time += slotDuration)
            {
                var slotStart = selectedDate.Date + time;
                var slotEnd = slotStart.AddMinutes(service.DurationMinutes);

                // Çakışan randevu kontrolü
                var isConflict = sameDayAppointments.Any(ap =>
                {
                    var apStart = ap.AppointmentDate;
                    var apEnd = apStart.AddMinutes(_context.Services.First(s => s.Id == ap.ServiceId).DurationMinutes);
                    return slotStart < apEnd && slotEnd > apStart;
                });

                if (!isConflict)
                {
                    availableSlots.Add(slotStart);
                }
            }

            return availableSlots;
        }

        // Randevuyu Onayla
        [HttpPost]
        public IActionResult ConfirmAppointment(DateTime chosenSlot, int barberId, int serviceId)
        {
            var userEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);

            if (user == null) return RedirectToAction("Login", "User");

            var appointment = new Appointment
            {
                UserId = user.Id,
                BarberId = barberId,
                ServiceId = serviceId,
                AppointmentDate = chosenSlot
            };

            _context.Appointments.Add(appointment);
            _context.SaveChanges();

            return RedirectToAction("MyAppointments");
        }

        // Kullanıcının Randevuları
        public IActionResult MyAppointments()
        {
            var userEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);

            if (user == null) return RedirectToAction("Login", "User");

            var appointments = _context.Appointments
                .Include(a => a.Barber)
                .Include(a => a.Service)
                .Where(a => a.UserId == user.Id)
                .ToList();

            return View(appointments);
        }
    }
}
