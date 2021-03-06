﻿using System;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using PlatoCore.Abstractions.Extensions;
using PlatoCore.Data.Abstractions;
using PlatoCore.Stores.Abstractions;
using PlatoCore.Stores.Abstractions.FederatedQueries;
using PlatoCore.Stores.Abstractions.QueryAdapters;

namespace Plato.Entities.Stores
{
    #region "FeatureEntityCountQuery"

    public class FeatureEntityCountQuery<TModel> : DefaultQuery<TModel> where TModel : class
    {

        public IFederatedQueryManager<TModel> FederatedQueryManager { get; set; }

        public IQueryAdapterManager<TModel> QueryAdapterManager { get; set; }

        public FeatureEntityCountQueryParams Params { get; set; }

        private readonly IQueryableStore<TModel> _store;

        public FeatureEntityCountQueryBuilder<TModel> Builder { get; private set; }

        public FeatureEntityCountQuery(IQueryableStore<TModel> store)
        {
            _store = store;
        }

        public override IQuery<TModel> Select<T>(Action<T> configure)
        {
            var defaultParams = new T();
            configure(defaultParams);
            Params = (FeatureEntityCountQueryParams)Convert.ChangeType(defaultParams, typeof(FeatureEntityCountQueryParams));
            return this;
        }

        public override async Task<IPagedResults<TModel>> ToList()
        {

            Builder = new FeatureEntityCountQueryBuilder<TModel>(this);
            var populateSql = Builder.BuildSqlPopulate();
            var countSql = Builder.BuildSqlCount();
            var keywords = Params?.Keywords.Value.ToEmptyIfNull() ?? string.Empty;

            return await _store.SelectAsync(new IDbDataParameter[]
            {
                new DbParam("PageIndex", DbType.Int32, PageIndex),
                new DbParam("PageSize", DbType.Int32, PageSize),
                new DbParam("SqlPopulate", DbType.String, populateSql),
                new DbParam("SqlCount", DbType.String, countSql),
                new DbParam("Keywords", DbType.String, keywords)
            });
        }

    }

    #endregion

    #region "FeatureEntityCountQueryParams"

    public class FeatureEntityCountQueryParams
    {

        private WhereInt _id;
        private WhereInt _userId;
        private WhereInt _categoryId;
        private WhereInt _roleId;
        private WhereInt _labelId;
        private WhereInt _tagId;
        private WhereString _keywords;
        private WhereBool _showHidden;
        private WhereBool _hideHidden;
        private WhereBool _showPrivate;
        private WhereBool _hidePrivate;
        private WhereBool _showSpam;
        private WhereBool _hideSpam;
        private WhereBool _showClosed;
        private WhereBool _hideClosed;
        private WhereBool _isPinned;
        private WhereBool _isNotPinned;
        private WhereBool _hideDeleted;
        private WhereBool _showDeleted;
        private WhereBool _isClosed;
        private WhereInt _createdUserId;
        private WhereDate _createdDate;
        private WhereInt _modifiedUserId;
        private WhereDate _modifiedDate;
        private WhereInt _participatedUserId;
        private WhereInt _followUserId;
        private WhereInt _starUserId;
        private WhereInt _totalViews;
        private WhereInt _totalReplies;
        private WhereInt _totalParticipants;
        private WhereInt _totalReactions;
        private WhereInt _totalFollows;
        private WhereInt _totalStars;

        public WhereInt Id
        {
            get => _id ?? (_id = new WhereInt());
            set => _id = value;
        }

        public WhereInt UserId
        {
            get => _userId ?? (_userId = new WhereInt());
            set => _userId = value;
        }

        public WhereInt CategoryId
        {
            get => _categoryId ?? (_categoryId = new WhereInt());
            set => _categoryId = value;
        }

        public WhereInt RoleId
        {
            get => _roleId ?? (_roleId = new WhereInt());
            set => _roleId = value;
        }

        public WhereInt LabelId
        {
            get => _labelId ?? (_labelId = new WhereInt());
            set => _labelId = value;
        }

