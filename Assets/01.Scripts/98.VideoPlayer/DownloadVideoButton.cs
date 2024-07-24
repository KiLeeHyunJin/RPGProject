using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using YoutubePlayer.Components;
using YoutubePlayer.Extensions;

namespace YoutubePlayer.Samples.PlayVideo
{
    public class DownloadVideoButton : MonoBehaviour
    {
        public InvidiousVideoPlayer videoPlayer;
        [SerializeField] UnityEngine.UI.Button downloadBtn;

        string path;
        private void Start()
        {
            downloadBtn.NullCheckAndAddListener(Download);
            path = Application.streamingAssetsPath;
        }


        public async void Download()
        {
            string videoId = videoPlayer.VideoId;

            string videoUrl = await videoPlayer.InvidiousInstance.GetVideoUrl(videoId);

            if(await WebUtils.ValidURL(videoUrl) && await WebUtils.CanDownload(videoUrl) == false)
            {
                videoUrl = videoPlayer.VideoPlayer.url;
            }
            else
                videoUrl = videoPlayer.VideoPlayer.url;

            string filePath = System.IO.Path.Combine(path, $"{videoId}.mp4");
            try
            {
                Message.Log($"Downloading video ${videoId} to {filePath}");
                Message.Log($"Downloaded video to {filePath}");

                await DownloadAsync(videoUrl, filePath);

                Utils.OpenFolder(path);

            }
            catch (Exception e)
            {
                Message.LogError($"Failed to download video: {e.Message}");
            }
        }

        static Task DownloadAsync(string videoUrl, string filePath, CancellationToken cancellationToken = default)
        {
            Message.Log(videoUrl);
            var tcs = new TaskCompletionSource<bool>();
            var request = UnityWebRequest.Get(videoUrl);
            cancellationToken.Register(o => request.Abort(), true);
            request.downloadHandler = new DownloadHandlerFile(filePath);
            request.SendWebRequest().completed += operation => 
            {
                if (request.result != UnityWebRequest.Result.Success)
                {
                    tcs.TrySetException(new Exception(request.error));
                    return;
                }
                tcs.TrySetResult(true);
            };
            return tcs.Task;
        }
    }
}
