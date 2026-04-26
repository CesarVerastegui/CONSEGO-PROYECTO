using CONSEGO.DTOs;
using CONSEGO.Models;
using CONSEGO.Repository;

namespace CONSEGO.Service
{
    public class RolService : IRolService
    {
        private readonly IRolRepository _repository;

        public RolService(IRolRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<RolResponseDTO>> ListarRolesAsync()
        {
            var roles = await _repository.GetAllAsync();
            var listaDto = new List<RolResponseDTO>();

            foreach (var r in roles)
            {
                listaDto.Add(new RolResponseDTO
                {
                    Id = r.Id,
                    Nombre = r.Nombre,
                    Descripcion = r.Descripcion,
                    CantidadUsuarios = r.Usuarios.Count
                });
            }
            return listaDto;
        }

        public async Task<bool> CrearRolAsync(RolCreateDTO dto)
        {
            var existe = await _repository.GetByNombreAsync(dto.Nombre);
            if (existe != null) return false;

            var nuevoRol = new Rol
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion
            };

            await _repository.AddAsync(nuevoRol);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<RolUpdateDTO?> ObtenerParaEditarAsync(int id)
        {
            var rol = await _repository.GetByIdAsync(id);
            if (rol == null) return null;

            return new RolUpdateDTO
            {
                Id = rol.Id,
                Nombre = rol.Nombre,
                Descripcion = rol.Descripcion
            };
        }

        public async Task<bool> ActualizarRolAsync(RolUpdateDTO dto)
        {
            var rol = await _repository.GetByIdAsync(dto.Id);
            if (rol == null) return false;

            var rolConMismoNombre = await _repository.GetByNombreAsync(dto.Nombre);
            if (rolConMismoNombre != null && rolConMismoNombre.Id != dto.Id) return false;

            rol.Nombre = dto.Nombre;
            rol.Descripcion = dto.Descripcion;

            _repository.Update(rol);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<string?> EliminarRolAsync(int id)
        {
            var rol = await _repository.GetByIdAsync(id);
            if (rol == null) return "El rol no existe.";

            if (rol.Usuarios.Count > 0)
                return "No se puede eliminar el rol porque tiene usuarios asignados.";

            _repository.Delete(rol);
            await _repository.SaveChangesAsync();
            return null;
        }

        public async Task<RolResponseDTO?> ObtenerDetallesAsync(int id)
        {
            var rol = await _repository.GetByIdAsync(id);
            if (rol == null) return null;

            return new RolResponseDTO
            {
                Id = rol.Id,
                Nombre = rol.Nombre,
                Descripcion = rol.Descripcion,
                CantidadUsuarios = rol.Usuarios.Count
            };
        }
    }
}