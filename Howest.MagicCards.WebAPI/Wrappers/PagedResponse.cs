using System.Transactions;

namespace Howest.MagicCards.WebAPI.Wrappers
{
    public class PagedResponse<T>
    {
        public T Data { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public bool Succeeded { get; set; }
        public List<string> Errors { get; set; }
        public string Message { get; set; }

        public PagedResponse(T data, int pageNumber, int pageSize)
        {
            Data = data;
            PageNumber = pageNumber;
            PageSize = pageSize;
            Succeeded = true;
            Errors = null;
            Message = string.Empty;
        }
    }

}

