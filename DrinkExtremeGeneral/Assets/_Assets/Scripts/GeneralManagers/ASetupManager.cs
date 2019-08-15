using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ASetupManager// : MonoBehaviour
{
    
    public static void Setup()
    {
        Debug.Log(" ===== Start setup ===== ");

        new GameManager();
        //new DebugManager();
        //new DatabaseManager();
        new LocalizationManager();
        new SavesManager();
        new DataManager();
        //new ConsoleManager();

        LocalizationManager.instance.LoadLanguage(DataManager.instance.language);

        Debug.Log(" ===== Setup finished ===== ");
    }

}