using Microsoft.AspNetCore.Mvc;
using RefikHaber.Models;
using RefikHaber_Portal.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefikHaber_Portal.Controllers
{
    public class RoleManagerController : Controller
    {
        private readonly RoleManagerRepository _roleManagerRepository;

        public RoleManagerController(RoleManagerRepository roleManagerRepository)
        {
            _roleManagerRepository = roleManagerRepository;
        }

        // GET: /RoleManager/GetRoleList
        public async Task<IActionResult> GetRoleList()
        {
            var roles = await _roleManagerRepository.GetAllRolesAsync();
            return View(roles.ToList());
        }

        // GET: /RoleManager/RoleAdd
        public IActionResult RoleAdd()
        {
            return View();
        }

        // POST: /RoleManager/RoleAdd
        [HttpPost]
        public async Task<IActionResult> RoleAdd(AppRole role)
        {
            if (ModelState.IsValid)
            {
                await _roleManagerRepository.AddRoleAsync(role);
                return RedirectToAction("GetRoleList");
            }
            return View(role);
        }

        // GET: /RoleManager/GetUserList
        public IActionResult GetUserList()
        {
            return View();
        }

        // GET: /RoleManager/UserListAjax
        [HttpGet]
        public async Task<IActionResult> UserListAjax()
        {
            var users = await _roleManagerRepository.GetUsersAsync();
            return Json(users);
        }

        // GET: /RoleManager/ManageUserRoles
        public async Task<IActionResult> ManageUserRoles()
        {
            var users = await _roleManagerRepository.GetAllUsersAsync();
            ViewBag.Roles = await _roleManagerRepository.GetAllRolesAsync();

            // Kullanıcıların mevcut rollerini ViewBag'e ekle
            ViewBag.UserRoles = await _roleManagerRepository.GetUserRolesGroupedAsync();

            return View(users);
        }

        // POST: /RoleManager/ManageUserRoles
        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(string userId, string[] selectedRoles)
        {
            if (selectedRoles != null && selectedRoles.Length > 0)
            {
                // Kullanıcı rollerini güncelle
                await _roleManagerRepository.UpdateUserRolesAsync(userId, selectedRoles);
                return RedirectToAction("ManageUserRoles");
            }

            // Hata mesajı ekle
            ModelState.AddModelError("", "Rol seçimi yapılmadı.");
            return RedirectToAction("ManageUserRoles");
        }

        [HttpGet]
    public async Task<IActionResult> UserList()
    {
        // Repository'den tüm kullanıcıları alıyoruz
        var users = await _roleManagerRepository.GetAllUsersAsync();

        // Veriyi view'a gönderiyoruz
        return View(users); // UserList.cshtml'ye gönderiyoruz
    }
    }
}
