using System.ComponentModel.DataAnnotations;
using CONSEGO.Models.Enums;

namespace CONSEGO.DTOs
{
    public class SolicitudCreateDTO
    {
        [Required(ErrorMessage = "La plataforma es obligatoria.")]
        public int PlataformaId { get; set; }

        [Required(ErrorMessage = "El tipo de acceso es obligatorio.")]
        public TipoAcceso TipoAcceso { get; set; }

        [Required(ErrorMessage = "La justificación es obligatoria.")]
        [StringLength(500)]
        public string Justificacion { get; set; } = string.Empty;
    }
}
