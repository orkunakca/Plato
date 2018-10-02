﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Plato.Internal.Abstractions.Extensions;
using Plato.Internal.Data.Abstractions;
using Plato.Internal.Hosting.Abstractions;
using Plato.Internal.Models.Users;
using Plato.Internal.Stores.Abstractions.Users;
using Plato.Internal.Stores.Users;

namespace Plato.WebApi.Controllers
{

    public class Results
    {

        public int Page { get; set; }

        public int PageSize { get; set; }

        public int Total { get; set; }

        public int TotalPages { get; set; }
        
        public IEnumerable<Result> Data { get; set; }

    }

    public class Result
    {

        public int Id { get; set; }

        public string DisplayName { get; set; }

        public string UserName { get; set; }

        public string Url { get; set; }

        public int Rank { get; set; }

    }

    public class UsersController : BaseWebApiController
    {
        
        private readonly IPlatoUserStore<User> _ploatUserStore;
        private readonly IContextFacade _contextFacade;
   
        public UsersController(
            IPlatoUserStore<User> platoUserStore,
            IUrlHelperFactory urlHelperFactory,
            IContextFacade contextFacade)
        {
            _ploatUserStore = platoUserStore;
            _contextFacade = contextFacade;
        }
        
        [HttpGet]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> Get(
            int page = 1,
            int size = 10,
            string keywords = "",
            string sort = "LastLoginDate",
            OrderBy order = OrderBy.Desc)
        {

            var users = await GetUsers(
                page,
                size,
                keywords,
                sort,
                order);
            
            PagedResults<Result> results = null;
            if (users != null)
            {
                results = new PagedResults<Result>
                {
                    Total = users.Total
                };
                
                var baseUrl = await _contextFacade.GetBaseUrlAsync();
                foreach (var user in users.Data)
                {

                    var profileUrl = baseUrl + _contextFacade.GetRouteUrl(new RouteValueDictionary()
                    {
                        ["Area"] = "Plato.Users",
                        ["Controller"] = "Home",
                        ["Action"] = "Display",
                        ["Id"] = user.Id,
                        ["Alias"] = user.Alias
                    });

                    results.Data.Add(new Result()
                    {
                        Id = user.Id,
                        DisplayName = user.DisplayName,
                        UserName = user.UserName,
                        Url = profileUrl,
                        Rank = 0
                    });
                }
            }

            Results output = null;
            if (results != null)
            {
                var totalPages = results.Total.ToSafeCeilingDivision(size);
                output = new Results()
                {
                    Page = page,
                    PageSize = size,
                    Total = results.Total,
                    TotalPages = totalPages,
                    Data = results.Data
                };
            }

            return output != null
                ? base.Result(output)
                : base.NoResults();

        }
        
        async Task<IPagedResults<User>> GetUsers(
            int page,
            int pageSize,
            string username,
            string sortBy,
            OrderBy sortOrder)
        {

            return await _ploatUserStore.QueryAsync()
                .Take(page, pageSize)
                .Select<UserQueryParams>(q =>
                {
                    if (!String.IsNullOrEmpty(username))
                    {
                        q.UserName.StartsWith(username).Or();
                        q.Email.StartsWith(username).Or();
                    }
                })
                .OrderBy(sortBy, sortOrder)
                .ToList();

        }

    }

}