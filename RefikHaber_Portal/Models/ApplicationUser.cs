using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace haberPortali1.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email {  get; set; }
    }
}
