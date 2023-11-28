using Common;
using DTOs;

namespace CustomApiClient
{
    public interface ICustomApiClient
    {
        Task<GoogleBookVolumesResultDTO> FindGoogleVolume(string query);

        Task<int> CreateNewCategory(CategoryCreateDTO categoryCreateDTO);
        Task<CategoryDTO> GetCategoryById(int categoryId);
        Task<List<CategoryDTO>> GetAllCategories();
        Task UpdateCategory(CategoryDTO updatedCategory);
        Task DeleteCategory(int categoryId);
    }

    public class CustomApiClient : BaseAPIClient<CustomApiClient>, ICustomApiClient
    {
        private readonly HttpClient _client;

        public CustomApiClient(HttpClient client) : base(client, "https://localhost:7086/api/")
        {
            _client = client;
        }

        public async Task<GoogleBookVolumesResultDTO> FindGoogleVolume(string query)
        {
            return await Get<GoogleBookVolumesResultDTO>($"Volume/find?q={query}");
        }


        public async Task<int> CreateNewCategory(CategoryCreateDTO categoryCreateDTO)
        {
            return await Post<CategoryCreateDTO, int>($"Category", categoryCreateDTO);
        }

        public async Task<CategoryDTO> GetCategoryById(int categoryId)
        {
            return await Get<CategoryDTO>($"Category/{categoryId}");
        }

        public async Task<List<CategoryDTO>> GetAllCategories()
        {
            return await Get<List<CategoryDTO>>($"Category/All");
        }

        public async Task UpdateCategory(CategoryDTO updatedCategory)
        {
            await Put<CategoryDTO>($"Category", updatedCategory);
        }

        public async Task DeleteCategory(int categoryId)
        {
            await Delete($"Category/{categoryId}");
        }
    }
}