using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Usuario.ViewModel
{
    public class UsuariosViewModel
    {
        public int id { get; set; }
        
        [Required(ErrorMessage = "The Name is Required")]
        [MinLength(3)]
        [MaxLength(100)]
        [DisplayName("Name")]
        public string name { get; set; }
        
        [Required(ErrorMessage = "The E-mail is Required")]
        [MinLength(5)]
        [MaxLength(100)]
        [DisplayName("E-mail")]
        public string email { get; set; }

        [Required(ErrorMessage = "The Password is Required")]
        [MinLength(6)]
        [MaxLength(10)]
        [DisplayName("Password")]
        public string? password { get; set; }

        [Required(ErrorMessage = "The Password need to be confirmed")]
        [MinLength(6)]
        [MaxLength(10)]
        [DisplayName("Confirmed password")]
        [Compare("password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? passwordConfirmation { get; set; }
    }
}
