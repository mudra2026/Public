using System.ComponentModel.DataAnnotations;

namespace PurviEnterprises.Models;

public class EnquiryViewModel
{
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    [Required, StringLength(80)]
    [Display(Name = "Your name")]
    public string Name { get; set; } = string.Empty;

    [Required, Phone]
    [Display(Name = "Phone number")]
    public string Phone { get; set; } = string.Empty;

    [Required, EmailAddress]
    [Display(Name = "Email address")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Service")]
    public string Service { get; set; } = string.Empty;

    [Required, StringLength(1000)]
    [Display(Name = "Project details")]
    public string Message { get; set; } = string.Empty;
}
