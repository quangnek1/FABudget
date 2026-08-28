using ItemMaster.Server.Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("FABudgetHistory")]
public class FABudgetHistory
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; } // Primary Key for History

	public int FABudgetId { get; set; } // Foreign Key to FABudget
	public int? No { get; set; } 
	public DateTime? ChangeDate { get; set; } // When the change happened
	[MaxLength(50)]
	[Column(TypeName = "nvarchar(50)")]
	public string? ChangedBy { get; set; } // User who made the change

	// Columns to store the previous and current values
	public string? OldValue { get; set; } // Serialized old values (before change)
	public string? NewValue { get; set; } // Serialized new values (after change)

	// Add a navigation property for the foreign key
	[ForeignKey("FABudgetId")]
	public FABudget FABudget { get; set; }
}
