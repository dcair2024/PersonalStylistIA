using Microsoft.AspNetCore.Identity;

namespace PersonalStylistIA.Models
{
    public class ApplicationUser : IdentityUser

    {
        public string? NomeCompleto{ get; set; }
        public DateTime? DataNascimento { get; set; }
        public string? Genero { get; set; }
        public string? Telefone { get; set; }
    }
}
