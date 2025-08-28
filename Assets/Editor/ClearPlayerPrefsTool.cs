using UnityEditor;
using UnityEngine;

public static class ClearPlayerPrefsTool {


    [MenuItem("Tools/Clear Player Prefs")]
    public static void ClearPlayerPrefs() {
        PlayerPrefs.DeleteAll();
    }
}