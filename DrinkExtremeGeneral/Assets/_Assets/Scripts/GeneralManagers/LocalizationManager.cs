using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System;
using DrinkExtreme;


public class LocalizationManager
{
    public List<LocalizedText> localizedTexts { get; private set; }

    public static LocalizationManager instance;
    public LocalizationManager()
    {
        Debug.Log("Initializing a new LocalizationManager.");
        if (instance == null)
        {
            instance = this;
            localizedTexts = new List<LocalizedText>();
            //GetAllLocalizedLanguagesAndDialects();
            Debug.Log("LocalizationManager initialization successful.");
        }
        else
        {
            Debug.Log("LocalizationManager already exists. New initialization unsuccessful.");
        }
    }


    public bool LoadLanguage(string lang)
    {
        Debug.Log(" > Loading language: '" + lang +"'");

        if (string.IsNullOrEmpty(lang)) {
            Debug.LogError("The desired language is null or empty");
            return false;
        }

        DataManager.instance.setLanguage(lang, true);

        localizedTexts.Clear();

        //Get the line
        TextAsset csvFile = Resources.Load<TextAsset>(Utils.localizationFilesPath + Utils.translationsFilename);
        string[] linesFromFile = csvFile.text.Split("\n"[0]);

        int[] langColumn = new int[3]; //0 = desired language, 1 = main dialect of the desired language, 2 = main/backup language of the game
        langColumn[0] = Utils.GetColumNumberWithText(lang, linesFromFile[0]);
        langColumn[1] = Utils.GetColumNumberWithText(GetMainDialectOfLanguage(lang), linesFromFile[0]);
        langColumn[2] = Utils.GetColumNumberWithText(GetMainLanguage(), linesFromFile[0]);

        string[] langnames = Utils.CsvLineToArray(linesFromFile[0]); //To debug
        Debug.Log(
            " > Selected language configuration: \n"
            + ((langColumn[0] < 0) ? "null (" + langColumn[0] + ")" : langnames[langColumn[0]]) + " = language column as desired language (" + langColumn[0] + "), \n"
            + ((langColumn[1] < 0) ? "null (" + langColumn[1] + ")" : langnames[langColumn[1]]) + " = language column as main dialect of the desired language (" + langColumn[1] + "), \n"
            + ((langColumn[2] < 0) ? "null (" + langColumn[2] + ")" : langnames[langColumn[2]]) + " = language column as backup language (" + langColumn[2] + ")");

        for (int lineNumber = 1; lineNumber < linesFromFile.Length; lineNumber++)
        {
            string[] formatedLine = Utils.CsvLineToArray(linesFromFile[lineNumber]);
            int langNumber = 0;
            string text = null;

            while (langColumn[langNumber] < 0 && langNumber < 3)
                langNumber++; //search for the first valid language found

            while (String.IsNullOrEmpty(text) && langNumber < 3)
            {
                if (langColumn[langNumber] < 0)
                {
                    langNumber++;
                }
                else
                {
                    if (formatedLine[langColumn[langNumber]].CompareTo(Utils.notLocalizableSentenceIdentifier) != 0)
                    {
                        text = formatedLine[langColumn[langNumber]];
                        langNumber++;
                    }
                    else
                    {
                        langNumber = int.MaxValue;
                    }
                }

            }

            if (String.IsNullOrEmpty(text))
            {
                if (langNumber != int.MaxValue)
                    Debug.LogError(" > " + formatedLine[0] + "could not be loaded in " + lang + " or any major language.");
            } else
            {
                localizedTexts.Add(new LocalizedText(formatedLine[0], formatedLine[1], Int32.Parse(formatedLine[2]), text));
            }
        }

        Debug.Log(" > " + "Finished load of the language.");

        LocalizeAllLocalizableObjects();
        localizedTexts = Utils.RandomizeOrder(localizedTexts);

        return true;
    }

    public delegate void LocalizeAllAction();
    public static event LocalizeAllAction OnLocalizeAllAction;
    public void LocalizeAllLocalizableObjects()
    {
        if (OnLocalizeAllAction != null)
        {
            Debug.Log("   > " + "Localizing all objects");
            OnLocalizeAllAction();
        }
    } 

    public string GetMainLanguage()
    {
        return Utils.mainLanguage;
    }

