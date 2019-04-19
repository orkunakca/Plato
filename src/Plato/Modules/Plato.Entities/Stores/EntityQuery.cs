﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Plato.Internal.Data.Abstractions;
using Plato.Internal.Stores.Abstractions;

namespace Plato.Entities.Stores
{

    #region "EntityQuery"

    public class EntityQuery<TModel> : DefaultQuery<TModel> where TModel : class
    {

        public IFederatedQueryManager<TModel> FederatedQueryManager { get; set; }

        public EntityQueryParams Params { get; set; }

        private readonly IStore<TModel> _store;
        
        public EntityQueryBuilder<TModel> Builder { get; private set; }
        
        public EntityQuery(IStore<TModel> store)
        {
            _store = store;
        }

        public override IQuery<TModel> Select<T>(Action<T> configure)
        {
            var defaultParams = new T();
            configure(defaultParams);
            Params = (EntityQueryParams)Convert.ChangeType(defaultParams, typeof(EntityQueryParams));
            return this;
        }
        
        public override async Task<IPagedResults<TModel>> ToList()
        {

            Builder = new EntityQueryBuilder<TModel>(this);
            var populateSql = Builder.BuildSqlPopulate();
            var countSql = Builder.BuildSqlCount();

            return await _store.SelectAsync(
                PageIndex,
                PageSize,
                populateSql,
                countSql,
                Params.Keywords.Value);
        }
        
     
    }

    #endregion

    #region "EntityQueryParams"

    public class EntityQueryParams
    {

        private WhereInt _id;
        private WhereInt _userId;
        private WhereInt _featureId;
        private WhereInt _categoryId;
        private WhereInt _roleId;
        private WhereInt _labelId;
        private WhereInt _tagId;
        private WhereString _keywords;
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
            get => _userId ?? (_id = new WhereInt());
            set => _userId = value;
        }
        
