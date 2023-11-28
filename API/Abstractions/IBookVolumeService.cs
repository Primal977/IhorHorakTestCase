using DTOs;

namespace API.Abstractions;

public interface IBookVolumeService
{
    Task<List<GoogleBookVolumeDTO>> GetByIDsAsync(List<string> ids);
}
