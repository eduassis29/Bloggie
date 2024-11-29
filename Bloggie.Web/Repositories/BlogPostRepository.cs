using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories {
    public class BlogPostRepository : IBlogPostRepository {
        private readonly BloggieDbContext bloggieDbContext;

        public BlogPostRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }

        public async Task<BlogPost> AddAsync(BlogPost blogPost) {
            await bloggieDbContext.AddAsync(blogPost);
            await bloggieDbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id) {
            var existingBlog = await bloggieDbContext.BlogPosts.FindAsync(id);

            if (existingBlog != null) { 
                bloggieDbContext.BlogPosts.Remove(existingBlog);
                await bloggieDbContext.SaveChangesAsync();
                return existingBlog;
            }
            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllBlogAsync() {
            return await bloggieDbContext.BlogPosts.Include(x => x.TagsBlo).ToListAsync();
        }

        public async Task<BlogPost?> GetBlogAsync(Guid id) {
            return await bloggieDbContext.BlogPosts.Include(x => x.TagsBlo).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle) {
            return await bloggieDbContext.BlogPosts.Include(x => x.TagsBlo)
                .FirstOrDefaultAsync(x => x.UrlHandleBlo == urlHandle);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost) {
            var existingBlogPost = await bloggieDbContext.BlogPosts.Include(x => x.TagsBlo).FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            if (existingBlogPost != null) {
                existingBlogPost.Id = blogPost.Id;
                existingBlogPost.HeadingBlo = blogPost.HeadingBlo;
                existingBlogPost.PageTitleBlo = blogPost.PageTitleBlo;
                existingBlogPost.ContentBlo = blogPost.ContentBlo;
                existingBlogPost.AuthorBlo = blogPost.AuthorBlo;
                existingBlogPost.FeatureImageUrlBlo = blogPost.FeatureImageUrlBlo;
                existingBlogPost.UrlHandleBlo = blogPost.UrlHandleBlo;
                existingBlogPost.ShortDescriptionBlo = blogPost.ShortDescriptionBlo;
                existingBlogPost.PublishedDateBlo = blogPost.PublishedDateBlo;
                existingBlogPost.VisibleBlo = blogPost.VisibleBlo;
                existingBlogPost.TagsBlo = blogPost.TagsBlo;

                await bloggieDbContext.SaveChangesAsync();
                return existingBlogPost;
            }
            return null;
        }
    }
}
