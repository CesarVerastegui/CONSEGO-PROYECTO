using System.ComponentModel.DataAnnotations;

namespace CONSEGO.Models.ViewModels
{
    public class UsuarioCreateViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100)]
        [Display(Name = "Nombre Completo")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "Email no válido.")]
        [StringLength(150)]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "El rol es obligatorio.")]
        [Display(Name = "Rol")]
        public int RolId { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;
    }
}
