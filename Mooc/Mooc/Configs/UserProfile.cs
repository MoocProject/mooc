using AutoMapper;
using Mooc.Models;
using Mooc.ViewModels;

namespace Mooc.Configs
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserViewModel>();
        }
    }
}
