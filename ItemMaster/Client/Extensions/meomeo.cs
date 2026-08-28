using MudBlazor;

namespace ItemMaster.Client.Extensions
{
    public class meomeo<T> : TableGroupDefinition<T>
    {
        public bool? ShowDetails { get; set; }
        public int? Id { get; set; }
        public DateTime? DateTime { get; set; }
    }
}
