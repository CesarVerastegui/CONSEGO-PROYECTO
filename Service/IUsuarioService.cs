using CONSEGO.DTOs;

namespace CONSEGO.Service
{
    public interface IUsuarioService
    {
        Task<IEnumerable<UsuarioResponseDTO>> ListarUsuariosAsync();
        Task<bool> CrearUsuarioAsync(UsuarioCreateDTO dto);
        Task<UsuarioUpdateDTO?> ObtenerParaEditarAsync(int id);
        Task<bool> ActualizarUsuarioAsync(UsuarioUpdateDTO dto);
        Task<bool> ToggleEstadoAsync(int id);
        Task<string?> EliminarUsuarioAsync(int id);
    }
}
