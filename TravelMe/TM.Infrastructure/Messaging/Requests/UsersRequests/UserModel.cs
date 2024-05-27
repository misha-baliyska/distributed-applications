using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.Data.Entities.Enums;

namespace TM.Infrastructure.Messaging.Requests.UsersRequests
{
    public class UserModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required!")]
        [MaxLength(25)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MaxLength(25, ErrorMessage = "Password should be less than 25 characters!")]
        [MinLength(6, ErrorMessage = "Password should be more than 6 characters!")]
        public string Password { get; set; }
        public bool IsAdmin { get; set; }

        [Required(ErrorMessage = "FirstName is required.")]
        [MaxLength(20, ErrorMessage = "Name should be less than 20 characters!")]
        required public string FirstName { get; set; }

        [MaxLength(20, ErrorMessage = "Name should be less than 20 characters!")]
        [Required(ErrorMessage = "LastName is required!")]
        required public string LastName { get; set; }

        [MaxLength(25, ErrorMessage = "Email should be less than 25 characters!")]
        [Required(ErrorMessage = "Email is required!")]
        required public string Email { get; set; }

        public Gender Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

    }
}
