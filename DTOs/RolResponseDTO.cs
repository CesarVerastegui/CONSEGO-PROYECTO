namespace CONSEGO.DTOs
{
    public class RolResponseDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public int CantidadUsuarios { get; set; }
    }
}
