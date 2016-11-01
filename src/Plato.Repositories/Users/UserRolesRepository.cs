﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Plato.Abstractions.Collections;
using Plato.Abstractions.Extensions;
using Plato.Data;
using Plato.Models.Roles;
using Plato.Models.Users;
using Plato.Repositories.Roles;

namespace Plato.Repositories.Users
{
    public class UserRolesRepository : IUserRolesRepository<UserRole>
    {
        #region "Constructor"

        public UserRolesRepository(
            IDbContext dbContext,
            IRoleRepository<Role> rolesRepository,
            ILogger<UserSecretRepository> logger)
        {
            _dbContext = dbContext;
            _rolesRepository = rolesRepository;
            _logger = logger;
        }

        #endregion

        #region "Private Variables"

        private readonly IDbContext _dbContext;
        private readonly IRoleRepository<Role> _rolesRepository;
        private readonly ILogger<UserSecretRepository> _logger;

        #endregion

        #region "Implementation"

        public Task<UserRole> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<UserRole> InsertUpdateAsync(UserRole userRole)
        {
            var id = 0;
            id = await InsertUpdateInternal(
                userRole.Id,
                userRole.UserId,
                userRole.RoleId,
                userRole.CreatedDate,
                userRole.CreatedUserId,
                userRole.ModifiedDate,
                userRole.ModifiedUserId,
                userRole.ConcurrencyStamp);

            if (id > 0)
                return await SelectByIdAsync(id);

            return null;
        }

        public Task<UserRole> SelectByIdAsync(int id)
        {
            throw new NotImplementedException();
        }


        public Task<IEnumerable<UserRole>> InsertUserRoles(int userId, IEnumerable<string> roleNames)
        {
            //var roles = _rolesRepository.SelectByNameAsync()
            //var userRoles = new List<UserRole>();
            //foreach (var name in roleNames)
            //{
            //    var role = 

            //}
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserRole>> InsertUserRoles(int userId, IEnumerable<int> roleIds)
        {
            foreach (var id in roleIds)
            {
            }
            throw new NotImplementedException();
        }


        public bool DeletetUserRoles(int userId)
        {
            throw new NotImplementedException();
        }

        public bool DeletetUserRole(int userId, string roleName)
        {
            throw new NotImplementedException();
        }

        public bool DeletetUserRole(int userId, int roleId)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region "Private Methods"

        private async Task<int> InsertUpdateInternal(
            int id,
            int userId,
            int roleId,
            DateTime? createdDate,
            int createdUserId,
            DateTime? modifiedDate,
            int modifiedUserId,
            string concurrencyStamp)
        {
            using (var context = _dbContext)
            {
                return await context.ExecuteScalarAsync<int>(
                    CommandType.StoredProcedure,
                    "plato_sp_InsertUpdateUserRole",
                    id,
                    userId,
                    roleId,
                    createdDate,
                    createdUserId,
                    modifiedDate,
                    modifiedUserId,
                    concurrencyStamp.ToEmptyIfNull().TrimToSize(50));
            }
        }


        public Task<IPagedResults<TModel>> SelectAsync<TModel>(params object[] inputParams) where TModel : class
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}