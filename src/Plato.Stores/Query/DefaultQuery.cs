﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Plato.Abstractions.Query;
using Plato.Abstractions.Collections;

namespace Plato.Stores.Query
{
    public abstract class DefaultQuery : IQuery
    {
        private readonly Dictionary<string, OrderBy> _sortColumns;

        public IDictionary<string, OrderBy> SortColumns => _sortColumns;

        public int PageIndex { get; private set; }

        public int PageSize { get; private set; }

        protected DefaultQuery()
        {
            _sortColumns = new Dictionary<string, OrderBy>();
        }

        public IQuery Page(int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            return this;
        }

        public abstract IQuery Select<T>(Action<T> configure) where T : new();

        public abstract Task<IPagedResults<T>> ToList<T>() where T : class;
        
        public IQuery OrderBy(string columnName, OrderBy sortOrder = Abstractions.Query.OrderBy.Asc)
        {
            _sortColumns.Add(columnName, sortOrder);
            return this;
        }


    }

}