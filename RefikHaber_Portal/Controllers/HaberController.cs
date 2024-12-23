using haberPortali1.Models;
using haberPortali1.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RefikHaber.Repostories;

namespace haberPortali1.Controllers
{
    public class HaberController : Controller
    {
        private readonly IHaberRepository _haberRepository;
        private readonly IHaberTuruRepository _haberTuruRepository;
        public readonly IWebHostEnvironment _webHostEnvironment;
        public HaberController(IHaberRepository haberRepository, IHaberTuruRepository haberTuruRepository, IWebHostEnvironment webHostEnvironment)
        {
            _haberRepository = haberRepository;
            _haberTuruRepository = haberTuruRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        [Authorize(Roles = "Admin,Kullanici")]
        public IActionResult Index()
        {
            // Tüm haberleri al
            List<Haber> objHaberList = _haberRepository.GetAll(includeProps: "HaberTuru").ToList();
            return View(objHaberList);
        }
        [Authorize(Roles = "Admin,Kullanici")]
        public IActionResult Privacy()
        {
            // Tüm haberleri al
            List<Haber> objHaberList = _haberRepository.GetAll(includeProps: "HaberTuru").ToList();
            return View(objHaberList);
        }
        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult EkleGuncelle(int? id) 
        {
            IEnumerable<SelectListItem> HaberTuruList = _haberTuruRepository.GetAll()
                .Select(k => new SelectListItem
                {
                    Text = k.Ad,
                    Value = k.Id.ToString(),
                });

            ViewBag.HaberTuruList = HaberTuruList;

            if(id == null|| id == 0)
            {
                return View();
            }
            else
            {
                //guncelleme
            Haber? haberVt = _haberRepository.Get(u=>u.Id==id);
            if (haberVt == null)
            {
                return NotFound();
            }
            return View(haberVt);
            }
        }
        [Authorize(Roles = UserRoles.Role_Admin)]
        [HttpPost]
        public IActionResult EkleGuncelle(Haber haber, IFormFile? file)
        {

            if(ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string haberPath = Path.Combine(wwwRootPath, wwwRootPath,@"img");

                if(file !=null)
                { 
                using(var fileStream = new FileStream(Path.Combine(haberPath, file.FileName),FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                haber.ResimUrl = @"\img\" + file.FileName;
                }

                if (haber.Id == 0)
                {
                    _haberRepository.Ekle(haber);
                    TempData["basarili"] = "Yeni Haber Başarıyla Oluşturuldu!";
                }
                else
                {
                    _haberRepository.Guncelle(haber);
                    TempData["basarili"] = "Haber Güncelleme Başarılı!";
                }

                _haberRepository.Kaydet();
                return RedirectToAction("Index","Haber");
            }
            return View();
        }
        /*
        public IActionResult Guncelle(int? id)
        {
            if (id == null || id == 0) 
            {
                return NotFound();
            }
            Haber? haberVt = _haberRepository.Get(u=>u.Id==id);
            if (haberVt == null)
            {
                return NotFound();
            }
            return View(haberVt);
        } */

        /* [HttpPost]
         public IActionResult Guncelle(Haber haber)
         {
             if (ModelState.IsValid)
             {
                 _haberRepository.Guncelle(haber);
                 _haberRepository.Kaydet();
                 TempData["basarili"] = "Haber Başarıyla Güncellendi";
                 return RedirectToAction("Index", "Haber");
             }
             return View();
         }*/

        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult Sil(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Haber? haberVt = _haberRepository.Get(u => u.Id == id);
            if (haberVt == null)
            {
                return NotFound();
            }
            return View(haberVt);
        }

        [Authorize(Roles = UserRoles.Role_Admin)]
        [HttpPost, ActionName("Sil")]
        public IActionResult SilPOST(int? id)
        {
            Haber? haber = _haberRepository.Get(u => u.Id == id);
            if (haber == null)
            {
                return NotFound();
            }
            _haberRepository.Sil(haber);
            _haberRepository.Kaydet();
            TempData["basarili"] = "Haber Başarıyla Silindi";
            return RedirectToAction("Index", "Haber");
        }
        [Authorize(Roles = "Admin,Kullanici")]
        public IActionResult Goruntule(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Haber? haber = _haberRepository.Get(h => h.Id == id, includeProps: "HaberTuru");

            if (haber == null)
            {
                return NotFound();
            }

            return View(haber); 
        }

    }
}
