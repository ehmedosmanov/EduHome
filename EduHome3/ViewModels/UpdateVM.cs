using System.ComponentModel.DataAnnotations;

namespace EduHome3.ViewModels
{
    public class UpdateVM
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
        public string Role { get; set; }

    }
}
