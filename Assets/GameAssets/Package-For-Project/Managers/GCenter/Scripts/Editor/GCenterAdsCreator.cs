using System.IO;
using UnityEditor;
using UnityEngine;

namespace gcenterSdk
{
    public class GCenterAdsCreator : ScriptableWizard
    {
        [MenuItem ("Project-Tools/GCenterAds Creator")]
        static void CreateWindow ()
        {
            DisplayWizard<GCenterAdsCreator> ("Create Data", "Exit", "Create").LoadJsonToData ();
        }

        [ContextMenuItem("Paste json", "LoadJsonToData")]
        public DataStruct dataStruct;

        private void LoadJsonToData ()
        {
            var json = GUIUtility.systemCopyBuffer;
            var data = JsonParse.FromJson<DataStruct> (json);
            if (data != default)
                dataStruct = data;
        }

        private void OnWizardCreate ()
        {
            
        }

        private void OnWizardOtherButton ()
        {
            Debug.Log ("Created!");
            var json = JsonParse.ToJson (dataStruct);
            var path = EditorUtility.SaveFilePanel ("Select Folder", "", "gcenter-ads-data-config", "json");

            if (!string.IsNullOrEmpty (path))
            {
                File.WriteAllText (path, json);
                Application.OpenURL (Path.GetDirectoryName (path));
            }
        }
    }
}