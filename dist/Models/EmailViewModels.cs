namespace Mailer.Models
{
    public class EmailViewModelBase
    {
        public string WebVersion { get; set; }
    }

    public class ConfirmationEmailViewModel : EmailViewModelBase
    {
        public string ConfirmationLink { get; set; }
    }

    public class NewsEmailViewModel : EmailViewModelBase
    {
        public string Title { get; set; }

        public string ImageLink { get; set; }

        public string Description { get; set; }

        public string ReadMoreLink { get; set; }
    }
}