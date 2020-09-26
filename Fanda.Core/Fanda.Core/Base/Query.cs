using System;

namespace Fanda.Core
{
    public class Query
    {
        public Query()
        {
        }

        public Query(string pageIndexString, string pageSizeString)
        {
            PageIndex = Convert.ToInt32(pageIndexString);
            PageSize = Convert.ToInt32(pageSizeString);

            PageIndex = PageIndex <= 0 && PageSize > 0 ? 1 : PageIndex;
            PageSize = PageSize <= 0 && PageIndex > 0 ? 100 : PageSize;
        }

        //public string Search { get; set; }
        public string Filter { get; set; }

        public string[] FilterArgs { get; set; }
        public string Sort { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}