using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitionsWebApp.Common.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Не указан логин")]
        public string? Login { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
