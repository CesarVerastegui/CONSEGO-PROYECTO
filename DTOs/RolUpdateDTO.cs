using System.ComponentModel.DataAnnotations;

namespace CONSEGO.DTOs
{
    public class RolUpdateDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del rol es obligatorio.")]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Descripcion { get; set; }
    }
}
