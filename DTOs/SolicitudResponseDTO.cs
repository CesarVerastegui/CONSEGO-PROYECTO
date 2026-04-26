namespace CONSEGO.DTOs
{
    public class SolicitudResponseDTO
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public DateTime FechaSolicitud { get; set; }
        public string SolicitanteNombre { get; set; } = string.Empty;
        public string PlataformaNombre { get; set; } = string.Empty;
        public string TipoAcceso { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string? AnalistaNombre { get; set; }
        public string Justificacion { get; set; } = string.Empty;
    }
}
