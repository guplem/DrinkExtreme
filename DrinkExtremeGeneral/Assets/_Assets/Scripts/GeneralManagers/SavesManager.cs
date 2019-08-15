using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using DrinkExtreme;
using UnityEditor;

[ExecuteInEditMode]
public class SavesManager
{
    public static SavesManager instance;
    public SavesManager()
    {
        Debug.Log("Initializing a new SavesManager.");
        if (instance == null)
        {
            instance = this;
            Debug.Log("SavesManager initialization successful.");
        }
        else
        {
            Debug.Log("SavesManager already exists. New initialization unsuccessful.");
        }
    }


    public string LoadLanguage()
    {
        return PlayerPrefs.GetString(Utils.languageSavename, GetSystemLanguageAndShowLanguagePanel() );
    }

    public string GetSystemLanguageAndShowLanguagePanel()
    {
        if (GUIManager.instance == null)
        {
            Debug.LogError("NULL GUIMANAGER");
        }
        GUIManager.instance.OpenConfigurationPanel(GUIManager.configurationPanels.ConfLang);
        return Utils.GetSystemLanguage();
    }

    public Dictionary<Utils.TypeOfSentence, bool> LoadAllowedTypesOfSentences()
    {
        Dictionary<Utils.TypeOfSentence, bool> typesOfSentencesToReturn = new Dictionary<Utils.TypeOfSentence, bool>();

        string typesOfSentencesSaved = PlayerPrefs.GetString(Utils.allowedTypesOfSentencesSavename, null);

        if (string.IsNullOrEmpty(typesOfSentencesSaved))
        {
            foreach (Utils.TypeOfSentence type in Enum.GetValues(typeof(Utils.TypeOfSentence)))
                typesOfSentencesToReturn.Add(type, true);
        }
        else
        {
            foreach (Utils.TypeOfSentence type in Enum.GetValues(typeof(Utils.TypeOfSentence)))
                typesOfSentencesToReturn.Add(type, false);

            string[] typesOfSentencesSeparated = Utils.getStringSeparated(typesOfSentencesSaved);
            foreach (string typeString in typesOfSentencesSeparated)
            {
                Utils.TypeOfSentence typeOfSentence = Utils.ParseEnum<Utils.TypeOfSentence>(typeString);
                typesOfSentencesToReturn[typeOfSentence] = true;
            }
        }

        return typesOfSentencesToReturn;
    }

    public int LoadDifficulty()
    {
        return PlayerPrefs.GetInt(Utils.difficultySavename, Utils.defaultDifficulty);
    }

    public List<string> LoadPlayers()
    {
        string playersStringSaved = PlayerPrefs.GetString(Utils.playersSaveName, null);
        string[] playersStringSeparated = Utils.getStringSeparated(playersStringSaved);

        if (string.IsNullOrEmpty(playersStringSaved))
            return Utils.defaultPlayers;

        return Utils.Trim(new List<string>(playersStringSeparated));
    }

    public List<LocalizedText> LoadCustomSentences()
    {
        List<LocalizedText> customSentences = new List<LocalizedText>();

        foreach (string sentence in ReadLinesFromFile(Utils.customSentencesFilename))
        {
            string[] parts = Utils.getStringSeparated(sentence);
            customSentences.Add(new LocalizedText(parts[0]/*Id*/, parts[1]/*Text*/));
        }
        return customSentences;
    }

    public void SaveLanguage(string language)
    {
        PlayerPrefs.SetString(Utils.languageSavename, language);
    }

    public void SaveAllowedTypesOfSentences(Dictionary<Utils.TypeOfSentence, bool> typesOfSentences)
    {
        string stringToSave = "";
        foreach (var type in typesOfSentences)
            if (type.Value == true)
                stringToSave += Utils.GetTypeOfSentenceStringOf(type.Key) + Utils.dataSeparator; //Saves only the types of sentences marked as "true"
            
        PlayerPrefs.SetString(Utils.allowedTypesOfSentencesSavename, stringToSave);
    }

    public void SaveDifficulty(int difficulty)
    {
        PlayerPrefs.SetInt(Utils.difficultySavename, difficulty);
    }

