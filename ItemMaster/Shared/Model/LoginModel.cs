using System.ComponentModel.DataAnnotations;

namespace ItemMaster.Shared.Model
{
    public class LoginModel
    {
        public string Code { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
