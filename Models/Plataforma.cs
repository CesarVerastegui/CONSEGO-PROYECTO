using System.ComponentModel.DataAnnotations;
using CONSEGO.Models.Enums;

namespace CONSEGO.Models
{
    public class Plataforma
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la plataforma es obligatorio.")]
        [StringLength(100)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo es obligatorio.")]
        [Display(Name = "Tipo")]
        public TipoPlataforma Tipo { get; set; }

        [Required(ErrorMessage = "La criticidad es obligatoria.")]
        [Display(Name = "Criticidad")]
        public Criticidad Criticidad { get; set; }

        [Display(Name = "Activa")]
        public bool Activa { get; set; } = true;

        public ICollection<SolicitudAcceso> Solicitudes { get; set; } = new List<SolicitudAcceso>();
    }
}
