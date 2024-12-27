using Microsoft.AspNetCore.Mvc;
using ProjeBerber.Data;
using ProjeBerber.Models;

namespace ProjeBerber.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // Üye kayıt formunu göster
        public IActionResult Register()
        {
            return View();
        }

        // Üye kayıt işlemi
        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                // Aynı email var mı?
                if (_context.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Bu e-posta adresi zaten kayıtlı.");
                    return View(user);
                }

                // Belirli mail+şifre admin olsun
                if (user.Email == "g201210067@sakarya.edu.tr" && user.Sifre == "sau")
                {
                    user.Role = "Admin";
                    ModelState.Remove("Sifre"); // Şifre validasyonunu geçici olarak kaldır
                }
                else
                {
                    user.Role = "User";
                }

                // Kaydı veritabanına ekle
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }

            // Model geçerli değilse form tekrar gösterilir
            return View(user);
        }

        // Giriş formu
        public IActionResult Login()
        {
            return View();
        }

        // Giriş işlemi
        [HttpPost]
        public IActionResult Login(string email, string sifre)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Sifre == sifre);
            if (user != null)
            {
                // Oturum bilgilerini (Session) ayarla
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserName", user.AdSoyad);
                HttpContext.Session.SetString("UserRole", user.Role);

                // Eğer admin ise Admin/Index'e gidebilir (isteğe bağlı)
                // return RedirectToAction("Index", "Admin");

                // Ya da normalde anasayfaya
                return RedirectToAction("Index", "Home");
            }

            // Kullanıcı yoksa hata
            ViewBag.Hata = "Geçersiz e-posta veya şifre.";
            return View();
        }

        // Çıkış
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Oturum sıfırlama
            return RedirectToAction("Login");
        }
    }
}
