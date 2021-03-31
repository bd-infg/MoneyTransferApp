using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationServices.DTOs
{
    public class AccountPasswordRequestDTO
    {
        public string Id { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get;  set; }
    }
}
