﻿using System.Collections.Generic;
using Plato.Internal.Models.Notifications;
using Plato.Internal.Notifications.Abstractions;
using Plato.Internal.Security.Abstractions;

namespace Plato.Discuss.Follow.NotificationTypes
{

    public class EmailNotifications : INotificationTypeProvider
    {
        
        public static readonly EmailNotification NewReply =
            new EmailNotification("NewReplyEmail", "Topic Replies", "Send me an email notification for each new reply within topics I'm following.");

        public IEnumerable<DefaultNotificationTypes> GetNotificationTypes()
        {
            return new[]
            {
                new DefaultNotificationTypes
                {
                    RoleName = DefaultRoles.Administrator,
                    NotificationTypes = new[]
                    {
                        NewReply
                    }
                },
                new DefaultNotificationTypes
                {
                    RoleName = DefaultRoles.Staff,
                    NotificationTypes = new[]
                    {
                        NewReply
                    }
                },
                new DefaultNotificationTypes
                {
                    RoleName = DefaultRoles.Member,
                    NotificationTypes = new[]
                    {
                        NewReply
                    }
                }

            };
        }

        public IEnumerable<DefaultNotificationTypes> GetDefaultNotificationTypes()
        {
            return new[]
            {
                new DefaultNotificationTypes
                {
                    RoleName = DefaultRoles.Administrator,
                    NotificationTypes = new[]
                    {
                        NewReply
                    }
                },
                new DefaultNotificationTypes
                {
                    RoleName = DefaultRoles.Staff,
                    NotificationTypes = new[]
                    {
                        NewReply
                    }
                },
                new DefaultNotificationTypes
                {
                    RoleName = DefaultRoles.Member,
                    NotificationTypes = new[]
                    {
                        NewReply
                    }
                }

            };

        }


    }

}
