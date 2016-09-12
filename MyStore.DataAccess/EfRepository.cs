using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using MyStore.DataAccess.Interface;

namespace MyStore.DataAccess
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        private readonly IDbContext _context;
        private IDbSet<T> _entities;

        public EfRepository(IDbContext context)
        {
            _context = context;
        }

        private IDbSet<T> Entities
        {
            get
            {
                return _entities ?? (_entities = _context.Set<T>());
            }
        }

        public IEnumerable<T> Gets(bool isReadOnly, System.Linq.Expressions.Expression<Func<T, bool>> spec = null, Func<IQueryable<T>, IQueryable<T>> preFilers = null, params Func<IQueryable<T>, IQueryable<T>>[] postFilers)
        {
            return FindCore(isReadOnly, spec, preFilers, postFilers);
        }

        public IEnumerable<T> GetsReadOnly(System.Linq.Expressions.Expression<Func<T, bool>> spec = null, Func<IQueryable<T>, IQueryable<T>> preFilers = null, params Func<IQueryable<T>, IQueryable<T>>[] postFilers)
        {
            return FindCore(true, spec, preFilers, postFilers);
        }

        public T Get(params object[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                throw new ArgumentException("ids");
            }
            return Entities.Find(ids);
        }

        public T Get(bool isReadOnly, System.Linq.Expressions.Expression<Func<T, bool>> spec)
        {
            if (spec == null)
            {
                throw new ArgumentException("spec");
            }

            return isReadOnly ? Entities.AsNoTracking().SingleOrDefault(spec) : Entities.SingleOrDefault(spec);
        }

        public T GetReadOnly(System.Linq.Expressions.Expression<Func<T, bool>> spec)
        {
            return Get(true, spec);
        }

        public bool Exist(System.Linq.Expressions.Expression<Func<T, bool>> spec = null)
        {
            return spec == null ? Entities.Any() : Entities.Any(spec);
        }

        public int Count(System.Linq.Expressions.Expression<Func<T, bool>> spec = null)
        {
            return spec == null ? Entities.Count() : Entities.Count(spec);
        }

        public void Create(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentException("entity");
                }
                Entities.Add(entity);
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = "";
                foreach (var validationsErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationsError in validationsErrors.ValidationErrors)
                    {
                        msg += string.Format("Property: {0} Error: {1}", validationsError.PropertyName, validationsError.ErrorMessage) + Environment.NewLine;
                    }
                    var fail = new Exception(msg, dbEx);
                    throw fail;
                }
                throw;
            }
        }

        public void Delete(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentException("entity");
                }
                Entities.Remove(entity);
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = "";
                foreach (var validationsErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationsError in validationsErrors.ValidationErrors)
                    {
                        msg += string.Format("Property: {0} Error: {1}", validationsError.PropertyName, validationsError.ErrorMessage) + Environment.NewLine;
                    }
                    var fail = new Exception(msg, dbEx);
                    throw fail;
                }
                throw;
            }
        }

        private IQueryable<T> FindCore(bool isReadOnly, Expression<Func<T, bool>> spec, Func<IQueryable<T>, IQueryable<T>> preFilter, params Func<IQueryable<T>, IQueryable<T>>[] postFilters)
        {
            var entities = isReadOnly ? Entities.AsNoTracking() : Entities;
            var result = preFilter != null ? preFilter(entities) : entities;
            if (spec != null)
            {
                result = result.Where(spec);
            }
            foreach (var postFilter in postFilters)
            {
                if (postFilter != null)
                {
                    result = postFilter(result);
                }
            }
            return result;
        }
    }
}