    public string GetMainDialectOfLanguage(string lang)
    {
        TextAsset languagePriority = Resources.Load<TextAsset>(Utils.localizationFilesPath + Utils.languagePriorityFilename);
        string[] lineOfLanguage = languagePriority.text.Split("\n"[0]);

        foreach (string line in lineOfLanguage)
        {
            string lineModified = line;
            lineModified = lineModified.Trim().ToUpper();
            lineModified = lineModified.Replace(" ", "");
            lineModified = lineModified.Replace(Utils.endOfFileTag, "");

            if (lineModified.Contains(lang.Replace(" ", "").Trim().ToUpper()))
            {
                return Utils.CsvLineToArray(lineModified)[0];
            }
        }
        Debug.LogWarning("Main dialect of language " + lang + " not found");
        return null;
    }

    private string GetLocalizedValueOfId(string id, bool registerTimestamp)
    {
        if (!String.IsNullOrEmpty(id))
        {
            foreach (LocalizedText txt in localizedTexts)
            {
                if (String.Equals(txt.id, id, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (registerTimestamp)
                        return txt.getTextAddingTimestamp();
                    else
                        return txt.getTextWithoutAddingTimestamp();
                }
            }
            Debug.LogWarning("Localized value of '" + id + "' not found.");
        }
        return "";
    }

    public string GetLocalizedTextOf(string id, bool registerTimestamp)
    {
        string text = GetLocalizedValueOfId(id, registerTimestamp);

        if (String.IsNullOrEmpty(text) && (!String.IsNullOrEmpty(id)))
        {
            text = "Text '" + id + "' not found.";
            //Debug.LogWarning(text);
        }

        return text;
    }

    public Sprite GetLocalizedSpriteOf(string id, bool registerTimestamp)
    {
        string spriteName = GetLocalizedValueOfId(id, registerTimestamp);
        if (String.IsNullOrEmpty(spriteName))
        {
            //Debug.LogWarning("Sprite '" + id + "' not found.");
            return null;
        }
        else
        {
            var resource = Resources.Load<Sprite>(Utils.localizedImagesPath + spriteName);
            if (resource == null)
                Debug.LogWarning("Resource '" + id + "' could not be loaded.");
            return resource;
        }
    }

    public List<string> GetAllLocalizedLanguagesAndDialects()
    {
        TextAsset csvFile = Resources.Load<TextAsset>(Utils.localizationFilesPath + Utils.translationsFilename);
        string[] firstLineFromFile = Utils.CsvLineToArray( csvFile.text.Split("\n"[0]) [0] ); //Split the csv in lines (using "split"), get the first line (using "[0]") and convert it to an array (using "CsvLineToArray").

        List<string> listOfLanguages = new List<string>();
        foreach (string lang in firstLineFromFile)
        {
            if( (lang != Utils.localizationFileNameColumnId) && (lang != Utils.localizationFileNameColumnType) && (lang != Utils.localizationFileNameColumnDifficulty) && (!lang.Contains(Utils.endOfFileTag)) )
            {
                if (!listOfLanguages.Contains(lang))
                    listOfLanguages.Add(lang);
            }
        }
        return listOfLanguages;
    }

    private string GetLineOfTheLanguageInLangInfoFile(string languageOrDialect)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(Utils.localizationFilesPath + Utils.languagesInfoFilename);

        string[] allFileLines = csvFile.text.Split("\n"[0]); //Split the csv in lines (using "split"), get the first line (using "[0]") and convert it to an array (using "CsvLineToArray").
        foreach (string line in allFileLines)
        {
            if (line.ToUpper().Contains(languageOrDialect.ToUpper()))
            {
                Debug.Log("Obtained info's line for the language '" + languageOrDialect + "' = " + line);
                return line;
            }
        }
        Debug.LogWarning("Language or dialect " + languageOrDialect + " not found in LangInfo file");
        return "";
    }

    public string GetLanguageOrDialectName(string languageOrDialect)
    {
        string text = Utils.CsvLineToArray(
            GetLineOfTheLanguageInLangInfoFile(languageOrDialect)
            )[Utils.columnNumberOfLanguageNameInLangInfoFile];
        if (String.IsNullOrEmpty(text))
        {
            Debug.LogWarning(text);
        }
        Debug.Log("Loaded name for " + languageOrDialect + " is '" + text + "'");
        return text;
    }

    public Sprite GetLanguageOrDialectImg(string languageOrDialect)
    {
        string resourceId = Utils.CsvLineToArray(GetLineOfTheLanguageInLangInfoFile(languageOrDialect))[Utils.columnNumberOfLanguageImgInLangInfoFile];
        var resource = Resources.Load<Sprite>(Utils.localizedImagesPath + resourceId);
        if (resource == null)
            Debug.LogWarning("Resource '" + resourceId + "' could not be loaded.");
        return resource;
    }

}
