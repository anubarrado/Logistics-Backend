using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.DTOs.User
{
    public class UserDTO
    {
        public string IdEntidad { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        
        public bool State { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
