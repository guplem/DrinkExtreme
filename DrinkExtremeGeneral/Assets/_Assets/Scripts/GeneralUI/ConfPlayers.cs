using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DrinkExtreme;

public class ConfPlayers : AConfigurationPanel
{
    [SerializeField] private GameObject newPlayerInputFieldObject;
    [SerializeField] private GameObject playersContainerObject;
    [SerializeField] private GameObject playerPrefab;
    private string newPlayerName;

    private void Start()
    {
        buildList();
    }

    public void buildList()
    {
        Debug.Log("Building player list: " + string.Join (", ", DataManager.instance.players.ToArray() ) );

        while (playersContainerObject.transform.childCount < DataManager.instance.players.Count)
        {
            Instantiate(playerPrefab, playersContainerObject.transform);
        }

        while (playersContainerObject.transform.childCount > DataManager.instance.players.Count)
        {
            int randomNumber = Random.Range(0, playersContainerObject.transform.childCount);
            DestroyImmediate(playersContainerObject.transform.GetChild(randomNumber).gameObject);
        }

        int playerIndex = 0;
        foreach (Transform player in playersContainerObject.transform)
        {
            string playerName = DataManager.instance.players[playerIndex];
            player.GetComponent<ConfPlayersPlayer>().configurePlayer(playerName, playerIndex, this);
            playerIndex++;
        }
    }

    public void setNewPlayerName(string newName)
    {
        this.newPlayerName = newName;
    }

    public void addNewPlayer(string newPlayerName)
    {
        if (!string.IsNullOrEmpty(newPlayerName))
        {
            this.newPlayerName = newPlayerName;
            addNewPlayer();
        }
    }

    public void addNewPlayer()
    {
        if (!string.IsNullOrEmpty(this.newPlayerName))
        {
            if (DataManager.instance.players.Contains(newPlayerName))
            {
                GUIManager.instance.OpenWarningPopup(Utils.repeatedPlayerText, "", "");
                return;
            }
            else
            {
                DataManager.instance.addPlayer(newPlayerName, true);
                buildList();
                newPlayerInputFieldObject.GetComponent<TMP_InputField>().text = "";
            }
        }
    }

    public void CheckAndQuitConfPlayers()
    {
        if (GameManager.instance.CheckCurrentNumberOfPlayersForCurrentMode())
            gameObject.SetActive(false);
        GameManager.instance.NextTurn();
    }
}
