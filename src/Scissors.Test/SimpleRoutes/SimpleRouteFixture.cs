﻿using System.Collections.Generic;

using static Scissors.Test.InMemoryConfigurationWriter;

namespace Scissors.Test.SimpleRoutes
{
    public sealed class SimpleRouteFixture : TestFixture
    {
        public SimpleRouteFixture() :
            base(new List<KeyValuePair<string, string>>
            {
                BuildMethodNamePair(0, "GET"),
                BuildPathPair(0, "/api/product?id=2"),
                BuildReturnStatusCodePair(0, 200.ToString()),
                BuildReturnJsonContentPair(0, 2.ToString()),

                BuildMethodNamePair(1, "GET"),
                BuildPathPair(1, "/api/product/2"),
                BuildReturnStatusCodePair(1, 200.ToString()),
                BuildReturnJsonContentPair(1, 2.ToString()),

                BuildMethodNamePair(2, "GET"),
                BuildPathPair(2, "api/user/*"),
                BuildReturnStatusCodePair(2, 200.ToString()),
                BuildReturnJsonContentPair(2, 11.ToString())
            })
        {
        }
    }
}