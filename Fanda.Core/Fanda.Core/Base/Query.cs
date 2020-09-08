using System;

namespace Fanda.Core
{
    public class Query
    {
        public Query()
        {
        }

        public Query(string pageString, string pageSizeString)
        {
            Page = Convert.ToInt32(pageString);
            PageSize = Convert.ToInt32(pageSizeString);

            Page = Page <= 0 && PageSize > 0 ? 1 : Page;
            PageSize = PageSize <= 0 && Page > 0 ? 100 : PageSize;
        }

        //public string Search { get; set; }
        public string Filter { get; set; }

        public string[] FilterArgs { get; set; }
        public string Sort { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
