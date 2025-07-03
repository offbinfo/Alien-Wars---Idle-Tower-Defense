using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace gcenterSdk
{
    [RequireComponent (typeof (RawImage))]
    public class VideoRender : MonoBehaviour
    {
        RawImage rawImage;

        private void Awake ()
        {
            rawImage = GetComponent<RawImage> ();

            var video = GetComponent<VideoPlayer> ();
            video.started += Video_started;
        }

        private void Video_started (VideoPlayer source)
        {
            var texture = new RenderTexture ((int)source.width, (int)source.height, 16, RenderTextureFormat.ARGB32);
            rawImage.texture = texture;
            source.targetTexture = texture;

            if (texture.width > texture.height)
            {
                var width = Screen.width;
                var ratio = width * 1f / texture.width;
                (transform as RectTransform).sizeDelta = new Vector2 (width, texture.height * ratio);
            }
            else
            {
                var height = Screen.height;
                var ratio = height * 1f / texture.height;
                (transform as RectTransform).sizeDelta = new Vector2 (texture.width * ratio, height);
            }
        }
    }
}
