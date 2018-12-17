using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mooc.Models
{
    public class EnumModels
    {
        public enum RoleTypeEnum
        {
            超级管理员 = 1,
            一般管理员 = 2,
            普通用户 = 3,
            游客 = 4
        }

        public enum UserStateEnum
        {
            正常 = 0,
            锁定 = 1,
            删除 = 10
        }

    }
}
