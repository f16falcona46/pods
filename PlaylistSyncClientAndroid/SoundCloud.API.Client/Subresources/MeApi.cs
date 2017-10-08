using System.Collections.Generic;
using System.Linq;
using SoundCloud.API.Client.Internal.Client;
using SoundCloud.API.Client.Internal.Converters;
using SoundCloud.API.Client.Internal.Infrastructure.Objects;
using SoundCloud.API.Client.Internal.Objects;
using SoundCloud.API.Client.Internal.Validation;
using SoundCloud.API.Client.Objects;
using SoundCloud.API.Client.Objects.ConnectionPieces;
using SoundCloud.API.Client.Subresources.Helpers;

namespace SoundCloud.API.Client.Subresources
{
    public class MeApi : UserApi, IMeApi
    {
        private readonly IConnectionConverter connectionConverter;
        private readonly IActivityResultConverter activityResultConverter;
        private const string prefix = "me";

        internal MeApi(
            ISoundCloudRawClient soundCloudRawClient, 
            IPaginationValidator paginationValidator, 
            IUserConverter userConverter, 
            ITrackConverter trackConverter, 
            IPlaylistConverter playlistConverter, 
            ICommentConverter commentConverter, 
            IGroupConverter groupConverter, 
            IWebProfileConverter webProfileConverter,
            IConnectionConverter connectionConverter,
            IActivityResultConverter activityResultConverter)
            : base(
                null, 
                soundCloudRawClient, 
                paginationValidator, 
                userConverter, 
                trackConverter, 
                playlistConverter, 
                commentConverter, 
                groupConverter, 
                webProfileConverter,
                prefix)
        {
            this.connectionConverter = connectionConverter;
            this.activityResultConverter = activityResultConverter;
        }

        public SCConnection[] GetConnections(int offset = 0, int limit = 50)
        {
            var connections = soundCloudRawClient.GetCollection<Connection>(paginationValidator, prefix, "connections", offset, limit);
            return connections.Select(connectionConverter.Convert).ToArray();
        }

        public string PostConnection(SCServiceType serviceType, string redirectUri)
        {
            var parameters = new Dictionary<string, object>
            {
                { "service", serviceType.GetParameterName() },
                { "redirect_uri", redirectUri}
            };
            var unsavedConnection = soundCloudRawClient.Request<UnsavedConnection>(prefix, "connections", HttpMethod.Post, parameters: parameters);
            return unsavedConnection == null ? null : unsavedConnection.AuthorizeUrl;
        }

        public SCActivityResult GetRecentActivities(string cursorToNext = null)
        {
            return GetRecentActivities("activities", cursorToNext);
        }

        public SCActivityResult GetActivityQueryResult(string queryId)
        {
            var activityResult = soundCloudRawClient.Request<ActivityResult>(prefix, "activities", HttpMethod.Get, parameters: new Dictionary<string, object> { { "uuid[to]", queryId } });
            return activityResultConverter.Convert(activityResult);
        }

        public SCActivityResult GetRecentAllActivities(string cursorToNext = null)
        {
            return GetRecentActivities("activities/all", cursorToNext);
        }

        public SCActivityResult GetRecentFollowingTracks(string cursorToNext = null)
        {
            return GetRecentActivities("activities/tracks/affiliated", cursorToNext);
        }

        public SCActivityResult GetRecentExclusivelySharedTracks(string cursorToNext = null)
        {
            return GetRecentActivities("activities/tracks/exclusive", cursorToNext);
        }

        public SCActivityResult GetRecentUserTracksActivities(string cursorToNext = null)
        {
            return GetRecentActivities("activities/all/own", cursorToNext);
        }

        //note: Temp hack. API always returns 200 comments. Must make two network calls. :(
        public override SCComment[] GetComments(int offset = 0, int limit = 50)
        {
            var me = GetUser();
            var userId = me.Id;
            var comments = soundCloudRawClient.GetCollection<Comment>(paginationValidator, string.Format("users/{0}", userId), "comments", offset, limit);
            return comments.Select(commentConverter.Convert).ToArray();
        }

        private SCActivityResult GetRecentActivities(string command, string cursorToNext)
        {
            var parameters = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(cursorToNext))
            {
                parameters.Add("cursor", cursorToNext);
            }

            var activityResult = soundCloudRawClient.Request<ActivityResult>(prefix, command, HttpMethod.Get, parameters: parameters);
            return activityResultConverter.Convert(activityResult);
        }
    }
}