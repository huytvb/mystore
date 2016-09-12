using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyStore.DataAccess.Interface;

namespace MyStore.Business
{
    public class ServiceBase : IDisposable
    {
        protected readonly IDbContext Context;
        protected ServiceBase(IDbContext context)
        {
            Context = context;
        }

        public void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
            }
        }
    }
}