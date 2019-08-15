using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfPlayersPlayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    private int indexInPlayerList;
    private ConfPlayers confPlayers;

    public void movePlayer(int positions)
    {
        int newIndex = indexInPlayerList + positions;
        string player = DataManager.instance.players[indexInPlayerList];

        DataManager.instance.removePlayer(indexInPlayerList, false);

        //if (newIndex > indexInPlayerList) newIndex--; // the actual index could have shifted due to the removal

        if (newIndex < 0)
        {
            DataManager.instance.addPlayer(player, true);
        }
        else if (newIndex > DataManager.instance.players.Count)
        {
            DataManager.instance.addPlayer(player, 0, true);
        }
        else
        {
            DataManager.instance.addPlayer(player, newIndex, true);
        }

        confPlayers.buildList();
    }

    internal void configurePlayer(string playerName, int indexInPlayerList, ConfPlayers confPlayers)
    {
        playerNameText.text = playerName;
        this.indexInPlayerList = indexInPlayerList;
        this.confPlayers = confPlayers;
    }

    public void removePlayer()
    {
        DataManager.instance.removePlayer(indexInPlayerList, true);
        confPlayers.buildList();
    }
}
