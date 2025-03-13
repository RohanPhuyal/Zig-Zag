using UnityEditor;
using UnityEngine;

public class PlayerPrefsEditor : EditorWindow
{
    [MenuItem("Tools/Reset PlayerPrefs")]
    public static void ResetPlayerPrefs()
    {
        // This will delete all PlayerPrefs
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs have been reset!");
    }
}