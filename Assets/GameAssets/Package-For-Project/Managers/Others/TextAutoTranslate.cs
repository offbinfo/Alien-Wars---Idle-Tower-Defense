using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class TextAutoTranslate : MonoBehaviour
{
    private void Start ()
    {
        var text = GetComponent<Text> ();
        if (text)
        {
            CultureInfo info = CultureInfo.CurrentCulture;
            var code = info.Name;
            Translator.Translate (text.text, code, (s) =>
            {
                if (text)
                    text.text = s;
            });
        }
    }
}
