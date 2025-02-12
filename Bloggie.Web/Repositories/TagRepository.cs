using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories {
    public class TagRepository : ITagRepository {

        private readonly BloggieDbContext _bloggieDbContext;

        public TagRepository(BloggieDbContext bloggieDbContext)
        {
            this._bloggieDbContext = bloggieDbContext;
        }

        public async Task<Tag> AddTagsAsync(Tag tag) {
            await _bloggieDbContext.Tags.AddAsync(tag);
            await _bloggieDbContext.SaveChangesAsync();
            return tag;
        }

        public async Task<int> CountAsync() {
            return await _bloggieDbContext.Tags.CountAsync();
        }

        public async Task<Tag?> DeleteTagsAsync(Guid id) {
            var existingTags = await _bloggieDbContext.Tags.FindAsync(id);
            if (existingTags != null) { 
                _bloggieDbContext.Tags.Remove(existingTags);

                await _bloggieDbContext.SaveChangesAsync();
                return existingTags;
            }
            return null;
        }

        public async Task<IEnumerable<Tag>> GetAllTagsAsync(string? searchQuery, string? sortBy = null, string? sortDirection = null, int pageNumber = 1, int pageSize = 100) {
            var query = _bloggieDbContext.Tags.AsQueryable();
            //Filtering
            if (string.IsNullOrWhiteSpace(searchQuery) == false){
                query = query.Where(x => x.NameTag.Contains(searchQuery) || x.DisplayNameTag.Contains(searchQuery));
            }

            //Sorting
            if (string.Equals(sortBy, "Name", StringComparison.OrdinalIgnoreCase)){
                var isDesc = string.Equals(sortDirection, "Desc", StringComparison.OrdinalIgnoreCase);
                query = isDesc ? query.OrderByDescending(x => x.NameTag) : query.OrderBy(x => x.NameTag);
            }

            if (string.Equals(sortBy, "DisplayNameTag", StringComparison.OrdinalIgnoreCase)){
                var isDesc = string.Equals(sortDirection, "Desc", StringComparison.OrdinalIgnoreCase);
                query = isDesc ? query.OrderByDescending(x => x.DisplayNameTag) : query.OrderBy(x => x.DisplayNameTag);
            }

            //Paginatiom
            var skipResults = (pageNumber - 1) * pageSize;
            query = query.Skip(skipResults).Take(pageSize);


            return await query.ToListAsync();
        }


        public async Task<Tag?> GetTagsAsync(Guid id) {
            return await _bloggieDbContext.Tags.FindAsync(id);

        }

        public async Task<Tag?> UpdateTagsAsync(Tag tag) {
            var existingTag = await _bloggieDbContext.Tags.FindAsync(tag.Id);

            if (existingTag != null) {
                existingTag.NameTag = tag.NameTag;
                existingTag.DisplayNameTag = tag.DisplayNameTag;

                await _bloggieDbContext.SaveChangesAsync();

                return existingTag;
            }
            return null;

        }
    }
}
