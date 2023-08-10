using System.ComponentModel.DataAnnotations;

namespace EduHome3.ViewModels
{
    public class ForgotPasswordVM
    {
        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
    }
}
