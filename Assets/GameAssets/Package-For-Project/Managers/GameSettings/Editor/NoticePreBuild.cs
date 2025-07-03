using ProjectTools;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class NoticePreBuild : MonoBehaviour, IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild (BuildReport report)
    {
        var cheat = GameSettings.OpenMap || GameSettings.OpenPurchaser || GameSettings.TestService;

        if (cheat)
        {
            var text = "";

            if(GameSettings.OpenMap)
                text += "\nOpen Map";
            if (GameSettings.OpenPurchaser)
                text += "\nOpen Purchaser";
            if (GameSettings.TestService)
                text += "\nTest Service";

            if (!EditorUtility.DisplayDialog ("NOTICE", "---- HACK HACK HACK ----" + text, "Continue", "Cancel build"))
            {
                throw new BuildFailedException ("Cancel build!");
            }
        }
    }
}
