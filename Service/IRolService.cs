using CONSEGO.DTOs;

namespace CONSEGO.Service
{
    public interface IRolService
    {
        Task<IEnumerable<RolResponseDTO>> ListarRolesAsync();
        Task<RolUpdateDTO?> ObtenerParaEditarAsync(int id);
        Task<RolResponseDTO?> ObtenerDetallesAsync(int id);
        Task<bool> CrearRolAsync(RolCreateDTO dto);
        Task<bool> ActualizarRolAsync(RolUpdateDTO dto);
        Task<string?> EliminarRolAsync(int id);
    }
}
