using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleManager
{
    public static ConsoleManager instance;
    public ConsoleManager()
    {
        Debug.Log("Initializing a new ConsoleManager.");
        if (instance == null)
        {
            instance = this;
            Debug.Log("ConsoleManager initialization successful.");
        }
        else
        {
            Debug.Log("ConsoleManager already exists. New initialization unsuccessful.");
        }
    }
}
