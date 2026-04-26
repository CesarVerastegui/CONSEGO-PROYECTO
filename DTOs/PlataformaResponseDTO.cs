using CONSEGO.Models.Enums;

namespace CONSEGO.DTOs
{
    public class PlataformaResponseDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public TipoPlataforma Tipo { get; set; }
        public Criticidad Criticidad { get; set; }
        public bool Activa { get; set; }
        public int CantidadSolicitudes { get; set; }
    }
}
