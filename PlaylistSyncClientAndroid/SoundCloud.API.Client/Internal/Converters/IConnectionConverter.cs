﻿using SoundCloud.API.Client.Internal.Objects;
using SoundCloud.API.Client.Objects;

namespace SoundCloud.API.Client.Internal.Converters
{
    internal interface IConnectionConverter
    {
        SCConnection Convert(Connection connection);
        Connection Convert(SCConnection connection);
    }
}