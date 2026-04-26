using System.ComponentModel.DataAnnotations;

namespace CONSEGO.DTOs
{
    public class UsuarioUpdateDTO
    {
        public int Id { get; set; }
        [Required] public string Nombre { get; set; } = string.Empty;
        [Required][EmailAddress] public string Email { get; set; } = string.Empty;
        [Required] public int RolId { get; set; }
        public string? Password { get; set; }
    }
}
