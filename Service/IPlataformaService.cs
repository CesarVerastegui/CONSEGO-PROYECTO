using CONSEGO.DTOs;

namespace CONSEGO.Service
{
    public interface IPlataformaService
    {
        Task<IEnumerable<PlataformaResponseDTO>> ListarTodoAsync();
        Task<PlataformaResponseDTO?> ObtenerDetallesAsync(int id);
        Task<PlataformaUpdateDTO?> ObtenerParaEditarAsync(int id);
        Task<bool> CrearAsync(PlataformaCreateDTO dto);
        Task<bool> ActualizarAsync(PlataformaUpdateDTO dto);
        Task<string?> EliminarAsync(int id);
    }
}
