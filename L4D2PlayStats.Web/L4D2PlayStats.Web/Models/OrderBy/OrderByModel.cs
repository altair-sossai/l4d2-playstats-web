using L4D2PlayStats.Web.Models.OrderBy.Enum;

namespace L4D2PlayStats.Web.Models.OrderBy
{
    public class OrderByOption
    {
        public OrderByFilter Value { get; set; }
        public string? FriendlyName { get; set; }
    }
    public class OrderByModel
    {
        public IEnumerable<OrderByOption>? OrderByOptions { get; set; }
    }    
}
