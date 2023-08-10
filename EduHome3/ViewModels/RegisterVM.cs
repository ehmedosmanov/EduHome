using System.ComponentModel.DataAnnotations;

namespace EduHome3.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Bu xana bos ola bilmez")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Bu xana bos ola bilmez")]

        public string Surname { get; set; }

        [Required(ErrorMessage = "Bu xana bos ola bilmez")]

        public string Username { get; set; }

        [Required(ErrorMessage = "Bu xana bos ola bilmez")]
        [DataType(DataType.EmailAddress)]

        public string Email { get; set; }

        [Required(ErrorMessage = "Bu xana bos ola bilmez")]
        [DataType(DataType.Password)]

        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Bu xana bos ola bilmez")]
        [Compare("Password", ErrorMessage = "Eyni parol deyil")]
        public string CheckPassword { get; set; }

        public bool IsRemember { get; set; }
    }
}
