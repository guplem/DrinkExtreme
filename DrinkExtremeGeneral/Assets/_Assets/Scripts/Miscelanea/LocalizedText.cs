using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizedText
{
    public string id { get; private set; }
    public string type { get; private set; }
    public int difficulty { get; private set; }
    private string text;
    public DateTime lastRequested { get; private set; }

    public LocalizedText(string id, string type, int difficulty, string text)
    {
        this.id = id;
        this.type = type;
        this.difficulty = difficulty;
        this.text = text;
        lastRequested = DateTime.MinValue;
    }

    public LocalizedText(string id, string text)
        : this(id, null, 0, text) {
        //Calls the main constructor
    }

    public string getTextWithoutAddingTimestamp()
    {
        return text;
    }

    public string getTextAddingTimestamp()
    {
        lastRequested = DateTime.UtcNow;
        return text;
    }

    public static LocalizedText GetRandomTextWithIdStartingWith(string idBeggining, bool getOldestConsulted, bool useConfigFilters, List<LocalizedText> localizedTextsList)
    {
        DateTime oldestTimestampFound = DateTime.MinValue;
        LocalizedText oldestTxtFound = null;

        foreach (LocalizedText txt in localizedTextsList)
        {

            if (txt.id.StartsWith(idBeggining))
            {
                //If it is not necessary to search for the oldest one
                if (!getOldestConsulted)
                {
                    if ((useConfigFilters && IsLocalizedTextAcceptedByConfiguration(txt)) || (!useConfigFilters)) //Txt accepted by the configuration (or not needed to check)
                    {
                        oldestTxtFound = txt;
                        oldestTimestampFound = txt.lastRequested;
                        break;
                    }
                }
                //Otherwise
                else
                {
                    //If there is no text selected yet as the oldest (to increment the successful return possibilities)
                    if (oldestTxtFound == null)
                    {
                        if ((useConfigFilters && IsLocalizedTextAcceptedByConfiguration(txt)) || (!useConfigFilters)) //Txt accepted by the configuration (or not needed to check)
                        {
                            oldestTxtFound = txt;
                            oldestTimestampFound = txt.lastRequested;
                        }
                    }

                    //if txt.lastRequested is older than oldestTimestampFound save it
                    if (DateTime.Compare(oldestTimestampFound, txt.lastRequested) > 0)
                    {

                        if ((useConfigFilters && IsLocalizedTextAcceptedByConfiguration(txt) ) || (!useConfigFilters)) //Txt accepted by the configuration (or not needed to check)
                        {
                            oldestTimestampFound = txt.lastRequested;
                            oldestTxtFound = txt;
                        }
                    }

                    //If the txt can not be older than it is exit the search
                    if ((oldestTxtFound != null) && (oldestTimestampFound == DateTime.MinValue))
                    {
                        break;
                    }

                }
            }

        }

        if (oldestTxtFound == null)
        {
            Debug.LogError("No text found with the id begining with '" + idBeggining + "'" + (getOldestConsulted ? " (searching the oldest text consulted)" : ""));
            return new LocalizedText(idBeggining, "No text found with the id begining with '" + idBeggining + "'" + (useConfigFilters? " with the configured filters" : "") );
        }
        else
        {
            return oldestTxtFound;
        }

    }

    //Check if the text matches the current configuration
    public static bool IsLocalizedTextAcceptedByConfiguration(LocalizedText txt)
    {
        if (DataManager.instance == null)
        {
            Debug.LogError("DataManager.instance have not been created and it is needed to do a proper execution of the method 'IsLocalizedTextAcceptedByConfiguration()'");
            return true;
        }
        else
        {
            return DataManager.instance.difficulty >= txt.difficulty && DataManager.instance.GetAllowedTypeOfSentences().Contains(txt.type.ToUpper());
        }
    }
}
