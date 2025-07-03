using System;
using UnityEngine;

namespace language
{
    public class LanguageManager
    {
        static LanguageAssets languageAssets = null;
        public static Action onChangeLanguage = null;

        public static LanguageAssets LanguageAssets
        {
            get
            {
                if (!languageAssets)
                {
                    var assets = Resources.LoadAll<LanguageAssets> ("");
                    if (assets.Length > 0)
                    {
                        languageAssets = assets [0];
                    }
                }

                return languageAssets;
            }
        }

        static string languageSave = null;
        public static string language
        {
            set
            {
                languageSave = value;
                SavePrefs.SetString (GameKeys.Key_Language, value);
                if (onChangeLanguage != null)
                    onChangeLanguage.Invoke ();
            }
            get
            {
                if (languageSave == null)
                {
                    languageSave = SavePrefs.GetString (GameKeys.Key_Language, LanguageAssets.LanguageDefault);
                }

                return languageSave;
            }
        }

        public static string GetText (string key, string language)
        {

            if (LanguageAssets)
            {
                if (!LanguageAssets.DictionaryLanguage.ContainsKey (language))
                {
                    language = LanguageAssets.LanguageDefault;
                }

                
                if (LanguageAssets.DictionaryLanguage.ContainsKey (language))
                {
                    
                    if (LanguageAssets.DictionaryLanguage [language].ContainsKey (key))
                    {
                        string text = LanguageAssets.DictionaryLanguage [language] [key];
                        return text.Replace ("[*]", "\n");
                    }
                }

                if (LanguageAssets.DictionaryLanguage [LanguageAssets.LanguageDefault].ContainsKey (key))
                    return LanguageAssets.DictionaryLanguage [LanguageAssets.LanguageDefault] [key].Replace ("[*]", "\n");
            }

            return key;
        }

        public static string GetText (string key)
        {
            return GetText (key, language);
        }

        public static void Reload ()
        {
            languageAssets = null;
            languageSave = null;
        }
    }
}
