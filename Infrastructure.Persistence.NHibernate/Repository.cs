﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Domain.Common;
using Infrastructure.Persistence.Common;
using NHibernate;
using NHibernate.Linq;

namespace Infrastructure.Persistence.NHibernate
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : IEntity
    {
        private IQueryable<TEntity> _session;

        public Repository(ISession session)
        {
            _session = session.Query<TEntity>();
        }

        public IRepository<TEntity> Specification(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate != null)
            {
                _session = _session.Where(predicate);
            }

            return this;
        }

        public IRepository<TEntity> Include(Expression<Func<TEntity, object>> path)
        {
            _session.Fetch(path).ToFuture();
            return this;
        }

        public IRepository<TEntity> OrderByAsc(Expression<Func<TEntity, object>> path)
        {
            _session = _session.OrderBy(path);
            return this;
        }

        public IRepository<TEntity> OrderByDesc(Expression<Func<TEntity, object>> path)
        {
            _session = _session.OrderByDescending(path);
            return this;
        }

        public IRepository<TEntity> Paged(int page, int pageSize)
        {
            _session = _session.Skip((page - 1) * pageSize).Take(pageSize);
            return this;
        }

        public TEntity First()
        {
            return _session.FirstOrDefault();
        }

        public List<TEntity> ToList()
        {
            return _session.ToList();
        }

        public int Count()
        {
            return _session.Count();
        }
    }
}