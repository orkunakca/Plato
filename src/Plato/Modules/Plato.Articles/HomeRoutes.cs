﻿using System.Collections.Generic;
using Plato.Internal.Abstractions.Routing;

namespace Plato.Articles
{

    public class HomeRoutes : IHomeRouteProvider
    {
        public IEnumerable<HomeRoute> GetRoutes()
        {
            return new[]
            {
                new HomeRoute("Plato.Articles", "Home", "Index")
            };
        }

    }

}
