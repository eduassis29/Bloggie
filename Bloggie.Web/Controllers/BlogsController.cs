using Bloggie.Web.Models.Domain;
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
        private readonly IBlogPostCommentRepository blogPostCommentRepository;

        public BlogsController(IBlogPostRepository blogPostRepository, 
            IBlogPostLikeRepository blogPostLikeRepository,
            SignInManager<IdentityUser> signManager,
            UserManager<IdentityUser> userManager,
            IBlogPostCommentRepository blogPostCommentRepository) {
            this.blogPostRepository = blogPostRepository;
            this.blogPostLikeRepository = blogPostLikeRepository;
            this.signManager = signManager;
            this.userManager = userManager;
            this.blogPostCommentRepository = blogPostCommentRepository;
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

                //Get comments for blog posts

                var blogCommentsDomainModel = await blogPostCommentRepository.GetCommentsByBlogIdAsync(blogPost.Id);
                var blogCommentsForView = new List<BlogComment>();
                
                foreach (var blogComment in blogCommentsDomainModel) { 
                    blogCommentsForView.Add(new BlogComment { 
                        Description = blogComment.Description ,
                        DateAdded = blogComment.DateAdded ,
                        Username = (await userManager.FindByIdAsync(blogComment.UserId.ToString())).UserName
                    });
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
                    Liked = liked,
                    Comments = blogCommentsForView,
                };


            }

            return View(BlogDetailsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(BlogDetailsViewModel blogDetailsViewModel) {

            if (signManager.IsSignedIn(User)){
                var domainModel = new BlogPostComment {
                    BlogPostId = blogDetailsViewModel.Id,
                    Description = blogDetailsViewModel.CommentDescription,
                    UserId = Guid.Parse(userManager.GetUserId(User)),
                    DateAdded = DateTime.Now,
                };
                await blogPostCommentRepository.AddAsync(domainModel);
                return RedirectToAction("Index", "Blogs", new { urlHandle = blogDetailsViewModel.UrlHandleBlo });
            }
            return View();

        }

    }
}