        public WhereInt TagId
        {
            get => _tagId ?? (_tagId = new WhereInt());
            set => _tagId = value;
        }

        public WhereString Keywords
        {
            get => _keywords ?? (_keywords = new WhereString());
            set => _keywords = value;
        }

        public WhereBool ShowHidden
        {
            get => _showHidden ?? (_showHidden = new WhereBool());
            set => _showHidden = value;
        }

        public WhereBool HideHidden
        {
            get => _hideHidden ?? (_hideHidden = new WhereBool());
            set => _hideHidden = value;
        }

        public WhereBool ShowPrivate
        {
            get => _showPrivate ?? (_showPrivate = new WhereBool());
            set => _showPrivate = value;
        }

        public WhereBool HidePrivate
        {
            get => _hidePrivate ?? (_hidePrivate = new WhereBool());
            set => _hidePrivate = value;
        }

        public WhereBool HideSpam
        {
            get => _hideSpam ?? (_hideSpam = new WhereBool());
            set => _hideSpam = value;
        }

        public WhereBool ShowSpam
        {
            get => _showSpam ?? (_showSpam = new WhereBool());
            set => _showSpam = value;
        }

        public WhereBool HideClosed
        {
            get => _hideClosed ?? (_hideClosed = new WhereBool());
            set => _hideClosed = value;
        }

        public WhereBool ShowClosed
        {
            get => _showClosed ?? (_showClosed = new WhereBool());
            set => _showClosed = value;
        }

        public WhereBool IsPinned
        {
            get => _isPinned ?? (_isPinned = new WhereBool());
            set => _isPinned = value;
        }

        public WhereBool IsNotPinned
        {
            get => _isNotPinned ?? (_isNotPinned = new WhereBool());
            set => _isNotPinned = value;
        }

        public WhereBool HideDeleted
        {
            get => _hideDeleted ?? (_hideDeleted = new WhereBool());
            set => _hideDeleted = value;
        }

        public WhereBool ShowDeleted
        {
            get => _showDeleted ?? (_showDeleted = new WhereBool());
            set => _showDeleted = value;
        }

        public WhereBool IsClosed
        {
            get => _isClosed ?? (_isClosed = new WhereBool(false));
            set => _isClosed = value;
        }

        public WhereInt CreatedUserId
        {
            get => _createdUserId ?? (_createdUserId = new WhereInt());
            set => _createdUserId = value;
        }

        public WhereDate CreatedDate
        {
            get => _createdDate ?? (_createdDate = new WhereDate());
            set => _createdDate = value;
        }

        public WhereInt ModifiedUserId
        {
            get => _modifiedUserId ?? (_modifiedUserId = new WhereInt());
            set => _modifiedUserId = value;
        }

        public WhereDate ModifiedDate
        {
            get => _modifiedDate ?? (_modifiedDate = new WhereDate());
            set => _modifiedDate = value;
        }

        public WhereInt ParticipatedUserId
        {
            get => _participatedUserId ?? (_participatedUserId = new WhereInt());
            set => _participatedUserId = value;
        }

        public WhereInt FollowUserId
        {
            get => _followUserId ?? (_followUserId = new WhereInt());
            set => _followUserId = value;
        }

        public WhereInt StarUserId
        {
            get => _starUserId ?? (_starUserId = new WhereInt());
            set => _starUserId = value;
        }

        public WhereInt TotalViews
        {
            get => _totalViews ?? (_totalViews = new WhereInt());
            set => _totalViews = value;
        }

        public WhereInt TotalReplies
        {
            get => _totalReplies ?? (_totalReplies = new WhereInt());
            set => _totalReplies = value;
        }

        public WhereInt TotalParticipants
        {
            get => _totalParticipants ?? (_totalParticipants = new WhereInt());
            set => _totalParticipants = value;
        }

        public WhereInt TotalReactions
        {
            get => _totalReactions ?? (_totalReactions = new WhereInt());
            set => _totalReactions = value;
        }

