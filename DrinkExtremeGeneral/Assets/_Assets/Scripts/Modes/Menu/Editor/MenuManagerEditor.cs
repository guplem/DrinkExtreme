using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MenuManager))]
public class MenuManagerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MenuManager mainMenuManager = (MenuManager)target;

        if (GUILayout.Button("Preview buttons of modes (without correct image)"))
            mainMenuManager.GenerateModeButtons();
    }

}
