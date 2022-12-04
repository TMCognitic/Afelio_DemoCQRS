using System.ComponentModel.DataAnnotations;

namespace Afelio_DemoCQRS.Models.Forms
{
#nullable disable
    public class LoginForm
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Passwd { get; set; }
    }
#nullable enable
}
