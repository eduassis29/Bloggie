using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Models.ViewModels {
    public class BlogDetailsViewModel {
        public Guid Id { get; set; }
        public string HeadingBlo { get; set; }
        public string PageTitleBlo { get; set; }
        public string ContentBlo { get; set; }
        public string ShortDescriptionBlo { get; set; }
        public string FeatureImageUrlBlo { get; set; }
        public string UrlHandleBlo { get; set; }
        public DateTime PublishedDateBlo { get; set; }
        public string AuthorBlo { get; set; }
        public bool VisibleBlo { get; set; }
        public ICollection<Tag> TagsBlo { get; set; }
        public int TotalLikes { get; set; }
        public bool Liked{ get; set; }
        public string CommentDescription { get; set; }
        public IEnumerable<BlogComment> Comments { get; set; }
    }
}
