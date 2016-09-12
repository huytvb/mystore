using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using MyStore.Entities;

namespace MyStore.DataAccess.Mapping
{
    public class UserMapSqlServer : EntityTypeConfiguration<User>
    {
        public UserMapSqlServer()
        {
            ToTable("user");
            HasKey(x => x.UserId);
            Property(x => x.Name).HasColumnType("nvarchar").HasMaxLength(100).IsRequired();
            Property(x => x.Account).HasColumnType("varchar").HasMaxLength(16).IsRequired();
            Property(x => x.Account).HasColumnType("varchar").HasMaxLength(1000).IsRequired();
        }
    }

    public class UserMapMySql : EntityTypeConfiguration<User>
    {
        public UserMapMySql()
        {
            ToTable("user");
            HasKey(x => x.UserId);
            Property(x => x.Name).HasColumnType("varchar").HasMaxLength(100).IsRequired();
            Property(x => x.Account).HasColumnType("varchar").HasMaxLength(16).IsRequired();
            Property(x => x.Account).HasColumnType("varchar").HasMaxLength(1000).IsRequired();
        }
    }
}