        public WhereInt TotalFollows
        {
            get => _totalFollows ?? (_totalFollows = new WhereInt());
            set => _totalFollows = value;
        }

        public WhereInt TotalStars
        {
            get => _totalStars ?? (_totalStars = new WhereInt());
            set => _totalStars = value;
        }

    }

    #endregion

    #region "FeatureEntityCountQueryBuilder"

    public class FeatureEntityCountQueryBuilder<TModel> : IQueryBuilder where TModel : class
    {
      
        #region "Constructor"
            
        private readonly string _entitiesTableName;
        private readonly string _entityRepliesTableName;
        private readonly string _shellFeaturesTableName;

        private readonly FeatureEntityCountQuery<TModel> _query;

        public FeatureEntityCountQueryBuilder(FeatureEntityCountQuery<TModel> query)
        {
            _query = query;
            _entitiesTableName = GetTableNameWithPrefix("Entities");
            _entityRepliesTableName = GetTableNameWithPrefix("EntityReplies");
            _shellFeaturesTableName = GetTableNameWithPrefix("ShellFeatures");
        }

        #endregion

        #region "Implementation"

        public string BuildSqlPopulate()
        {
            var whereClause = BuildWhere();
            var sb = new StringBuilder();
            sb.Append("DECLARE @MaxRank int;")
                .Append(Environment.NewLine)
                .Append(BuildFederatedResults())
                .Append(Environment.NewLine);
            sb.Append("SELECT ")
                .Append(BuildSelect())
                .Append(" FROM ")
                .Append(BuildTables());
            if (!string.IsNullOrEmpty(whereClause))
                sb.Append(" WHERE (").Append(whereClause).Append(")");
            sb.Append(" GROUP BY f.ModuleId");
            return sb.ToString();
        }

        public string BuildSqlCount()
        {
            if (!_query.CountTotal)
                return string.Empty;
            var whereClause = BuildWhere();
            var sb = new StringBuilder();
            sb.Append("DECLARE @MaxRank int;")
                .Append(Environment.NewLine)
                .Append(BuildFederatedResults())
                .Append(Environment.NewLine);
            sb.Append("SELECT COUNT(e.Id) FROM ")
                .Append(BuildTables());
            if (!string.IsNullOrEmpty(whereClause))
                sb.Append(" WHERE (").Append(whereClause).Append(")");
            return sb.ToString();
        }

        private string _where = null;

        public string Where => _where ?? (_where = BuildWhere());

        #endregion

        #region "Private Methods"

        private string BuildSelect()
        {
            return "f.ModuleId AS ModuleId, COUNT(e.Id) AS [Count]";
        }

        private string BuildTables()
        {
            
            var sb = new StringBuilder();
            sb.Append(_entitiesTableName).Append(" e ");

            // join shell features table
            sb.Append("INNER JOIN ")
                .Append(_shellFeaturesTableName)
                .Append(" f ON e.FeatureId = f.Id ");

            // join search results if we have keywords
            if (HasKeywords())
            {
                sb.Append("INNER JOIN @results r ON r.Id = e.Id ");
            }
            
            // -----------------
            // Apply any table query adapters
            // -----------------

            _query.QueryAdapterManager?.BuildTables(_query, sb);

            return sb.ToString();

        }

        private string BuildWhere()
        {

            var sb = new StringBuilder();

            // -----------------
            // Apply any where query adapters
            // -----------------

            _query.QueryAdapterManager?.BuildWhere(_query, sb);

            // -----------------
            // Ensure we have params
            // -----------------

            if (_query.Params == null)
            {
                return string.Empty;
            }

            // -----------------
            // Id
            // -----------------

            if (_query.Params.Id.Value > -1)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.Id.Operator);
                sb.Append(_query.Params.Id.ToSqlString("e.Id"));
            }
            
            // -----------------
            // CategoryId
            // -----------------

