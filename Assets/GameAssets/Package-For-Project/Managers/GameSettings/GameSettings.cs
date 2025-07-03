using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ProjectTools
{
    public class GameSettings : ScriptableObject
    {
#if UNITY_EDITOR

        [MenuItem ("GameSettings/OpenSettings")]
        public static void Select ()
        {
            if (!Instance)
            {
                var obj = CreateInstance<GameSettings> ();
                AssetDatabase.CreateAsset (obj, "Assets/Resources/GameSettings.asset");
            }

            Selection.activeObject = Instance;
        }

        [MenuItem ("GameSettings/Off All")]
        public static void OffAll ()
        {
            Instance.openMap = false;
            Instance.openPurchaser = false;
            Instance.testService = false;

            EditorUtility.SetDirty (Instance);
            AssetDatabase.SaveAssets ();
        }
#endif

        [SerializeField] bool openMap = false;
        [SerializeField] bool openPurchaser = false;
        [SerializeField] bool testService = false;

        public static bool OpenMap => Instance.openMap;
        public static bool OpenPurchaser => Instance.openPurchaser;
        public static bool TestService => Instance.testService;

        static GameSettings instance;
        static GameSettings Instance
        {
            get
            {
                if (!instance)
                    instance = Resources.Load<GameSettings> ("GameSettings");

                return instance;
            }
        }

        public static void SetValue (bool openMap, bool openPurchaser, bool testSerive, bool testButton)
        {
            Instance.openMap = openMap;
            Instance.openPurchaser = openPurchaser;
            Instance.testService = testSerive;
        }
    }
}