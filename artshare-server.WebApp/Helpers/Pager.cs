namespace artshare_server.WebApp.Helpers
{
    public class Pager
    {
        public int TotalItem { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }

        public Pager()
        {

        }

        public Pager(int totalItems, int page, int pageSize = 5)
        {
            int totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
            int current = page;
            int startPage = current - 2;
            int endPage = current + 2;
            if (startPage <= 0)
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;
            }
            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 5)
                {
                    startPage = endPage - 4;
                }
            }
            TotalItem = totalItems;
            TotalPages = totalPages;
            PageSize = pageSize;
            CurrentPage = current;
            StartPage = startPage;
            EndPage = endPage;
        }
    }
}