            if (_query.Params.CategoryId.Value > -1)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.CategoryId.Operator);
                sb.Append(_query.Params.CategoryId.ToSqlString("e.CategoryId"));
            }
            
            // -----------------
            // TotalViews
            // -----------------

            if (_query.Params.TotalViews.Value > -1)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.TotalViews.Operator);
                sb.Append(_query.Params.TotalViews.ToSqlString("e.TotalViews"));
            }

            // -----------------
            // TotalReplies
            // -----------------

            if (_query.Params.TotalReplies.Value > -1)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.TotalReplies.Operator);
                sb.Append(_query.Params.TotalReplies.ToSqlString("e.TotalReplies"));
            }

            // -----------------
            // TotalParticipants
            // -----------------

            if (_query.Params.TotalParticipants.Value > -1)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.TotalParticipants.Operator);
                sb.Append(_query.Params.TotalParticipants.ToSqlString("e.TotalParticipants"));
            }

            // -----------------
            // TotalReactions
            // -----------------

            if (_query.Params.TotalReactions.Value > -1)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.TotalReactions.Operator);
                sb.Append(_query.Params.TotalReactions.ToSqlString("e.TotalReactions"));
            }

            // -----------------
            // TotalFollows
            // -----------------

            if (_query.Params.TotalFollows.Value > -1)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.TotalFollows.Operator);
                sb.Append(_query.Params.TotalFollows.ToSqlString("e.TotalFollows"));
            }

            // -----------------
            // TotalStars
            // -----------------

            if (_query.Params.TotalStars.Value > -1)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.TotalStars.Operator);
                sb.Append(_query.Params.TotalStars.ToSqlString("e.TotalStars"));
            }


            // -----------------
            // CreatedUserId
            // -----------------

            if (_query.Params.CreatedUserId.Value > -1)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.CreatedUserId.Operator);
                sb.Append(_query.Params.CreatedUserId.ToSqlString("e.CreatedUserId"));
            }

            // -----------------
            // ParticipatedUserId
            // --> Returns all entities with replies by the supplied
            // --> user and excludes entities created by the supplied user
            // -----------------

            if (_query.Params.ParticipatedUserId.Value > -1)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.LabelId.Operator);
                sb.Append(" e.Id IN (")
                    .Append("SELECT EntityId FROM ")
                    .Append(_entityRepliesTableName)
                    .Append(" WHERE (")
                    .Append(_query.Params.ParticipatedUserId.ToSqlString("CreatedUserId"))
                    .Append(") AND e.CreatedUserId != ")
                    .Append(_query.Params.ParticipatedUserId.Value)
                    .Append(")");
            }

            // -----------------
            // IsHidden 
            // -----------------

            // hide = true, show = false
            if (_query.Params.HideHidden.Value && !_query.Params.ShowHidden.Value)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.HideHidden.Operator);
                sb.Append("e.IsHidden = 0");
            }

            // show = true, hide = false
            if (_query.Params.ShowHidden.Value && !_query.Params.HideHidden.Value)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.ShowHidden.Operator);
                sb.Append("e.IsHidden = 1");
            }

            // -----------------
            // IsPrivate 
            // --> If true and we have a user id hide all private entities
            // --> except those created by the supplied user id
            // --> If true and we don't have a user id just hide all private entities
            // -----------------

            // hide = true, show = false
            if (_query.Params.HidePrivate.Value && !_query.Params.ShowPrivate.Value)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.HidePrivate.Operator);
                if (_query.Params.UserId.Value > 0)
                {
                    sb.Append("(")
                        .Append("(e.IsPrivate = 0) OR (e.IsPrivate = 1 AND e.CreatedUserId = ")
                        .Append(_query.Params.UserId.Value)
                        .Append("))");
                }
                else
                {
                    // Else just hide all private
                    sb.Append("e.IsPrivate = 0");
                }
            }

            // show = true, hide = false
            if (_query.Params.ShowPrivate.Value && !_query.Params.HidePrivate.Value)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.ShowPrivate.Operator);
                sb.Append("e.IsPrivate = 1");
            }

            // -----------------
            // IsSpam 
            // -----------------

            // hide = true, show = false
            if (_query.Params.HideSpam.Value && !_query.Params.ShowSpam.Value)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.HideSpam.Operator);
                sb.Append("e.IsSpam = 0");
            }

            // show = true, hide = false
            if (_query.Params.ShowSpam.Value && !_query.Params.HideSpam.Value)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.ShowSpam.Operator);
                sb.Append("e.IsSpam = 1");
            }

            // -----------------
            // IsDeleted 
            // -----------------

            // hide = true, show = false
            if (_query.Params.HideDeleted.Value && !_query.Params.ShowDeleted.Value)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.HideDeleted.Operator);
                sb.Append("e.IsDeleted = 0");
            }

            // show = true, hide = false
            if (_query.Params.ShowDeleted.Value && !_query.Params.HideDeleted.Value)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.ShowDeleted.Operator);
                sb.Append("e.IsDeleted = 1");
            }

            // -----------------
            // IsClosed 
            // -----------------

            // hide = true, show = false
            if (_query.Params.HideClosed.Value && !_query.Params.ShowClosed.Value)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.HideClosed.Operator);
                sb.Append("e.IsClosed = 0");
            }

            // show = true, hide = false
            if (_query.Params.ShowClosed.Value && !_query.Params.HideClosed.Value)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.ShowClosed.Operator);
                sb.Append("e.IsClosed = 1");
            }

            // -----------------
            // IsPinned 
            // -----------------

            // hide = true, show = false
            if (_query.Params.IsNotPinned.Value && !_query.Params.IsPinned.Value)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.IsNotPinned.Operator);
                sb.Append("e.IsPinned = 0");
            }

            // show = true, hide = false
            if (_query.Params.IsPinned.Value && !_query.Params.IsNotPinned.Value)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.IsPinned.Operator);
                sb.Append("e.IsPinned = 1");
            }

            return sb.ToString();

        }

        private string GetTableNameWithPrefix(string tableName)
        {
            return !string.IsNullOrEmpty(_query.Options.TablePrefix)
                ? _query.Options.TablePrefix + tableName
                : tableName;
        }
        
        // -- Search

        private string BuildFederatedResults()
        {

            // No keywords
            if (string.IsNullOrEmpty(GetKeywords()))
            {
                return string.Empty;
            }

            // Build standard SQL or full text queries
            var sb = new StringBuilder();

            // Compose federated queries
            var queries = _query.FederatedQueryManager.GetQueries(_query);

            // Create a temporary table for all our federated queries
            sb.Append("DECLARE @temp TABLE (Id int, [Rank] int); ");

            // Execute each federated query adding results to temporary table
            foreach (var query in queries)
            {
                sb.Append("INSERT INTO @temp ")
                    .Append(Environment.NewLine)
                    .Append(query)
                    .Append(Environment.NewLine);
            }

            // Build final distinct and aggregated results from federated results
            sb.Append("DECLARE @results TABLE (Id int, [Rank] int); ")
                .Append(Environment.NewLine)
                .Append("INSERT INTO @results ")
                .Append(Environment.NewLine)
                .Append("SELECT Id, SUM(Rank) FROM @temp GROUP BY Id;")
                .Append(Environment.NewLine);

            // Get max / highest rank from final results table
            sb.Append("SET @MaxRank = ")
                .Append(_query.Options.SearchType != SearchTypes.Tsql
                    ? "(SELECT TOP 1 [Rank] FROM @results ORDER BY [Rank] DESC)"
                    : "0")
                .Append(";");

            return sb.ToString();

        }

        private bool HasKeywords()
        {
            return !string.IsNullOrEmpty(GetKeywords());
        }

        private string GetKeywords()
        {

            if (string.IsNullOrEmpty(_query.Params.Keywords.Value))
            {
                return string.Empty;
            }

            return _query.Params.Keywords.Value;

        }

        #endregion

    }

    #endregion

}
