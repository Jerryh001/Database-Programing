using System;

namespace MVC.Models
{
    public class LoginModel
    {
        public Guid id { get; set; }
        public String in_username { get; set; }
        public String in_password { get; set; }
    }
}