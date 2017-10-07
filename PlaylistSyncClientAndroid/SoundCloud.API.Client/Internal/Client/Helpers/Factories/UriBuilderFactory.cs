﻿using System;

namespace SoundCloud.API.Client.Internal.Client.Helpers.Factories
{
    internal class UriBuilderFactory : IUriBuilderFactory
    {
        public IUriBuilder Create(Uri uri)
        {
            return new UriBuilder(uri);
        }

        public IUriBuilder Create(string url)
        {
            return new UriBuilder(new Uri(url));
        }
    }
}