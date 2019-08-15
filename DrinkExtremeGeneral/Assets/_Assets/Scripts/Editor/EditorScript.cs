using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorScript
{
    [MenuItem("Drink Extreme/Saves/Delete all saved info")]
    public static void DeleteAllSavedInfo()
    {
        if (Application.isPlaying)
        {
            Debug.LogError("Not avaliable in play mode");
        }
        else
        {
            Debug.LogWarning("Deleting of custom sentences trough this method is not implemented yet");
            PlayerPrefs.DeleteAll();
            Debug.Log("All player prefs have been deleted");
        }
    }
}
