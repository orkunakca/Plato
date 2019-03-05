﻿using System;
using Microsoft.Extensions.Localization;
using Plato.Discuss.Models;
using Plato.Internal.Navigation.Abstractions;

namespace Plato.Discuss.History.Navigation
{

    public class TopicMenu : INavigationProvider
    {
        
        public IStringLocalizer T { get; set; }

        public TopicMenu(IStringLocalizer localizer)
        {
            T = localizer;
        }
        
        public void BuildNavigation(string name, INavigationBuilder builder)
        {

            if (!String.Equals(name, "topic", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            // Get model from navigation builder
            var topic = builder.ActionContext.HttpContext.Items[typeof(Topic)] as Topic;
            if (topic == null)
            {
                return;
            }

            // Add HistoryMenu view to topic
            builder
                .Add(T["History"], int.MinValue, history => history
                    .View("HistoryMenu", new
                    {
                        topic
                    })
                    .Permission(Permissions.ViewTopicHistory)
                );

        }

    }

}