        public WhereInt FeatureId
        {
            get => _featureId ?? (_featureId = new WhereInt());
            set => _featureId = value;
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

    #region "EntityQueryBuilder"

    public class EntityQueryBuilder<TModel> : IQueryBuilder where TModel : class
    {

        /*

            DECLARE @MaxRank int;
            DECLARE @temp TABLE (Id int, [Rank] int); 

            -- Provided via federated search
            INSERT INTO @temp 
                SELECT i.[Key], i.[Rank] FROM plato_Entities e INNER JOIN CONTAINSTABLE(plato_Entities, *, 'FORMSOF(INFLECTIONAL, introduction)') AS i ON i.[Key] = e.Id WHERE (e.Id IN (IsNull(i.[Key], 0)));

            -- Provided via federated search
            INSERT INTO @temp 
                SELECT er.EntityId, SUM(i.[Rank]) AS [Rank] FROM plato_EntityReplies er INNER JOIN CONTAINSTABLE(plato_EntityReplies, *, 'FORMSOF(INFLECTIONAL, introduction)') i ON i.[Key] = er.Id INNER JOIN plato_Entities e ON e.Id = er.EntityId WHERE (er.Id IN (IsNull(i.[Key], 0)))GROUP BY er.EntityId, i.[Rank];

            DECLARE @results TABLE (Id int, [Rank] int); 
            INSERT INTO @results 
                SELECT Id, SUM(Rank) FROM @temp GROUP BY Id;

            SET @MaxRank = (SELECT TOP 1 [Rank] FROM @results ORDER BY [Rank] DESC);

            SELECT e.*, 
                f.ModuleId, 
                c.UserName AS CreatedUserName, 
                c.DisplayName AS CreatedDisplayName,
                c.Alias AS CreatedAlias,
                c.PhotoUrl AS CreatedPhotoUrl,
                c.PhotoColor AS CreatedPhotoColor,
                c.SignatureHtml AS CreatedSignatureHtml,
                m.UserName AS ModifiedUserName, 
                m.DisplayName AS ModifiedDisplayName,
                m.Alias AS ModifiedAlias, 
                m.PhotoUrl AS ModifiedPhotoUrl, 
                m.PhotoColor AS ModifiedPhotoColor, 
                m.SignatureHtml AS ModifiedSignatureHtml,
                l.UserName AS LastReplyUserName, 
                l.DisplayName AS LastReplyDisplayName,
                l.Alias AS LastReplyAlias, 
                l.PhotoUrl AS LastReplyPhotoUrl, 
                l.PhotoColor AS LastReplyPhotoColor, 
                l.SignatureHtml AS LastReplySignatureHtml, 
                r.[Rank] AS [Rank], 
                @MaxRank AS MaxRank 
            FROM plato_Entities e 
                INNER JOIN plato_ShellFeatures f ON e.FeatureId = f.Id 
                INNER JOIN @results r ON r.Id = e.Id -- joined on federated search results
                LEFT OUTER JOIN plato_Users c ON e.CreatedUserId = c.Id 
                LEFT OUTER JOIN plato_Users m ON e.ModifiedUserId = m.Id 
                LEFT OUTER JOIN plato_Users l ON e.LastReplyUserId = l.Id 
            ORDER BY 
	            e.IsPinned DESC, 
	            [Rank] DESC 
            OFFSET 0 ROWS FETCH NEXT 20 ROWS ONLY;
            
        */

        #region "Constructor"

        private readonly string _shellFeaturesTableName;
        private readonly string _entitiesTableName;
        private readonly string _usersTableName;
        private readonly string _userRolesTableName;
        private readonly string _rolesTableName;
        private readonly string _entityRepliesTableName;
        private readonly string _entityLabelsTableName;
        private readonly string _entityTagsTableName;
        private readonly string _followsTableName;
        private readonly string _starsTableName;
        private readonly string _categoryRolesTableName;

        private readonly EntityQuery<TModel> _query;
  
        public EntityQueryBuilder(EntityQuery<TModel> query)
        {
            _query = query;
            _shellFeaturesTableName = GetTableNameWithPrefix("ShellFeatures");
            _entitiesTableName = GetTableNameWithPrefix("Entities");
            _usersTableName = GetTableNameWithPrefix("Users");
            _rolesTableName = GetTableNameWithPrefix("Roles");
            _userRolesTableName = GetTableNameWithPrefix("UserRoles");
            _entityRepliesTableName = GetTableNameWithPrefix("EntityReplies");
            _entityLabelsTableName = GetTableNameWithPrefix("EntityLabels");
            _entityTagsTableName = GetTableNameWithPrefix("EntityTags");
            _followsTableName = GetTableNameWithPrefix("Follows");
            _starsTableName = GetTableNameWithPrefix("Stars");
            _categoryRolesTableName = GetTableNameWithPrefix("CategoryRoles");

        }

        #endregion

        #region "Implementation"
        
        public string BuildSqlPopulate()
        {
            
            var whereClause = BuildWhere();
            var orderBy = BuildOrderBy();

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
            sb.Append(" ORDER BY ")
                .Append(!string.IsNullOrEmpty(orderBy)
                    ? orderBy
                    : "Id ASC");
            sb.Append(" OFFSET @RowIndex ROWS FETCH NEXT @PageSize ROWS ONLY;");
            return sb.ToString();
        }

        public string BuildSqlCount()
        {
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

        string BuildSelect()
        {
            var sb = new StringBuilder();
            sb
                .Append("e.*, f.ModuleId, ")
                .Append("c.UserName AS CreatedUserName, ")
                .Append("c.DisplayName AS CreatedDisplayName,")
                .Append("c.Alias AS CreatedAlias,")
                .Append("c.PhotoUrl AS CreatedPhotoUrl,")
                .Append("c.PhotoColor AS CreatedPhotoColor,")
                .Append("c.SignatureHtml AS CreatedSignatureHtml,")
                .Append("m.UserName AS ModifiedUserName, ")
                .Append("m.DisplayName AS ModifiedDisplayName,")
                .Append("m.Alias AS ModifiedAlias, ")
                .Append("m.PhotoUrl AS ModifiedPhotoUrl, ")
                .Append("m.PhotoColor AS ModifiedPhotoColor, ")
                .Append("m.SignatureHtml AS ModifiedSignatureHtml,")
                .Append("l.UserName AS LastReplyUserName, ")
                .Append("l.DisplayName AS LastReplyDisplayName,")
                .Append("l.Alias AS LastReplyAlias, ")
                .Append("l.PhotoUrl AS LastReplyPhotoUrl, ")
                .Append("l.PhotoColor AS LastReplyPhotoColor, ")
                .Append("l.SignatureHtml AS LastReplySignatureHtml, ")
                .Append(HasKeywords() ? "r.[Rank] AS [Rank], " : "0 AS [Rank],")
                .Append("@MaxRank AS MaxRank");
            
            return sb.ToString();

        }
        
        string BuildTables()
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

            // join created user
            sb.Append("LEFT OUTER JOIN ")
                .Append(_usersTableName)
                .Append(" c ON e.CreatedUserId = c.Id ");

            // join last modified user
            sb.Append("LEFT OUTER JOIN ")
                .Append(_usersTableName)
                .Append(" m ON e.ModifiedUserId = m.Id ");

            // join last reply user
            sb.Append("LEFT OUTER JOIN ")
                .Append(_usersTableName)
                .Append(" l ON e.LastReplyUserId = l.Id ");

            return sb.ToString();

        }

        string BuildWhere()
        {

            var sb = new StringBuilder();

            // Id
            if (_query.Params.Id.Value > -1)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.Id.Operator);
                sb.Append(_query.Params.Id.ToSqlString("e.Id"));
            }

            // RoleId
            if (_query.Params.RoleId.Value > -1)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.RoleId.Operator);
                sb.Append("(e.CategoryId IN (")
                    .Append("SELECT cr.CategoryId FROM ")
                    .Append(_categoryRolesTableName)
                    .Append(" AS cr WITH (nolock) WHERE ")
                    .Append(_query.Params.RoleId.ToSqlString("RoleId"))
                    .Append("))");
            }

            // FeatureId
            if (_query.Params.FeatureId.Value > -1)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.FeatureId.Operator);
                sb.Append(_query.Params.FeatureId.ToSqlString("e.FeatureId"));
            }

            // CategoryId
            if (_query.Params.CategoryId.Value > -1)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.CategoryId.Operator);
                sb.Append(_query.Params.CategoryId.ToSqlString("e.CategoryId"));
            }

            // LabelId
            // --> Only available if the Labels feature is enabled
            if (_query.Params.LabelId.Value > -1)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.LabelId.Operator);
                sb.Append(" e.Id IN (")
                    .Append("SELECT EntityId FROM ")
                    .Append(_entityLabelsTableName)
                    .Append(" WHERE (")
                    .Append(_query.Params.LabelId.ToSqlString("LabelId"))
                    .Append("))");
            }

            // TagId
            // --> Only available if the Tags feature is enabled
            if (_query.Params.TagId.Value > -1)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.TagId.Operator);
                sb.Append(" e.Id IN (")
                    .Append("SELECT EntityId FROM ")
                    .Append(_entityTagsTableName)
                    .Append(" et WHERE (")
                    .Append(_query.Params.TagId.ToSqlString("et.TagId"))
                    .Append("))");
            }

            // FollowUserId
            // --> Only available if the follow feature is enabled
            if (_query.Params.FollowUserId.Value > 0)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.FollowUserId.Operator);
                sb.Append(" e.Id IN (")
                    .Append("SELECT ThingId FROM ")
                    .Append(_followsTableName)
                    .Append(" f WHERE (")
                    .Append(_query.Params.FollowUserId.ToSqlString("f.CreatedUserId"))
                    .Append("))");
            }

            // StarUserId
            // --> Only available if the follow feature is enabled
            if (_query.Params.StarUserId.Value > 0)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.StarUserId.Operator);
                sb.Append(" e.Id IN (")
                    .Append("SELECT ThingId FROM ")
                    .Append(_starsTableName)
                    .Append(" s WHERE (")
                    .Append(_query.Params.StarUserId.ToSqlString("s.CreatedUserId"))
                    .Append("))");
            }

            // TotalViews
            if (_query.Params.TotalViews.Value > -1)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.TotalViews.Operator);
                sb.Append(_query.Params.TotalViews.ToSqlString("e.TotalViews"));
            }

            // TotalReplies
            if (_query.Params.TotalReplies.Value > -1)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.TotalReplies.Operator);
                sb.Append(_query.Params.TotalReplies.ToSqlString("e.TotalReplies"));
            }

            // TotalParticipants
            if (_query.Params.TotalParticipants.Value > -1)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.TotalParticipants.Operator);
                sb.Append(_query.Params.TotalParticipants.ToSqlString("e.TotalParticipants"));
            }

            // TotalReactions
            if (_query.Params.TotalReactions.Value > -1)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.TotalReactions.Operator);
                sb.Append(_query.Params.TotalReactions.ToSqlString("e.TotalReactions"));
            }

            // TotalFollows
            if (_query.Params.TotalFollows.Value > -1)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.TotalFollows.Operator);
                sb.Append(_query.Params.TotalFollows.ToSqlString("e.TotalFollows"));
            }

            // TotalStars
            if (_query.Params.TotalStars.Value > -1)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.TotalStars.Operator);
                sb.Append(_query.Params.TotalStars.ToSqlString("e.TotalStars"));
            }


            // CreatedUserId
            if (_query.Params.CreatedUserId.Value > -1)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.CreatedUserId.Operator);
                sb.Append(_query.Params.CreatedUserId.ToSqlString("e.CreatedUserId"));
            }

            // ParticipatedUserId
            // --> Returns all entities with replies by the supplied
            // --> user and excludes entities created by the supplied user
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
            // private 
            // -----------------

            // hide = true, show = false
            if (_query.Params.HidePrivate.Value && !_query.Params.ShowPrivate.Value)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.HidePrivate.Operator);
                sb.Append("e.IsPrivate = 0");
            }

            // show = true, hide = false
            if (_query.Params.ShowPrivate.Value && !_query.Params.HidePrivate.Value)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(_query.Params.ShowPrivate.Operator);
                sb.Append("e.IsPrivate = 1");
            }

            // -----------------
            // spam 
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
            // deleted 
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
            // closed 
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
            // pinned 
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

            // -----------------
            // Keywords 
            // -----------------

            //if (!string.IsNullOrEmpty(_query.Params.Keywords.Value))
            //{
            //    if (!string.IsNullOrEmpty(sb.ToString()))
            //        sb.Append(" AND ");

            //    sb.Append("(");

            //    if (_query.Options.SearchType == SearchTypes.Tsql)
            //    {

            //        // Entities

            //        sb.Append("(")
            //            .Append(_query.Params.Keywords.ToSqlString("Title", "Keywords"))
            //            .Append(" OR ")
            //            .Append(_query.Params.Keywords.ToSqlString("Message", "Keywords"))
            //            .Append(")");

            //        sb.Append(" OR ");

            //        // Entity Replies

            //        sb.Append("(e.Id IN (SELECT EntityId FROM ")
            //            .Append(_entityRepliesTableName)
            //            .Append(" WHERE (")
            //            .Append(_query.Params.Keywords.ToSqlString("Message", "Keywords"))
            //            .Append(")))");

            //    }
            //    else
            //    {
            //        sb.Append("e.Id IN (SELECT Id FROM @results)");
            //    }

            //    sb.Append(")");

            //}

            return sb.ToString();

        }


        string GetTableNameWithPrefix(string tableName)
        {
            return !string.IsNullOrEmpty(_query.Options.TablePrefix)
                ? _query.Options.TablePrefix + tableName
                : tableName;
        }
        
        private string BuildOrderBy()
        {
            if (_query.SortColumns.Count == 0) return null;
            var sb = new StringBuilder();
            var i = 0;
            foreach (var sortColumn in GetSafeSortColumns())
            {
                sb.Append(sortColumn.Key);
                if (sortColumn.Value != OrderBy.Asc)
                    sb.Append(" DESC");
                if (i < _query.SortColumns.Count - 1)
                    sb.Append(", ");
                i += 1;
            }
            return sb.ToString();
        }

        IDictionary<string, OrderBy> GetSafeSortColumns()
        {
            var output = new Dictionary<string, OrderBy>();
            foreach (var sortColumn in _query.SortColumns)
            {
                var columnName = GetSortColumn(sortColumn.Key);
                if (String.IsNullOrEmpty(columnName))
                {
                    throw new Exception($"No sort column could be found for the supplied key of '{sortColumn.Key}'");
                }
                output.Add(columnName, sortColumn.Value);

            }

            return output;
        }
        
        string GetSortColumn(string columnName)
        {

            if (String.IsNullOrEmpty(columnName))
            {
                return string.Empty;
            }

            switch (columnName.ToLowerInvariant())
            {
                case "id":
                    return "e.Id";
                case "title":
                    return "e.Title";
                case "message":
                    return "e.[Message]";
                case "replies":
                    return "e.TotalReplies";
                case "totalreplies":
                    return "e.TotalReplies";
                case "participants":
                    return "e.TotalParticipants";
                case "totalparticipants":
                    return "e.TotalParticipants";
                case "views":
                    return "e.TotalViews";
                case "totalviews":
                    return "e.TotalViews";
                case "follows":
                    return "e.TotalFollows";
                case "totalfollows":
                    return "e.TotalFollows";
                case "stars":
                    return "e.TotalStars";
                case "totalstars":
                    return "e.TotalStars";
                case "reactions":
                    return "e.TotalReactions";
                case "totalreactions":
                    return "e.TotalReactions";
                case "answers":
                    return "e.TotalAnswers";
                case "totalanswers":
                    return "e.TotalAnswers";
                case "pinned":
                    return "e.IsPinned";
                case "ispinned":
                    return "e.IsPinned";
                case "deleted":
                    return "e.IsDeleted";
                case "isdeleted":
                    return "e.IsDeleted";
                case "private":
                    return "e.IsPrivate";
                case "isprivate":
                    return "e.IsPrivate";
                case "spam":
                    return "e.IsSpam";
                case "isspam":
                    return "e.IsSpam";
                case "sortorder":
                    return "e.SortOrder";
                case "created":
                    return "e.CreatedDate";
                case "createddate":
                    return "e.CreatedDate";
                case "modified":
                    return "e.ModifiedDate";
                case "modifieddate":
                    return "e.ModifiedDate";
                case "lastreply":
                    return "e.LastReplyDate";
                case "lastreplydate":
                    return "e.LastReplyDate";
                case "rank":
                    return "[Rank]";
            }

            return string.Empty;

        }

        // -- Search
        
        string BuildFederatedResults()
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

        //List<string> BuildSqlQueries()
        //{

        //    var whereClause = BuildWhere();

        //    // Entities
        //    // ----------------------

        //    var q1 = new StringBuilder();
        //    q1.Append("SELECT e.Id, 0 AS [Rank] FROM ")
        //        .Append(_entitiesTableName)
        //        .Append(" e WHERE (");
        //    if (!string.IsNullOrEmpty(whereClause))
        //    {
        //        q1.Append("(").Append(whereClause).Append(") AND ");
        //    }
        //    q1.Append("(")
        //        .Append(_query.Params.Keywords.ToSqlString("e.Title", "Keywords"))
        //        .Append(" OR ")
        //        .Append(_query.Params.Keywords.ToSqlString("e.Message", "Keywords"))
        //        .Append("));");

        //    // Entity Replies
        //    // ----------------------

        //    var q2 = new StringBuilder();
        //    q2.Append("SELECT er.EntityId, 0 AS [Rank] FROM ")
        //        .Append(_entityRepliesTableName)
        //        .Append(" er INNER JOIN ")
        //        .Append(_entitiesTableName)
        //        .Append(" e ON e.Id = er.EntityId ")
        //        .Append(" WHERE (");
        //    if (!string.IsNullOrEmpty(whereClause))
        //    {
        //        q2.Append("(").Append(whereClause).Append(") AND ");
        //    }
        //    q2.Append("(")
        //        .Append(_query.Params.Keywords.ToSqlString("er.Message", "Keywords"))
        //        .Append(")) GROUP BY er.EntityId");

        //    // Return queries
        //    return new List<string>()
        //    {
        //        q1.ToString(),
        //        q2.ToString()
        //    };

        //}

        //List<string> BuildFullTextQueries()
        //{

        //    var whereClause = BuildWhere();

        //    // Entities
        //    // ----------------------

        //    var q1 = new StringBuilder();
        //    q1
        //        .Append("SELECT i.[Key], i.[Rank] ")
        //        .Append("FROM ")
        //        .Append(_entitiesTableName)
        //        .Append(" e ")
        //        .Append("INNER JOIN ")
        //        .Append(_query.Options.SearchType.ToString().ToUpper())
        //        .Append("(")
        //        .Append(_entitiesTableName)
        //        .Append(", *, '").Append(GetKeywords()).Append("'");
        //    if (_query.Options.MaxResults > 0)
        //        q1.Append(", ").Append(_query.Options.MaxResults.ToString());
        //    q1.Append(") AS i ON i.[Key] = e.Id WHERE ");
        //    if (!string.IsNullOrEmpty(whereClause))
        //        q1.Append("(").Append(whereClause).Append(") AND ");
        //    q1.Append("(e.Id IN (IsNull(i.[Key], 0)));");

        //    // Entity replies
        //    // ----------------------

        //    var q2 = new StringBuilder();
        //    q2
        //        .Append("SELECT er.EntityId, SUM(i.[Rank]) AS [Rank] ")
        //        .Append("FROM ")
        //        .Append(_entityRepliesTableName)
        //        .Append(" er ")

        //        .Append("INNER JOIN ")
        //        .Append(_query.Options.SearchType.ToString().ToUpper())
        //        .Append("(")
        //        .Append(_entityRepliesTableName)
        //        .Append(", *, '").Append(GetKeywords()).Append("'");
        //    if (_query.Options.MaxResults > 0)
        //        q2.Append(", ").Append(_query.Options.MaxResults.ToString());
        //    q2.Append(") i ON i.[Key] = er.Id ")
        //        .Append("INNER JOIN ")
        //        .Append(_entitiesTableName)
        //        .Append(" e ON e.Id = er.EntityId ")

        //        .Append("WHERE ");
        //    if (!string.IsNullOrEmpty(whereClause))
        //        q1.Append("(").Append(whereClause).Append(") AND ");
        //    q2.Append("(er.Id IN (IsNull(i.[Key], 0)))")
        //        .Append("GROUP BY er.EntityId, i.[Rank];");

        //    // Return queries
        //    return new List<string>()
        //    {
        //        q1.ToString(),
        //        q2.ToString()
        //    };

        //}

        bool HasKeywords()
        {
            return !string.IsNullOrEmpty(GetKeywords());
        }

        string GetKeywords()
        {

            if (string.IsNullOrEmpty(_query.Params.Keywords.Value))
            {
                return string.Empty;
            }

            return _query.Params.Keywords.Value;

            //No need to modify keywords
            //if (_query.Options.SearchType == SearchTypes.Tsql)
            //{
            //    return _query.Params.Keywords.Value;
            //}

            // Parse keywords into valid full text query syntax
            //var fullTextSearchQuery =
            //    _query.Options.FullTextQueryParser.ToFullTextSearchQuery(
            //        _query.Params.Keywords.Value);

            //// Ensure parse was successful
            //if (!String.IsNullOrEmpty(fullTextSearchQuery))
            //{
            //    return fullTextSearchQuery.Replace("'", "''");
            //}

            return string.Empty;

        }


        #endregion

    }

    #endregion

}
