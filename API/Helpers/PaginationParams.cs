namespace API.Helpers
{
    public class PaginationParams
    {
        
        private int _pageSize = 10;                 //Page Size Predefined in 10
        private const int MaxPageSize = 50;         // Max Page Size constant to 50 
        public int PageNumber { get; set; } = 1;    // Page Number Predefined to 1
        public int PageSize                         //PageSize Limited to 50
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}