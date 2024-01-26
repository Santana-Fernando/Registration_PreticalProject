using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Login.ViewModel
{
    public class LoginEntryViewModel
    {
        [Required(ErrorMessage = "The E-mail is Required")]
        [MinLength(10)]
        [MaxLength(100)]
        [DisplayName("E-mail")]
        public string email { get; set; }
        [Required(ErrorMessage = "The Password is Required")]
        [MinLength(6)]
        [MaxLength(10)]
        [DisplayName("Password")]
        public string password { get; set; }
    }
}
