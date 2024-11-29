using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Linq;

namespace Bloggie.Web.Controllers {
    public class AdminBlogPostsController : Controller {

        private readonly ITagRepository _tagRepository;
        private readonly IBlogPostRepository _blogPostRepository;

        public AdminBlogPostsController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
        {
            _tagRepository = tagRepository;
            _blogPostRepository = blogPostRepository;
        }

        public IBlogPostRepository BlogPostRepository { get; set; }

        [HttpGet]
        public async Task<IActionResult> Add() {
            //get tags from repositry
            var tags = await _tagRepository.GetAllTagsAsync();
            var model = new AddBlogPostRequest {
                Tags = tags.Select(x => new SelectListItem { Text = x.NameTag, Value = x.Id.ToString()})
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostResquest) {
            //Map the view model to domain model

            var blogPost = new BlogPost {
                HeadingBlo = addBlogPostResquest.HeadingBlo,
                PageTitleBlo = addBlogPostResquest.PageTitleBlo,
                ContentBlo = addBlogPostResquest.ContentBlo,
                ShortDescriptionBlo = addBlogPostResquest.ShortDescriptionBlo,
                FeatureImageUrlBlo = addBlogPostResquest.FeatureImageUrlBlo,
                UrlHandleBlo = addBlogPostResquest.UrlHandleBlo,
                PublishedDateBlo = addBlogPostResquest.PublishedDateBlo,
                AuthorBlo = addBlogPostResquest.AuthorBlo,
                VisibleBlo = addBlogPostResquest.VisibleBlo
            };

            //Map Tags from selected Tags

            var selectedTags = new List<Tag>();

            foreach(var SelectedTagId in addBlogPostResquest.SelectedTags) {
                var selectedTagIdAsGuid = Guid.Parse(SelectedTagId);
                var existingTag = await _tagRepository.GetTagsAsync(selectedTagIdAsGuid);

                if (existingTag != null) { 
                    selectedTags.Add(existingTag);
                }
            }

            //Mapping tags back to domain models
            blogPost.TagsBlo = selectedTags;

            await _blogPostRepository.AddAsync(blogPost);
            return RedirectToAction("List");
        }

        [HttpGet("List")]
        public async Task<IActionResult> List() {
            var blogs = await _blogPostRepository.GetAllBlogAsync();
            return View(blogs);
        }


        [HttpGet]
        public async Task<IActionResult> EditAsync(Guid id) {
            //Retrive the result from the repository
            var blogPost = await _blogPostRepository.GetBlogAsync(id);
            var tagsDomainModel = await _tagRepository.GetAllTagsAsync();

            if (blogPost != null) {

                //Map the domain model into the view model
                var model = new EditBlogPostRequest {   
                    Id = blogPost.Id,
                    HeadingBlo = blogPost.HeadingBlo,
                    PageTitleBlo = blogPost.PageTitleBlo,
                    ContentBlo = blogPost.ContentBlo,
                    AuthorBlo = blogPost.AuthorBlo,
                    FeatureImageUrlBlo = blogPost.FeatureImageUrlBlo,
                    UrlHandleBlo = blogPost.UrlHandleBlo,
                    ShortDescriptionBlo = blogPost.ShortDescriptionBlo,
                    PublishedDateBlo = blogPost.PublishedDateBlo,
                    VisibleBlo = blogPost.VisibleBlo,
                    TagsBlo = tagsDomainModel.Select(x => new SelectListItem {
                        Text = x.NameTag, Value = x.Id.ToString()
                    }),
                    SelectedTags = blogPost.TagsBlo.Select(X => X.Id.ToString()).ToArray()
                };
                return View(model);
            }
            //Pass data to view
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(EditBlogPostRequest editBlogPostRequest) {
            var blogPostDomainModel = new BlogPost {
                Id = editBlogPostRequest.Id,
                HeadingBlo = editBlogPostRequest.HeadingBlo,
                PageTitleBlo = editBlogPostRequest.PageTitleBlo,
                ContentBlo = editBlogPostRequest.ContentBlo,
                AuthorBlo = editBlogPostRequest.AuthorBlo,
                FeatureImageUrlBlo = editBlogPostRequest.FeatureImageUrlBlo,
                UrlHandleBlo = editBlogPostRequest.UrlHandleBlo,
                ShortDescriptionBlo = editBlogPostRequest.ShortDescriptionBlo,
                PublishedDateBlo = editBlogPostRequest.PublishedDateBlo,
                VisibleBlo = editBlogPostRequest.VisibleBlo,
            };

            //Map Tags into domain Model

            var selectedTags = new List<Tag>();
            foreach (var selectedTag in editBlogPostRequest.SelectedTags) {
                if (Guid.TryParse(selectedTag, out var tag)) {
                    var foundTag = await _tagRepository.GetTagsAsync(tag);

                    if (foundTag != null) {
                        selectedTags.Add(foundTag);
                    }
                }
            }
            blogPostDomainModel.TagsBlo = selectedTags;

            //Submit information to repository update

            var updatedBlog = await _blogPostRepository.UpdateAsync(blogPostDomainModel);

            if (updatedBlog != null) {
                //Return Success notification
                return RedirectToAction("List");
            }
            //Return error notification
            return RedirectToAction("Edit");
        }

        [HttpPost]

        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest) {
            //Talk to repository do delete this blog post and tags
            var deletedBlogpost = await _blogPostRepository.DeleteAsync(editBlogPostRequest.Id);

            if (deletedBlogpost != null) {
                //Show success notification
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new {id = editBlogPostRequest.Id});

            //display the response

        }
    }
}