    public void SavePlayers(List<string> players)
    {
        PlayerPrefs.SetString(Utils.playersSaveName, Utils.getStringsUnited(players) );
    }

    public void SaveCustomSentences(List<LocalizedText> customSentences)
    {
        List<string> linesToSave = new List<string>();
        foreach (LocalizedText sentence in customSentences)
        {
            string[] info = { sentence.id, sentence.getTextWithoutAddingTimestamp()};
            linesToSave.Add(Utils.getStringsUnited(info));
        }
        SaveLinesToFile(linesToSave, Utils.customSentencesFilename);
    }

    public int LoadCondition(Utils.Condition condition)
    {
        return PlayerPrefs.GetInt(Utils.GetConditionStringOf(condition), Utils.nullVal);
    }

    public void SaveCondition(Utils.Condition condition, bool value)
    {
        PlayerPrefs.SetInt(Utils.GetConditionStringOf(condition), value ? Utils.trueVal : Utils.falseVal);
    }

    public void SaveCondition(Utils.Condition condition, int valFromUtils)
    {
        PlayerPrefs.SetInt(Utils.GetConditionStringOf(condition), valFromUtils);
    }

    public bool LoadRandomSentencesAllowed()
    {
        return PlayerPrefs.GetInt(Utils.randomSentencesAllowedSavename, Utils.defaultRandomSentencesAllowed ? Utils.trueVal : Utils.falseVal) == Utils.trueVal ? true : false;
    }

    public bool LoadRateAppShown()
    {
        return PlayerPrefs.GetInt(Utils.rateAppShowSavename, Utils.falseVal) == Utils.trueVal ? true : false;
    }

    public bool LoadShowAds()
    {
        return PlayerPrefs.GetInt(Utils.showAdsSavename, Utils.trueVal) == Utils.trueVal ? true : false;
    }

    public void SaveShowAds(bool state)
    {
        PlayerPrefs.SetInt(Utils.showAdsSavename, state ? Utils.trueVal : Utils.falseVal);
    }

    public void SaveRateAppShown(bool state)
    {
        PlayerPrefs.SetInt(Utils.rateAppShowSavename, state ? Utils.trueVal : Utils.falseVal);
    }

    public bool LoadCustomSentencesAllowed()
    {
        return PlayerPrefs.GetInt(Utils.customSentencesAllowedSavename, Utils.defaultRandomSentencesAllowed ? Utils.trueVal : Utils.falseVal) == Utils.trueVal ? true : false;
    }

    public void SaveRandomSencesAllowed(bool state)
    {
        PlayerPrefs.SetInt(Utils.randomSentencesAllowedSavename, state? Utils.trueVal : Utils.falseVal);
    }

    public void SaveCustomSencesAllowed(bool state)
    {
        PlayerPrefs.SetInt(Utils.customSentencesAllowedSavename, state ? Utils.trueVal : Utils.falseVal);
    }

    public int LoadLastIdOfCustomSentence()
    {
        return PlayerPrefs.GetInt(Utils.lastIdOfCustomSentenceSavename, 0);
    }

    public void SaveLastIdOfCustomSentence(int lastId)
    {
        PlayerPrefs.SetInt(Utils.lastIdOfCustomSentenceSavename, lastId);
    }

    public List<string> ReadLinesFromFile(string filename) //TBD: add file extension?
    {
        List<string> lines = new List<string>();
        try
        {
            StreamReader inp_stm = new StreamReader(Application.persistentDataPath + "/" + filename);

            while (!inp_stm.EndOfStream)
            {
                string line = inp_stm.ReadLine();
                lines.Add(line);
            }

            inp_stm.Close( ); 
        }
        catch (Exception) { }

        return lines;
    }

    public void SaveLinesToFile(List<string> lines, string filename) //TBD: add file extension?
    {
        StreamWriter out_stm = new StreamWriter(Application.persistentDataPath + "/" + filename);

        foreach (string line in lines)
        {
            out_stm.WriteLine(line);
        }

        out_stm.Close();

    }



}