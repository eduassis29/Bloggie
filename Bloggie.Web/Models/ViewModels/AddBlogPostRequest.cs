using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggie.Web.Models.ViewModels {
    public class AddBlogPostRequest {
        public string HeadingBlo { get; set; }

        public string PageTitleBlo { get; set; }

        public string ContentBlo { get; set; }

        public string ShortDescriptionBlo { get; set; }

        public string FeatureImageUrlBlo { get; set; }

        public string UrlHandleBlo { get; set; }

        public DateTime PublishedDateBlo { get; set; }

        public string AuthorBlo { get; set; }

        public bool VisibleBlo { get; set; }

        //Display Tags

        public IEnumerable<SelectListItem> Tags { get; set; }

        //Collect Tags

        public string[] SelectedTags { get; set; } = Array.Empty<string>();
    }
}
