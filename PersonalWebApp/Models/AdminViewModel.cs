namespace PersonalWebApp.Models
{
    public class AdminViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public bool CanCreateBlogPost { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Message { get; set; }
    }
}
