using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DrinkExtreme;

public class GameManager : ASetupManager
{
    public Mode currentMode { get;  private set; }
    private int currentPlayerIndex;

    public static GameManager instance;
    public GameManager()
    {

        Debug.Log("Initializing a new GameManager.");
        if (instance == null)
        {
            instance = this;
            Debug.Log("GameManager initialization successful.");
            Setup();
            currentPlayerIndex = 0;
        }
        else
        {
            Debug.Log("GameManager already exists. New initialization unsuccessful.");
        }

        
    }

    /*private void Update()
    {
        DataManager.instance.timeInCurrentMode += Time.fixedUnscaledDeltaTime;
        Debug.Log(DataManager.instance.timeInCurrentMode);
    }*/

    private void LoadScene(string sceneName)
    {
        //TODO: Fade. Info: https://www.youtube.com/watch?v=Oadq-IrOazg
        SceneManager.LoadScene(sceneName);
        //SceneManager.LoadSceneAsync(sceneName); //If the scene loads laggy you can switch from "LoadScene" to "LoadSceneAsync" (but it mught be slower). See documentation.
    }

    public void LoadScene(Mode mode)
    {
        this.LoadScene(mode.sceneName);
        SetCurrentMode(mode);
    }

    public void SetCurrentMode(Mode mode)
    {
        if (this.currentMode != null)
            if (this.currentMode.modeId != mode.modeId)
            {
                DataManager.instance.timeInLastMode = DataManager.instance.timeInCurrentMode;
                DataManager.instance.timeInCurrentMode = 0;
                
            }
        this.currentMode = mode;
    }

    public void ReloadCurrentScene()
    {
        this.LoadScene(currentMode.sceneName);
    }

    public void nextPlayer()
    {
        currentPlayerIndex++;
        if (currentPlayerIndex > DataManager.instance.players.Count - 1)
        {
            currentPlayerIndex = 0;
        }
    }

    public string GetCurrentPlayer()
    {
        return GetPlayerAfterTurns(0);
    }

    public string GetPlayerAfterTurns(int numberOfAdditionalTurns)
    {
        int idx = currentPlayerIndex;

        for (int i = 0; i < numberOfAdditionalTurns; i++)
        {
            idx++;
            if (idx > DataManager.instance.players.Count - 1)
                idx = 0;
        }
        
        return DataManager.instance.players[idx];
    }

    public List<String> GetPlayers()
    {
        return DataManager.instance.players;
    }

    public string GetRandomPlayer()
    {
        return DataManager.instance.players[UnityEngine.Random.Range(0, DataManager.instance.players.Count)];
    }

    public string GetPlayerNumber(int num)
    {
        return DataManager.instance.players[num];
    }

    public bool CheckCurrentNumberOfPlayersForCurrentMode()
    {
        if (DataManager.instance.players.Count > currentMode.maxNumberOfPlayers && currentMode.maxNumberOfPlayers > 0)
        {
            //Too much players
            GUIManager.instance.OpenInfoPopup(Utils.tooMuchPlayersTextId, "", " " + currentMode.maxNumberOfPlayers.ToString());
            return false;
        }
        else if (DataManager.instance.players.Count < currentMode.minNumberOfPlayers && currentMode.minNumberOfPlayers > 0)
        {
            //Not enough players
            GUIManager.instance.OpenInfoPopup(Utils.notEnoughPlayersTextId, "", " " + currentMode.minNumberOfPlayers.ToString());
            return false;
        }
        return true;
    }


    public bool CheckCurrentAllowedTypesOfSentences()
    {
        if (DataManager.instance.GetAllowedTypeOfSentences().Count <= 0)
        {
            GUIManager.instance.OpenInfoPopup(Utils.notEnoughTypesOfSentences, "", "");
            return false;
        }
        return true;
    }

    public delegate bool NextTurnAction();
    public event NextTurnAction OnNextTurn;
    public void NextTurn()
    {
        if (OnNextTurn != null)
            OnNextTurn();
    }

    public void test()
    {
        GUIManager.instance.OpenLinkPopup("textValorate", Utils.Link.android, "", "");
    }
}
