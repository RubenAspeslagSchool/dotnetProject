using System.Transactions;

namespace Howest.MagicCards.WebAPI.Wrappers
{
    public class PagedResponse<T> : Response<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int) Math.Ceiling((double) TotalRecords / PageSize);
        public int TotalRecords { get; set; }

        public PagedResponse(T data, int pageNumber, int pageSize): base(data)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
