using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Discovery.v1;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using static Client.Common.CODESTATUS;
using Client.Models.DTO;
namespace Client.Service
{
    public class YoutubeService
    {
        private string _videoId;
        public async Task<string> UploadVideo(HttpPostedFileBase videofile,VideoItem videoName) {
            UserCredential credential;
            using (var stream = new FileStream(System.Web.HttpContext.Current.Server.MapPath("/Common/client_secrets.json"), FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    // This OAuth 2.0 access scope allows an application to upload files to the
                    // authenticated user's YouTube channel, but doesn't allow other types of access.
                    new[] { YouTubeService.Scope.YoutubeUpload },
                    "user",
                    CancellationToken.None
                );
            }
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = Assembly.GetExecutingAssembly().GetName().Name
            });
            var video = new Video();
            video.Snippet = new VideoSnippet();
            video.Snippet.Title = videoName.title;
            video.Snippet.Description = videoName.descreption;
            video.Snippet.Tags = new string[] { "tag1", "tag2" };
            video.Status = new VideoStatus();
            video.Status.PrivacyStatus = "unlisted";
            var videosInsertRequest = youtubeService.Videos.Insert(video, "snippet,status", videofile.InputStream, "video/*");
            videosInsertRequest.ResponseReceived += videosInsertRequest_ResponseReceived;
            await videosInsertRequest.UploadAsync();
            return _videoId;
        }

        void videosInsertRequest_ResponseReceived(Video video)
        {
            _videoId = video.Id;
        }
       
        public async Task<List<VideoItem>> GetListVids()
        {
            List<VideoItem> list = new List<VideoItem>();
            UserCredential credential;
            using (var stream = new FileStream(System.Web.HttpContext.Current.Server.MapPath("/Common/client_secrets.json"), FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    // This OAuth 2.0 access scope allows an application to upload files to the
                    // authenticated user's YouTube channel, but doesn't allow other types of access.
                    new[] { YouTubeService.Scope.YoutubeReadonly},
                    "user",
                    CancellationToken.None,
                    new FileDataStore(this.GetType().ToString())
                );
            }
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = this.GetType().ToString()
            });
            var channelsListRequest = youtubeService.Channels.List("contentDetails");
            channelsListRequest.Mine = true;
            // Retrieve the contentDetails part of the channel resource for the authenticated user's channel.
            var channelsListResponse = await channelsListRequest.ExecuteAsync();

            foreach (var channel in channelsListResponse.Items)
            {
                // From the API response, extract the playlist ID that identifies the list
                // of videos uploaded to the authenticated user's channel.
                var uploadsListId = channel.ContentDetails.RelatedPlaylists.Uploads;
                var playlistItemsListRequest = youtubeService.PlaylistItems.List("snippet");
                playlistItemsListRequest.PlaylistId = uploadsListId;
                playlistItemsListRequest.MaxResults = 50;

                // Retrieve the list of videos uploaded to the authenticated user's channel.
                var playlistItemsListResponse = await playlistItemsListRequest.ExecuteAsync();
                foreach (var playlistItem in playlistItemsListResponse.Items)
                {
                    // Print information about each video.
                    var vd = new VideoItem();
                    var newUrl = "https://www.youtube.com/embed/{0}";
                    vd.videoId = playlistItem.Snippet.ResourceId.VideoId;
                    vd.title = playlistItem.Snippet.Title;
                    vd.descreption = playlistItem.Snippet.Description;
                    vd.url = string.Format(newUrl, vd.videoId);
                    list.Add(vd);
                }
            }
        return list;
        }

    }
}