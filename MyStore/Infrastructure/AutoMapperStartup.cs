using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using MyStore.Entities;
using MyStore.Models;

namespace MyStore.Infrastructure
{
    public class AutoMapperStartup
    {
        public static void Initialize()
        {
            #region 'User'
            Mapper.CreateMap<User, UserModel>();
            Mapper.CreateMap<UserModel, User>();
            #endregion
        }
    }
}