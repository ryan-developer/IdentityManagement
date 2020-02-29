using System.Collections.Generic;

namespace IdentityManagement.Areas.Api.Contracts.Responses
{
    public class PaginationList<TListType>
    {
        public int Page { get; set; }

        public int PageCount { get; set; }

        public int ItemCount { get; set; }

        public int TotalCount { get; set; }

        public List<TListType> Items { get; set; }
    }
}
