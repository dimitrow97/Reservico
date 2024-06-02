namespace Reservico.Common.Models
{
    public class ListViewModel<T> where T : class
    {
        public int TotalCount { get; set; }

        public int NumberOfPages { get; set; }

        public IEnumerable<T> Data { get; set; }
    }
}