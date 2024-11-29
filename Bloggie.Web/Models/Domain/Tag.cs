using Microsoft.AspNetCore.Identity;

namespace Bloggie.Web.Models.Domain {
    public class Tag {
        public Guid Id { get; set; }

        public string NameTag { get; set; }

        public string DisplayNameTag { get; set; }

        public ICollection<BlogPost> BlogPostTag { get; set; }
    }
}
