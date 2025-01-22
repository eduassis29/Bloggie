namespace Bloggie.Web.Models.Domain {
    public class BlogPost {
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

        //Navigation property

        public ICollection<Tag> TagsBlo { get; set; }

        public ICollection<BlogPostLike> Likes { get; set; }

        public ICollection<BlogPostComment> Comments { get; set; }

    }
}
