using System.ComponentModel.DataAnnotations;

namespace CONSEGO.Models
{
    public class Rol
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del rol es obligatorio.")]
        [StringLength(50)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(200)]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
