using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DrinkExtreme;

public class DataManager
{

    public string language { get; private set; }
    public List<string> players { get; private set; }
    public int difficulty { get; private set; }
    public Dictionary<Utils.TypeOfSentence, bool> allowedTypesOfSentences { get; private set; }
    public bool randomSentencesAllowed { get; private set; }
    public bool customSentencesAllowed { get; private set; }
    public List<LocalizedText> customSentences { get { return Utils.RandomizeOrder(this.sortedCustomSentences);  }  private set { this.sortedCustomSentences = value;  } } //Get is less eficient than "sortedCustomSentences"
    public List<LocalizedText> sortedCustomSentences { get; private set; } //Get from here is more eficient than "customSentences"
    private int lastIdOfCustomSentence;
    public int numOfCustomSentencesAlreadyShown = 0;
    public int timeInLastMode = 0, timeInCurrentMode = 0;
    public bool rateAppPopupShown { get; private set; }
    public bool showAds { get; private set; }

    //custom sentences

    public static DataManager instance;
    public DataManager()
    {
        Debug.Log("Initializing a new DataManager.");
        if (instance == null)
        {
            instance = this;
            Setup();
            Debug.Log("DataManager initialization successful.");
        }
        else
        {
            Debug.Log("DataManager already exists. New initialization unsuccessful.");
        }
    }

    private void Setup()
    {
        setLanguage(SavesManager.instance.LoadLanguage(), false);
        setPlayers(SavesManager.instance.LoadPlayers(), false);
        setDifficulty(SavesManager.instance.LoadDifficulty(), false);
        allowedTypesOfSentences = SavesManager.instance.LoadAllowedTypesOfSentences();
        randomSentencesAllowed = SavesManager.instance.LoadRandomSentencesAllowed();
        customSentencesAllowed = SavesManager.instance.LoadCustomSentencesAllowed();
        sortedCustomSentences = SavesManager.instance.LoadCustomSentences();
        lastIdOfCustomSentence = SavesManager.instance.LoadLastIdOfCustomSentence();
        rateAppPopupShown = SavesManager.instance.LoadRateAppShown();
        showAds = SavesManager.instance.LoadShowAds();
    }

    
    public void setRateAppPopupShown(bool state, bool save)
    {
        this.rateAppPopupShown = state;
        if (save)
            SavesManager.instance.SaveRateAppShown(this.rateAppPopupShown);
    }

    public void setShowAds(bool state, bool save)
    {
        this.showAds = state;
        if (save)
            SavesManager.instance.SaveShowAds(this.showAds);
    }

    public void setLanguage(string language, bool save)
    {
        this.language = language;
        if (save)
            SavesManager.instance.SaveLanguage(this.language);
    }

    public void setPlayers(List<string> players, bool save)
    {
        this.players = players;
        if (save)
            SavesManager.instance.SavePlayers(this.players);
    }
    public void addPlayer(string player, bool save)
    {
        this.players.Add(player);
        if (save)
            SavesManager.instance.SavePlayers(this.players);
    }
    public void addPlayer(string player, int index, bool save)
    {
        this.players.Insert(index, player);
        if (save)
            SavesManager.instance.SavePlayers(this.players);
    }
    public void removePlayer(int playerIndex, bool save)
    {
        this.players.RemoveAt(playerIndex);
        if (save)
            SavesManager.instance.SavePlayers(this.players);
    }

    public void setDifficulty(int difficulty, bool save)
    {
        this.difficulty = difficulty;
        if (save)
            SavesManager.instance.SaveDifficulty(this.difficulty);
    }
    public void setStateOfTypeOfSentence(Utils.TypeOfSentence typeOfSentence, bool state, bool save)
    {
        this.allowedTypesOfSentences[typeOfSentence] = state;
        if (save)
            SavesManager.instance.SaveAllowedTypesOfSentences(this.allowedTypesOfSentences);
    }
    public void setAllowedTypesOfSentences(Dictionary<Utils.TypeOfSentence, bool> allowedTypesOfSentences, bool save)
    {
        this.allowedTypesOfSentences = allowedTypesOfSentences;
        if (save)
            SavesManager.instance.SaveAllowedTypesOfSentences(this.allowedTypesOfSentences);
    }
    public List<string> GetAllowedTypeOfSentences()
    {
        List<String> allowedTypes = new List<string>();

        /*foreach (KeyValuePair<Utils.TypeOfSentence, bool> type in allowedTypesOfSentences)
        {
            if (type.Value == true)
                allowedTypes.Add( (Enum.GetName(typeof(Utils.TypeOfSentence),type.Key)).ToUpper() ); 
        }*/

        foreach (Utils.TypeOfSentence type in allowedTypesOfSentences.Keys)
        {
            if (allowedTypesOfSentences[type] == true)
                allowedTypes.Add((Enum.GetName(typeof(Utils.TypeOfSentence), type)).ToUpper());
        }

        return allowedTypes;
    }

    public void setRandomSentencesAllowed(bool state, bool save)
    {
        this.randomSentencesAllowed = state;
        if (save)
            SavesManager.instance.SaveRandomSencesAllowed(this.randomSentencesAllowed);
    }
    public void setCustomSentencesAllowed(bool state, bool save)
    {
        this.customSentencesAllowed = state;
        if (save)
            SavesManager.instance.SaveCustomSencesAllowed(this.customSentencesAllowed);
    }


    public void setCustomSentences(List<LocalizedText> customSentences, bool save)
    {
        this.sortedCustomSentences = customSentences;
        if (save)
            SavesManager.instance.SaveCustomSentences(this.sortedCustomSentences);
    }
    public void addCustomSentence(LocalizedText sentence, bool save)
    {
        this.sortedCustomSentences.Add(sentence);
        if (save)
            SavesManager.instance.SaveCustomSentences(this.sortedCustomSentences);
    }
    public void addCustomSentence(LocalizedText sentence, int index, bool save)
    {
        this.sortedCustomSentences.Insert(index, sentence);
        if (save)
            SavesManager.instance.SaveCustomSentences(this.sortedCustomSentences);
    }
    public void removeCustomSentence(int sentenceIndex, bool save)
    {
        this.sortedCustomSentences.RemoveAt(sentenceIndex);
        if (save)
            SavesManager.instance.SaveCustomSentences(this.sortedCustomSentences);
    }
    public string getNextCustomSentenceId()
    {
        this.lastIdOfCustomSentence++;
        SavesManager.instance.SaveLastIdOfCustomSentence(this.lastIdOfCustomSentence);
        return Utils.beginingOfCustomSentenceId + (this.lastIdOfCustomSentence);
    }
}