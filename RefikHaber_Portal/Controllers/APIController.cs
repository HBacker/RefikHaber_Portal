using Microsoft.AspNetCore.Mvc;
using haberPortali1.Models; // Haber ve IHaberRepository için
using System.Collections.Generic;
using RefikHaber.Repostories;

namespace RefikHaber.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class HaberController : ControllerBase
    {
        private readonly IHaberRepository _haberRepository;

        public HaberController(IHaberRepository haberRepository)
        {
            _haberRepository = haberRepository;
        }

        // GET: api/v1/haber
        [HttpGet]
        public ActionResult<IEnumerable<Haber>> GetAllNews()
        {
            var haberler = _haberRepository.GetAll(); // Tüm haberleri al
            return Ok(haberler); // JSON formatında döner
        }

        // GET: api/v1/haber/{id} - Bu metodu kaldırabilirsiniz
        // [HttpGet("{id}")]
        // public ActionResult<Haber> GetNewsById(int id)
        // {
        //     var haber = _haberRepository.Get(h => h.Id == id); // ID'ye göre haber al
        //     if (haber == null)
        //     {
        //         return NotFound(); // 404 Not Found
        //     }
        //     return Ok(haber); // JSON formatında döner
        // }
    }
}
