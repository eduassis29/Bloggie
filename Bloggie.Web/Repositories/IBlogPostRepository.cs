﻿using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories {
    public interface IBlogPostRepository {
        Task<IEnumerable<BlogPost>> GetAllBlogAsync();

        Task<BlogPost?> GetBlogAsync(Guid id);

        Task<BlogPost?> AddAsync(BlogPost blogPost);

        Task<BlogPost?> UpdateAsync(BlogPost blogPost);

        Task<BlogPost?> DeleteAsync(Guid id);

        Task<BlogPost?> GetByUrlHandleAsync(string urlHandle);
    }
}
