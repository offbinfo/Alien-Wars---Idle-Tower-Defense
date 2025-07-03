using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace gcenterSdk
{
    public class GCenterAdsViewer : MonoBehaviour
    {
        [SerializeField] Image interstitalImage, icon;
        [SerializeField] Text platformText, nameText;
        [SerializeField] VideoPlayer videoPlayer;

        public static GCenterAdsViewer instance;

        ButtonExit buttonExit;
        string url;

        public void Download ()
        {
            Application.OpenURL (url);
        }

        public void Setup (GCenterAdsStruct adsStruct)
        {
            gameObject.SetActive (true);

            var interTexture = adsStruct.largeImage;

            interstitalImage.gameObject.SetActive (interTexture);
            interstitalImage.sprite = interTexture;

            if (interTexture)
            {
                var texture = interTexture.texture;

                if (texture.width > texture.height)
                {
                    var width = Screen.width;
                    var ratio = width * 1f / texture.width;
                    interstitalImage.rectTransform.sizeDelta = new Vector2 (width, texture.height * ratio);
                }
                else
                {
                    var height = Screen.height;
                    var ratio = height * 1f / texture.height;
                    interstitalImage.rectTransform.sizeDelta = new Vector2 (texture.width * ratio, height);
                }
            }

            icon.sprite = adsStruct.icon;

            videoPlayer.gameObject.SetActive (!string.IsNullOrEmpty (adsStruct.clipUrl));
            videoPlayer.url = adsStruct.clipUrl;

            if (!string.IsNullOrEmpty (videoPlayer.url))
                videoPlayer.Play ();

            platformText.text = adsStruct.platForm;
            nameText.text = adsStruct.title;

            url = adsStruct.url;

            buttonExit.Action (adsStruct.time);
        }

        private void Awake ()
        {
            if (!instance)
            {
                instance = this;
                DontDestroyOnLoad (this);
            }
            else
            {
                Destroy (gameObject);
            }

            buttonExit = GetComponentInChildren<ButtonExit> ();
        }
    }
}