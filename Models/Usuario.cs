using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CONSEGO.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100)]
        [Display(Name = "Nombre Completo")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "Email no válido.")]
        [StringLength(150)]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(256)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required(ErrorMessage = "El rol es obligatorio.")]
        [Display(Name = "Rol")]
        public int RolId { get; set; }

        [ForeignKey("RolId")]
        public Rol? Rol { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;

        [Display(Name = "Fecha de Creación")]
        [DataType(DataType.Date)]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public ICollection<SolicitudAcceso> SolicitudesComoSolicitante { get; set; } = new List<SolicitudAcceso>();
        public ICollection<SolicitudAcceso> SolicitudesComoAnalista { get; set; } = new List<SolicitudAcceso>();
    }
}
