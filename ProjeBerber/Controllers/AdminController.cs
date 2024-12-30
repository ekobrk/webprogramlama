using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjeBerber.Data;
using ProjeBerber.Models;

namespace ProjeBerber.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ManageBarbers()
        {
            var barbers = _context.Barbers.ToList();
            return View(barbers);
        }

        [HttpGet]
        public IActionResult AddBarber()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddBarber(Barber model)
        {
            if (ModelState.IsValid)
            {
                _context.Barbers.Add(model);
                _context.SaveChanges();
                return RedirectToAction("ManageBarbers");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult DeleteBarber(int id)
        {
            var barber = _context.Barbers.Find(id);
            if (barber != null)
            {
                _context.Barbers.Remove(barber);
                _context.SaveChanges();
            }
            return RedirectToAction("ManageBarbers");
        }

        public IActionResult ManageServices()
        {
            var services = _context.Services.ToList();
            return View(services);
        }

        [HttpGet]
        public IActionResult AddService()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddService(Service model)
        {
            if (ModelState.IsValid)
            {
                _context.Services.Add(model);
                _context.SaveChanges();
                return RedirectToAction("ManageServices");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult DeleteService(int id)
        {
            var service = _context.Services.Find(id);
            if (service != null)
            {
                _context.Services.Remove(service);
                _context.SaveChanges();
            }
            return RedirectToAction("ManageServices");
        }

        public IActionResult ViewAppointments()
        {
            var appointments = _context.Appointments
                .Include(a => a.Barber)
                .Include(a => a.User)
                .Include(a => a.Service)
                .ToList();

            return View(appointments);
        }

        public IActionResult ApproveAppointment(int id)
        {
            var appointment = _context.Appointments.Find(id);
            if (appointment != null)
            {
                appointment.IsApproved = true;
                _context.SaveChanges();
            }
            return RedirectToAction("ViewAppointments");
        }

        public IActionResult DeleteAppointment(int id)
        {
            var appointment = _context.Appointments.Find(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                _context.SaveChanges();
            }
            return RedirectToAction("ViewAppointments");
        }
    }
}
