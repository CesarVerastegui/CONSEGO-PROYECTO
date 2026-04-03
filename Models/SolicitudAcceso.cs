using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CONSEGO.Models.Enums;

namespace CONSEGO.Models
{
    public class SolicitudAcceso
    {
        public int Id { get; set; }

        [StringLength(20)]
        [Display(Name = "Código")]
        public string Codigo { get; set; } = string.Empty;

        [Display(Name = "Fecha de Solicitud")]
        [DataType(DataType.Date)]
        public DateTime FechaSolicitud { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "El solicitante es obligatorio.")]
        [Display(Name = "Solicitante")]
        public int UsuarioSolicitanteId { get; set; }

        [ForeignKey("UsuarioSolicitanteId")]
        public Usuario? UsuarioSolicitante { get; set; }

        [Required(ErrorMessage = "La plataforma es obligatoria.")]
        [Display(Name = "Plataforma")]
        public int PlataformaId { get; set; }

        [ForeignKey("PlataformaId")]
        public Plataforma? Plataforma { get; set; }

        [Required(ErrorMessage = "El tipo de acceso es obligatorio.")]
        [Display(Name = "Tipo de Acceso")]
        public TipoAcceso TipoAcceso { get; set; }

        [Required(ErrorMessage = "La justificación es obligatoria.")]
        [StringLength(500)]
        [Display(Name = "Justificación")]
        public string Justificacion { get; set; } = string.Empty;

        [Display(Name = "Estado")]
        public EstadoSolicitud Estado { get; set; } = EstadoSolicitud.Registrado;

        [Display(Name = "Analista Asignado")]
        public int? AnalistaId { get; set; }

        [ForeignKey("AnalistaId")]
        public Usuario? Analista { get; set; }

        [StringLength(500)]
        [Display(Name = "Observaciones de Seguridad")]
        public string? ObservacionesSeguridad { get; set; }

        [Display(Name = "Fecha de Decisión")]
        [DataType(DataType.Date)]
        public DateTime? FechaDecision { get; set; }

        [StringLength(500)]
        [Display(Name = "Motivo de Rechazo")]
        public string? MotivoRechazo { get; set; }
    }
}
