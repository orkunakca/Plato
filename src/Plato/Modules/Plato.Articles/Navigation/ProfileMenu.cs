﻿using Microsoft.Extensions.Localization;
using System;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Plato.Internal.Navigation;
using Plato.Internal.Navigation.Abstractions;

namespace Plato.Articles.Navigation
{
    public class ProfileMenu : INavigationProvider
    {

        private readonly IActionContextAccessor _actionContextAccessor;

        public IStringLocalizer T { get; set; }
        
        public ProfileMenu(
            IStringLocalizer localizer, 
            IActionContextAccessor actionContextAccessor)
        {
            T = localizer;
            _actionContextAccessor = actionContextAccessor;
        }
        
        public void BuildNavigation(string name, INavigationBuilder builder)
        {

            if (!String.Equals(name, "profile", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var context = _actionContextAccessor.ActionContext;
            object id = context.RouteData.Values["id"], 
                alias = context.RouteData.Values["alias"];

            builder
                .Add(T["Discuss"], 2, discuss => discuss
                    .Add(T["Topics"], 1, topics => topics
                        .Action("Index", "Profile", "Plato.Discuss", new RouteValueDictionary()
                        {
                            ["id"] = id?.ToString(),
                            ["alias"] = alias?.ToString()
                        })
                        //.Permission(Permissions.ManageRoles)
                        .LocalNav()
                    ).Add(T["Favourites"], 999, favourites => favourites
                        .Action("Channels", "Admin", "Plato.Discuss")
                        //.Permission(Permissions.ManageRoles)
                        .LocalNav()
                    ));
        }
    }

}