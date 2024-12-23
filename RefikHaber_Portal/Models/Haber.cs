using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace haberPortali1.Models
{
    public class Haber
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık alanı zorunludur.")]
        [MaxLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir.")]
        public string Baslik { get; set; }

        [MaxLength(5000, ErrorMessage = "Açıklama en fazla 5000 karakter olabilir.")]
        public string? Aciklama { get; set; } 

        [Required(ErrorMessage = "Yazar adı zorunludur.")]
        [MaxLength(100, ErrorMessage = "Yazar adı en fazla 100 karakter olabilir.")]
        public string Yazar { get; set; }

        [Required(ErrorMessage = "Puanlama zorunludur.")]
        [Range(1, 10, ErrorMessage = "Puanlama 1 ile 10 arasında olmalıdır.")]
        public double Puan { get; set; } 

        [Required(ErrorMessage = "Kategori seçilmesi zorunludur.")]
        [ValidateNever]
        public int KategoriId { get; set; }

        [ForeignKey("KategoriId")]
        [ValidateNever]
        public HaberTuru? HaberTuru { get; set; }

        [ValidateNever]
        public string? ResimUrl { get; set; } 
    }
}
