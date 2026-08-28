using System.ComponentModel.DataAnnotations.Schema;

namespace ItemMaster.Server.Data.Entities
{
    [Table("DivisionSign")]
    public class Division
    {
        public int Id { get; set; }
        public string DivisionName { get; set; }
    }
}
