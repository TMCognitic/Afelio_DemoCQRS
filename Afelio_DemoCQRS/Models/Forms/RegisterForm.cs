using System.ComponentModel.DataAnnotations;

namespace Afelio_DemoCQRS.Models.Forms
{
#nullable disable
    public class RegisterForm
    {
        [Required]
        public string Nom { get; set; }
        [Required]
        public string Prenom { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Anniversaire { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Passwd { get; set; }
        [Compare(nameof(Passwd))]
        [DataType(DataType.Password)]
        public string Confirm { get; set; }
    }
#nullable enable
}
