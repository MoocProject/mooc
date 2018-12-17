using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mooc.ViewModels
{
    public class UserViewModel
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string NickName { get; set; }
        public string ShowUserState { get; set; }
    }
}
