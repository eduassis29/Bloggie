using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories {
    public interface ITagRepository {
        Task<IEnumerable<Tag>> GetAllTagsAsync(string? searchQuery = null, string? sortBy = null, string? sortDirection = null, int pageNumber = 1, int pageSize = 100);

        Task<Tag?> GetTagsAsync(Guid id);

        Task<Tag> AddTagsAsync(Tag tag);

        Task<Tag?> UpdateTagsAsync(Tag tag);

        Task<Tag?> DeleteTagsAsync(Guid id);

        Task<int> CountAsync();
    }
}
