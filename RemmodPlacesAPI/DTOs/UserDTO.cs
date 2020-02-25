using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RemmodPlacesAPI.DTOs
{
    public class UserDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [MaxLength(8,ErrorMessage = "password maximum length is 8")]
        [MinLength(4 , ErrorMessage = "password minimum length is 4")]
        public string PassWord { get; set; }
    }
}
