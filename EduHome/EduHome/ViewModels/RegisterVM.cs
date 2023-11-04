using System.ComponentModel.DataAnnotations;

namespace EduHome.ViewModels
{
    public class RegisterVM
    {
        [Required]
        //Required da  xanaları boş verə bilməsil deye verilir
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        //DataType  type dəyişmək üçün istifadə olunur 
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
