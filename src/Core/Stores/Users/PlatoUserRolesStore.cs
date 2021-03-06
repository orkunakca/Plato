﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PlatoCore.Cache.Abstractions;
using PlatoCore.Data.Abstractions;
using PlatoCore.Models.Users;
using PlatoCore.Repositories.Users;
using PlatoCore.Stores.Abstractions.Roles;
using PlatoCore.Stores.Abstractions.Users;
using PlatoCore.Stores.Roles;

namespace PlatoCore.Stores.Users
{
    public class PlatoUserRolesStore : IPlatoUserRoleStore<UserRole>
    {
        private readonly IUserRolesRepository<UserRole> _userRolesRepository;
        private readonly ILogger<PlatoUserRolesStore> _logger;
        private readonly ICacheManager _cacheManager;
        private readonly IDbQueryConfiguration _dbQuery;

        public PlatoUserRolesStore(
            IPlatoRoleStore platoRoleStore,
            IUserRolesRepository<UserRole> userRolesRepository,
            ILogger<PlatoUserRolesStore> logger,
            ICacheManager cacheManager, 
            IDbQueryConfiguration dbQuery)
        {
            _userRolesRepository = userRolesRepository;
            _cacheManager = cacheManager;
            _logger = logger;
            _dbQuery = dbQuery;
        }

        public async Task<IEnumerable<UserRole>> AddUserRolesAsync(int userId, IEnumerable<string> roleNames)
        {
            var result = await _userRolesRepository.InsertUserRolesAsync(userId, roleNames);
            if (result != null)
            {
                _cacheManager.CancelTokens(this.GetType());
                // Expire GetRolesByUserIdAsync in PlatoRoleStore
                _cacheManager.CancelTokens(typeof(PlatoRoleStore), PlatoRoleStore.UserId, userId);
            }

            return result;

        }

        public async Task<IEnumerable<UserRole>> AddUserRolesAsync(int userId, IEnumerable<int> roleIds)
        {
            var result = await _userRolesRepository.InsertUserRolesAsync(userId, roleIds);
            if (result != null)
            {
                _cacheManager.CancelTokens(this.GetType());
                // Expire GetRolesByUserIdAsync in PlatoRoleStore
                _cacheManager.CancelTokens(typeof(PlatoRoleStore), PlatoRoleStore.UserId, userId);
            }

            return result;

        }

        public async Task<UserRole> CreateAsync(UserRole model)
        {

            var userRole = await _userRolesRepository.InsertUpdateAsync(model);
            if (userRole != null)
            {
                _cacheManager.CancelTokens(this.GetType());
                // Expire GetRolesByUserIdAsync in PlatoRoleStore
                _cacheManager.CancelTokens(typeof(PlatoRoleStore), PlatoRoleStore.UserId, userRole.UserId);
            }

            return userRole;

        }
        
        public async Task<UserRole> UpdateAsync(UserRole model)
        {
            var userRole = await _userRolesRepository.InsertUpdateAsync(model);
            if (userRole != null)
            {
                _cacheManager.CancelTokens(this.GetType());
                // Expire GetRolesByUserIdAsync in PlatoRoleStore
                _cacheManager.CancelTokens(typeof(PlatoRoleStore), PlatoRoleStore.UserId, userRole.UserId);
            }

            return userRole;

        }

        public async Task<bool> DeleteAsync(UserRole model)
        {

            // Ensure the entry exists
            var userRole = await GetByIdAsync(model.Id);
            if (userRole == null)
            {
                return false;
            }

            // Delete & expire caches
            var success = await _userRolesRepository.DeleteAsync(model.Id);
            if (success)
            {
                _cacheManager.CancelTokens(this.GetType());
                // Expire GetRolesByUserIdAsync in PlatoRoleStore
                _cacheManager.CancelTokens(typeof(PlatoRoleStore), PlatoRoleStore.UserId, userRole.UserId);
            }
            return success;

        }

        public async Task<bool> DeleteUserRole(int userId, int roleId)
        {
            var success = await _userRolesRepository.DeleteUserRole(userId, roleId);
            if (success)
            {
                _cacheManager.CancelTokens(this.GetType());
                // Expire GetRolesByUserIdAsync in PlatoRoleStore
                _cacheManager.CancelTokens(typeof(PlatoRoleStore), PlatoRoleStore.UserId, userId);
            }

            return success;
        }
        
        public async Task<bool> DeleteUserRolesAsync(int userId)
        {
            var success = await _userRolesRepository.DeleteUserRolesAsync(userId);
            if (success)
            {
                _cacheManager.CancelTokens(this.GetType());
                // Expire GetRolesByUserIdAsync in PlatoRoleStore
                _cacheManager.CancelTokens(typeof(PlatoRoleStore), PlatoRoleStore.UserId, userId);
            }

            return success;

        }

        public async Task<UserRole> GetByIdAsync(int id)
        {
            var token = _cacheManager.GetOrCreateToken(this.GetType(), id);
            return await _cacheManager.GetOrCreateAsync(token,
                async (cacheEntry) => await _userRolesRepository.SelectByIdAsync(id));
        }

        public async Task<IEnumerable<UserRole>> GetUserRoles(int userId)
        {
            return await _userRolesRepository.SelectUserRolesByUserId(userId);
        }
        
        public IQuery<UserRole> QueryAsync()
        {
            var query = new UserRoleQuery(this);
            return _dbQuery.ConfigureQuery<UserRole>(query); ;
        }
        
        public async Task<IPagedResults<UserRole>> SelectAsync(IDbDataParameter[] dbParams)
        {
            var token = _cacheManager.GetOrCreateToken(this.GetType(), dbParams.Select(p => p.Value).ToArray());
            return await _cacheManager.GetOrCreateAsync(token, async (cacheEntry) => await _userRolesRepository.SelectAsync(dbParams));
        }
        
        public void CancelTokens(UserRole model)
        {
            throw new NotImplementedException();
        }

    }

}