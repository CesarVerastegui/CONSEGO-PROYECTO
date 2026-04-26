using CONSEGO.DTOs;
using CONSEGO.Models;
using CONSEGO.Repository;

namespace CONSEGO.Service
{
    public class PlataformaService : IPlataformaService
    {
        private readonly IPlataformaRepository _repository;

        public PlataformaService(IPlataformaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PlataformaResponseDTO>> ListarTodoAsync()
        {
            var plataformas = await _repository.GetAllAsync();
            var lista = new List<PlataformaResponseDTO>();
            foreach (var p in plataformas)
            {
                lista.Add(new PlataformaResponseDTO
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Tipo = p.Tipo,
                    Criticidad = p.Criticidad,
                    Activa = p.Activa,
                    CantidadSolicitudes = p.Solicitudes.Count
                });
            }
            return lista;
        }

        public async Task<bool> CrearAsync(PlataformaCreateDTO dto)
        {
            var existe = await _repository.GetByNombreAsync(dto.Nombre);
            if (existe != null) return false;

            var entidad = new Plataforma
            {
                Nombre = dto.Nombre,
                Tipo = dto.Tipo,
                Criticidad = dto.Criticidad,
                Activa = dto.Activa
            };

            await _repository.AddAsync(entidad);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<PlataformaUpdateDTO?> ObtenerParaEditarAsync(int id)
        {
            var p = await _repository.GetByIdAsync(id);
            if (p == null) return null;

            return new PlataformaUpdateDTO
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Tipo = p.Tipo,
                Criticidad = p.Criticidad,
                Activa = p.Activa
            };
        }

        public async Task<bool> ActualizarAsync(PlataformaUpdateDTO dto)
        {
            var entidad = await _repository.GetByIdAsync(dto.Id);
            if (entidad == null) return false;

            var duplicado = await _repository.GetByNombreAsync(dto.Nombre);
            if (duplicado != null && duplicado.Id != dto.Id) return false;

            entidad.Nombre = dto.Nombre;
            entidad.Tipo = dto.Tipo;
            entidad.Criticidad = dto.Criticidad;
            entidad.Activa = dto.Activa;

            _repository.Update(entidad);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<string?> EliminarAsync(int id)
        {
            var entidad = await _repository.GetByIdAsync(id);
            if (entidad == null) return "La plataforma no existe.";
            if (entidad.Solicitudes.Any()) return "No se puede eliminar la plataforma porque tiene solicitudes asociadas.";

            _repository.Delete(entidad);
            await _repository.SaveChangesAsync();
            return null;
        }

        public async Task<PlataformaResponseDTO?> ObtenerDetallesAsync(int id)
        {
            var p = await _repository.GetByIdAsync(id);
            if (p == null) return null;

            return new PlataformaResponseDTO
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Tipo = p.Tipo,
                Criticidad = p.Criticidad,
                Activa = p.Activa,
                CantidadSolicitudes = p.Solicitudes.Count
            };
        }
    }
}
