using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using MyStore.Entities;

namespace MyStore.Business.Specifications
{
    public class UserQuery
    {
        public static Expression<Func<User, bool>> WithId(int userId)
        {
            return x => x.UserId == userId;
        }

        public static Expression<Func<User, bool>> WithAccount(string account)
        {
            return x => x.Account.Equals(account, StringComparison.InvariantCultureIgnoreCase);
        }

        public static Expression<Func<User, bool>> WithName(string name)
        {
            return x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase);
        }

    }
}