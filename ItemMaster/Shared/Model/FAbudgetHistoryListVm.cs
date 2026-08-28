namespace ItemMaster.Shared.Model
{
    public class FAbudgetHistoryListVm
    {
        public int Id { get; set; } // Primary Key for History
        public int? No { get; set; } // Primary Key for History
        public int FABudgetId { get; set; } // Foreign Key to FABudget
        public DateTime? ChangeDate { get; set; } // When the change happened
        public string? ChangedBy { get; set; } // User who made the change
        public string? OldValue { get; set; } // Serialized old values (before change)
        public string? NewValue { get; set; } // Serialized new values (after change)
    }
}
