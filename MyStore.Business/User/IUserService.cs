using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyStore.Entities;

namespace MyStore.Business
{
    public interface IUserService
    {
        User GetUserById(int id);
        User GetUserByAccount(string account);
        IList<User> GetAllUser();

    }
}
