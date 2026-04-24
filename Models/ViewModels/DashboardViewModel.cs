namespace CONSEGO.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalSolicitudes { get; set; }
        public int SolicitudesPendientes { get; set; }
        public int SolicitudesAprobadas { get; set; }
        public int SolicitudesImplementadas { get; set; }
        public int SolicitudesRechazadas { get; set; }
        public int TotalPlataformasActivas { get; set; }
        public int TotalUsuariosActivos { get; set; }
    }
}