using System.Data;
using ClosedXML.Excel;
using CONSEGO.DTOs;
using CONSEGO.Models;
using CONSEGO.Models.Enums;
using CONSEGO.Models.ViewModels;
using CONSEGO.Repository;
using Microsoft.EntityFrameworkCore;

namespace CONSEGO.Service
{
    public class SolicitudService : ISolicitudService
    {
        private readonly ISolicitudRepository _repo;

        public SolicitudService(ISolicitudRepository repo)
        {
            _repo = repo;
        }

        public async Task<string> CrearSolicitudAsync(SolicitudCreateDTO dto, int userId)
        {
            var anio = DateTime.Now.Year;
            var ultimoCodigo = await _repo.GetUltimoCodigoAsync(anio);

            int siguiente = 1;
            if (ultimoCodigo != null)
            {
                var partes = ultimoCodigo.Split('-');
                if (partes.Length == 3 && int.TryParse(partes[2], out int num))
                    siguiente = num + 1;
            }

            var solicitud = new SolicitudAcceso
            {
                Codigo = $"ACC-{anio}-{siguiente:D4}",
                UsuarioSolicitanteId = userId,
                PlataformaId = dto.PlataformaId,
                TipoAcceso = dto.TipoAcceso,
                Justificacion = dto.Justificacion,
                Estado = EstadoSolicitud.Registrado,
                FechaSolicitud = DateTime.Now
            };

            await _repo.AddAsync(solicitud);
            await _repo.SaveChangesAsync();
            return solicitud.Codigo;
        }

        public async Task<bool> TomarSolicitudAsync(int id, int analistaId)
        {
            var solicitud = await _repo.GetByIdAsync(id);

            // Regla de negocio: Solo se toma si está en 'Registrado'
            if (solicitud == null || solicitud.Estado != EstadoSolicitud.Registrado)
                return false;

            solicitud.Estado = EstadoSolicitud.EnAnalisis;
            solicitud.AnalistaId = analistaId;

            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ResolverSolicitudAsync(int id, string decision, string? obs, string? motivo)
        {
            var solicitud = await _repo.GetByIdAsync(id);

            if (solicitud == null || solicitud.Estado != EstadoSolicitud.EnAnalisis)
                return false;

            solicitud.ObservacionesSeguridad = obs;
            solicitud.FechaDecision = DateTime.Now;

            if (decision == "Aprobar")
            {
                solicitud.Estado = EstadoSolicitud.Aprobado;
            }
            else
            {
                solicitud.Estado = EstadoSolicitud.Rechazado;
                solicitud.MotivoRechazo = motivo;
            }

            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ImplementarSolicitudAsync(int id)
        {
            var solicitud = await _repo.GetByIdAsync(id);

            // Regla: Solo se implementa si fue aprobada previamente
            if (solicitud == null || solicitud.Estado != EstadoSolicitud.Aprobado)
                return false;

            solicitud.Estado = EstadoSolicitud.Implementado;

            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<SolicitudFiltroViewModel> ListarFiltradoAsync(SolicitudFiltroViewModel filtro, int userId, string rol)
        {
            var query = _repo.GetQueryable();

            // Lógica de visibilidad por Rol
            if (rol == "Solicitante")
                query = query.Where(s => s.UsuarioSolicitanteId == userId);

            if (rol == "Infra")
                query = query.Where(s => s.Estado == EstadoSolicitud.Aprobado || s.Estado == EstadoSolicitud.Implementado);

            // Aplicación de filtros manuales
            if (filtro.Estado.HasValue)
                query = query.Where(s => s.Estado == filtro.Estado.Value);

            if (filtro.PlataformaId.HasValue)
                query = query.Where(s => s.PlataformaId == filtro.PlataformaId.Value);

            if (filtro.FechaDesde.HasValue)
                query = query.Where(s => s.FechaSolicitud >= filtro.FechaDesde.Value);

            if (filtro.FechaHasta.HasValue)
                query = query.Where(s => s.FechaSolicitud <= filtro.FechaHasta.Value);

            filtro.TotalRegistros = await query.CountAsync();

            filtro.Solicitudes = await query
                .OrderByDescending(s => s.FechaSolicitud)
                .Skip((filtro.Pagina - 1) * filtro.TamañoPagina)
                .Take(filtro.TamañoPagina)
                .ToListAsync();

            return filtro;
        }

        public async Task<byte[]> ExportarExcelAsync(SolicitudFiltroViewModel filtro, int userId, string rol)
        {
            // Reutilizamos la lógica de filtrado pero sin paginación para el reporte
            var query = _repo.GetQueryable();

            if (rol == "Solicitante") query = query.Where(s => s.UsuarioSolicitanteId == userId);
            if (rol == "Infra") query = query.Where(s => s.Estado == EstadoSolicitud.Aprobado || s.Estado == EstadoSolicitud.Implementado);

            if (filtro.Estado.HasValue) query = query.Where(s => s.Estado == filtro.Estado.Value);
            if (filtro.PlataformaId.HasValue) query = query.Where(s => s.PlataformaId == filtro.PlataformaId.Value);

            var solicitudes = await query.OrderByDescending(s => s.FechaSolicitud).ToListAsync();

            using var wb = new XLWorkbook();
            var dt = new DataTable("Solicitudes");
            dt.Columns.AddRange(new DataColumn[] {
                new("Código"), new("Fecha"), new("Solicitante"), new("Plataforma"),
                new("Tipo"), new("Estado"), new("Analista")
            });

            foreach (var s in solicitudes)
            {
                dt.Rows.Add(s.Codigo, s.FechaSolicitud.ToShortDateString(),
                            s.UsuarioSolicitante?.Nombre, s.Plataforma?.Nombre,
                            s.TipoAcceso.ToString(), s.Estado.ToString(),
                            s.Analista?.Nombre ?? "N/A");
            }

            var ws = wb.Worksheets.Add(dt);
            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            return ms.ToArray();
        }

        public async Task<SolicitudAcceso?> ObtenerDetalleAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }
    }
}
