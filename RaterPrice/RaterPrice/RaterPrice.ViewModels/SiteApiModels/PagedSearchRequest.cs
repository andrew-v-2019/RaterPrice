namespace RaterPrice.Api.Areas.SiteApi.Models
{
    public class PagedSearchRequest
    {
        public int Page { get; set; }

        public int Take { get; set; }

        public int Skip => Take*Page;
    }
}