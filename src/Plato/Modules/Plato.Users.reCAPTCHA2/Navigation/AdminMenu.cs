﻿using Microsoft.Extensions.Localization;
using System;
using Plato.Internal.Navigation;

namespace Plato.Users.reCAPTCHA2.Navigation
{
    public class AdminMenu : INavigationProvider
    {
        public AdminMenu(IStringLocalizer<AdminMenu> localizer)
        {
            T = localizer;
        }

        public IStringLocalizer T { get; set; }

        public void BuildNavigation(string name, NavigationBuilder builder)
        {
            if (!String.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            builder
                .Add(T["SPAM"], int.MaxValue - 1, configuration => configuration
                    .IconCss("fal fa-exclamation-triangle")
                    .Add(T["reCAPTCHA2"], 2, installed => installed
                        .Action("Index", "Admin", "Plato.Users.reCAPTCHA2")
                        //.Permission(Permissions.ManageUsers)
                        .LocalNav()
                    ));


        }
    }

}