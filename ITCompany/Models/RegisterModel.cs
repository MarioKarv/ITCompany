using System;
using System.ComponentModel.DataAnnotations;

namespace ITCompany.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Не вказане ім'я")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Не вказане прізвище")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Не вказан логін")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Не вказан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль уведено не вірно")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Не вибрана роль")]
        public int RoleID { get; set; }

        [Required(ErrorMessage = "Не вибраний аватар")]
        public string Avatar { get; set; }

    }
}
