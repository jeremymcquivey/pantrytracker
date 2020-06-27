using System.ComponentModel.DataAnnotations;

namespace PantryTrackers
{
    public class RecoverViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
