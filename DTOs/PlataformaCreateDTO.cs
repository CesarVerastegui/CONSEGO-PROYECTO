using System.ComponentModel.DataAnnotations;
using CONSEGO.Models.Enums;

namespace CONSEGO.DTOs
{
    public class PlataformaCreateDTO
    {
        [Required(ErrorMessage = "El nombre de la plataforma es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo es obligatorio.")]
        public TipoPlataforma Tipo { get; set; }

        [Required(ErrorMessage = "La criticidad es obligatoria.")]
        public Criticidad Criticidad { get; set; }

        public bool Activa { get; set; } = true;
    }
}
