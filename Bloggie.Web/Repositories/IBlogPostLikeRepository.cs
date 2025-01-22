using Bloggie.Web.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Repositories {
    public interface IBlogPostLikeRepository {
    
        Task<int> GetTotalLikes(Guid blogPostid);

        Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike);

        Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId);


    }

}
