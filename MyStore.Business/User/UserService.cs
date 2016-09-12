using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyStore.Business.Specifications;
using MyStore.DataAccess.Interface;
using MyStore.Entities;

namespace MyStore.Business
{
    public class UserService : ServiceBase, IUserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IDbContext context)
            : base(context)
        {
            _userRepository = Context.GetRepository<User>();
        }

        public User GetUserById(int id)
        {
            User user = null;
            if (id > 0)
            {
                user = _userRepository.Get(id);
            }
            return user;
        }

        public User GetUserByAccount(string account)
        {
            User user = null;
            if (!String.IsNullOrWhiteSpace(account))
            {
                user = _userRepository.Get(false, UserQuery.WithAccount(account));
            }
            return user;
        }

        public IList<User> GetAllUser()
        {
            return _userRepository.GetsReadOnly(null).ToList();
        }
    }
}