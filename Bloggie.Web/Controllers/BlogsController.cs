using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace Bloggie.Web.Controllers {
    public class BlogsController : Controller {

        private readonly IBlogPostRepository blogPostRepository;
        private readonly IBlogPostLikeRepository blogPostLikeRepository;
        private readonly SignInManager<IdentityUser> signManager;
        private readonly UserManager<IdentityUser> userManager;

        public BlogsController(IBlogPostRepository blogPostRepository, 
            IBlogPostLikeRepository blogPostLikeRepository,
            SignInManager<IdentityUser> signManager,
            UserManager<IdentityUser> userManager) {
            this.blogPostRepository = blogPostRepository;
            this.blogPostLikeRepository = blogPostLikeRepository;
            this.signManager = signManager;
            this.userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle) {
            var liked = false;
            var blogPost = await blogPostRepository.GetByUrlHandleAsync(urlHandle);
            var BlogDetailsViewModel = new BlogDetailsViewModel();


            if (blogPost != null) {

                var totalLikes = await blogPostLikeRepository.GetTotalLikes(blogPost.Id);

                if (signManager.IsSignedIn(User)) {
                    //Get User Like For this Blog
                    var LikesForBlog = await blogPostLikeRepository.GetLikesForBlog(blogPost.Id);

                    var userId = userManager.GetUserId(User);

                    if (userId != null) {

                        var likeFromUser = LikesForBlog.FirstOrDefault(x => x.UserId == Guid.Parse(userId));
                        liked = likeFromUser != null;
                    }

                }

                BlogDetailsViewModel = new BlogDetailsViewModel {
                    Id = blogPost.Id,
                    ContentBlo = blogPost.ContentBlo,
                    PageTitleBlo = blogPost.PageTitleBlo,
                    AuthorBlo = blogPost.AuthorBlo,
                    FeatureImageUrlBlo = blogPost.FeatureImageUrlBlo,
                    HeadingBlo = blogPost.HeadingBlo,
                    PublishedDateBlo = blogPost.PublishedDateBlo,
                    ShortDescriptionBlo = blogPost.ShortDescriptionBlo,
                    UrlHandleBlo = blogPost.UrlHandleBlo,
                    VisibleBlo = blogPost.VisibleBlo,
                    TagsBlo = blogPost.TagsBlo,
                    TotalLikes = totalLikes,
                    Liked = liked
                };


            }

            return View(BlogDetailsViewModel);
        }

    }
}
