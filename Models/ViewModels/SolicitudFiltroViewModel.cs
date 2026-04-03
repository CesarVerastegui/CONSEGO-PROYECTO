using CONSEGO.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CONSEGO.Models.ViewModels
{
    public class SolicitudFiltroViewModel
    {
        [Display(Name = "Estado")]
        public EstadoSolicitud? Estado { get; set; }

        [Display(Name = "Plataforma")]
        public int? PlataformaId { get; set; }

        [Display(Name = "Desde")]
        [DataType(DataType.Date)]
        public DateTime? FechaDesde { get; set; }

        [Display(Name = "Hasta")]
        [DataType(DataType.Date)]
        public DateTime? FechaHasta { get; set; }

        public int Pagina { get; set; } = 1;
        public int TamañoPagina { get; set; } = 10;
        public int TotalRegistros { get; set; }
        public int TotalPaginas => (int)Math.Ceiling((double)TotalRegistros / TamañoPagina);

        public List<SolicitudAcceso> Solicitudes { get; set; } = new();
        public List<Plataforma> PlataformasDisponibles { get; set; } = new();
    }
}
