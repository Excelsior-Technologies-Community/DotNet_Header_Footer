using System.ComponentModel.DataAnnotations;

namespace DotNet_Header_Footer.Models
{
    public class RegisterViewModel
    {
        [Required(
            ErrorMessage = "Please enter your full name.")]
        [StringLength(
            100,
            MinimumLength = 3,
            ErrorMessage = "Name must be between 3 and 100 characters.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;


        [Required(
            ErrorMessage = "Please enter your email address.")]
        [EmailAddress(
            ErrorMessage = "Please enter a valid email address.")]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;


        [Required(
            ErrorMessage = "Please enter your phone number.")]
        [Phone(
            ErrorMessage = "Please enter a valid phone number.")]
        [RegularExpression(
            @"^[6-9]\d{9}$",
            ErrorMessage = "Enter a valid 10-digit Indian mobile number.")]
        public string Phone { get; set; } = string.Empty;


        [Required(
            ErrorMessage = "Please enter a password.")]
        [StringLength(
            100,
            MinimumLength = 8,
            ErrorMessage = "Password must be at least 8 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;


        [Required(
            ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [Compare(
            "Password",
            ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}