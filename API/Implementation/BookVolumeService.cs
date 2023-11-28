using API.Abstractions;
using DTOs;

namespace API.Implementation
{
    public class BookVolumeService : IBookVolumeService
    {
        private readonly IVolumeAPIClient _volumeAPIClient;
        public BookVolumeService(IVolumeAPIClient volumeAPIClient)
        {
            _volumeAPIClient = volumeAPIClient;
        }

        public async Task<List<GoogleBookVolumeDTO>> GetByIDsAsync(List<string> ids)
        {
            if (ids?.Any() != null)
            {
                var tasks = new List<Task<GoogleBookVolumeDTO>>();
                foreach (var id in ids)
                {
                    tasks.Add(_volumeAPIClient.GetVolumeById(id));
                }

                var results = await Task.WhenAll(tasks);

                return results.ToList();
            }
            else
            {
                return new List<GoogleBookVolumeDTO>();
            }
        }
    }
}
