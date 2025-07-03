using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace gcenterSdk
{
    public class ButtonExit : MonoBehaviour
    {
        [SerializeField] Image loader, exitImage;
        [SerializeField] Text countText;

        bool activate;

        public void Close ()
        {
            if (!activate)
                return;

            GCenterAdsViewer.instance.gameObject.SetActive (false);
            GCenter.onClose?.Invoke ();
        }

        public void Action (float time)
        {
            StartCoroutine (IEAction (time));
        }

        private IEnumerator IEAction (float time)
        {
            activate = false;
            exitImage.enabled = false;

            var t = time;
            while (t > 0)
            {
                countText.text = ((int)t + 1).ToString ();
                loader.fillAmount = t / time;
                t -= Time.unscaledDeltaTime;
                yield return null;
            }

            loader.fillAmount = 0;
            countText.text = "";

            exitImage.enabled = true;
            activate = true;
        }
    }
}