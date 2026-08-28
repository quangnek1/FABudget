using System.ComponentModel.DataAnnotations;

namespace ItemMaster.Shared.Model
{
    public class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }


        public string? Password { get; set; }


        public string? ConfirmPassword { get; set; }

        public string? Code { get; set; }
    }
}
