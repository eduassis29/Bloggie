﻿using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories {
    public interface ITagRepository {
        Task<IEnumerable<Tag>> GetAllTagsAsync();

        Task<Tag?> GetTagsAsync(Guid id);

        Task<Tag> AddTagsAsync(Tag tag);

        Task<Tag?> UpdateTagsAsync(Tag tag);

        Task<Tag?> DeleteTagsAsync(Guid id);
    }
}
