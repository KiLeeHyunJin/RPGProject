using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace YoutubePlayer
{
    public class OnlineVideoLoader : MonoBehaviour
    {
        /*
        YoutubePlayer youtubePlayer;
        VideoPlayer videoPlayer;
        public RenderTexture renderTexture;
        RawImage rawImage;
        public string cachedVideoPath = "CachedVideo.mp4";
        public string url;
        private void Awake()
        {
            videoPlayer = GetComponent<VideoPlayer>(); //���� �÷��̾ �����´�.
            videoPlayer.url = ""; //url�ּҸ� �ʱ�ȭ�Ѵ�.
            rawImage = GetComponent<RawImage>(); //����� �̹����� �����´�.
            if (rawImage.texture == null) //��� �̹����� ����ִٸ�
                rawImage.texture = renderTexture; //���� �̹����� �����Ѵ�.
        }
        private void OnEnable()
        {
            ClearRenderTexture(Color.black); // �ʱ�ȭ�� ������ �ִ´�.
            videoPlayer.time = 0; //�������� 0���� �ʱ�ȭ�Ѵ�.
            if (videoPlayer.url == "")
            {
                youtubePlayer = gameObject.AddComponent<YoutubePlayer>(); //��Ʃ���÷��̾� ������Ʈ�� �ִ´�.
                youtubePlayer.youtubeUrl = url; //��Ʈ���� url�� ��ȯ��ų url�� �����Ѵ�.
                youtubePlayer.cli = YoutubePlayer.Cli.YtDlp;
            }
            Prepare();
        }

        public async void Prepare()
        {
            try
            {
                if (youtubePlayer != null)
                    await youtubePlayer.PrepareVideoAsync(); //�񵿱�� ���𰡸� �����Ѵ�.(����꿡�� �����°Ŷ� �۵� ������ ��.)
                StartCoroutine(Play()); //����
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e.Message);
            }
        }

        public IEnumerator Play()
        {
            while (true)
            {
                yield return null;
                if (videoPlayer == null)
                    break;
                videoPlayer.Stop(); //�������ΰ� �ִٸ� �����Ѵ�.
                videoPlayer.Play(); //�����Ѵ�.
                break;
            }
        }


        void ClearRenderTexture(Color clearColor)
        {
            // RenderTexture�� ���� Ȱ��ȭ�� RenderTexture�� �����մϴ�.
            RenderTexture.active = renderTexture;

            // clearColor�� RenderTexture�� �ʱ�ȭ�մϴ�.
            GL.Clear(true, true, clearColor);

            // RenderTexture.active�� �ٽ� null�� �����մϴ�.
            RenderTexture.active = null;
        }
        private void OnDisable()
        {
            videoPlayer?.Stop(); //����� �����.
            videoPlayer.time = 0; //�������� 0���� �����Ѵ�.
        }
        */
    }

}
