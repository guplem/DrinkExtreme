using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager
{
    public static DatabaseManager instance;
    public DatabaseManager()
    {
        Debug.Log("Initializing a new DatabaseManager.");
        if (instance == null)
        {
            instance = this;
            Debug.Log("DatabaseManager initialization successful.");
        }
        else
        {
            Debug.Log("DatabaseManager already exists. New initialization unsuccessful.");
        }
    }
}
