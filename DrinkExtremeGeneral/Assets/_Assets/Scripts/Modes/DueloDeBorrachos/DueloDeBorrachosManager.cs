using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

//TODO: Documentation
public class DueloDeBorrachosManager : AModeManager
{

    [SerializeField] private BoardDDB[] boardsOrderedByPlayerNumber;
    private BoardDDB properBoard;
    private int minReductionPerSecond = 100, maxReductionPerSecond = 300, counterInitialValue = 1000, currentReductionPerSecond, playersReady;
    private int[] counters;
    private bool[] countersEnabled;
    private bool counterRunning;


    //Pre-Setup
    public static DueloDeBorrachosManager instance;
    private new void Awake()
    {
        base.Awake(); //Mandatory

        Debug.Log("Initializing a new DueloDeBorrachosManager.");
        if (instance == null)
        {
            instance = this;
            Debug.Log("DueloDeBorrachosManager initialization successful.");
        }
        else
        {
            Debug.LogError("DueloDeBorrachosManager already exists. New initialization unsuccessful.");
        }
    }
    

    //Setup
    private new void Start()
    {
        base.Start(); //Mandatory
        started = true; //Mandatory
        OnEnable(); //Mandatory

        base.NextTurn(); //Recommended
    }

    private void OnEnable() //Mandatory
    {
        if (started)
            GameManager.instance.OnNextTurn += _NextTurn; //Mandatory
    }

    private void OnDisable() //Mandatory
    {
        if (started)
            GameManager.instance.OnNextTurn -= _NextTurn; //Mandatory
    }

    //If you want to do a next turn you must execute "NextTurn()" or  "GameManager.instance.NextTurn()".
    private new bool _NextTurn() //Use to perform action in each "nex turn" call
    {
        if (!base._NextTurn()) return false; //Mandatory

        SetupProperBoard();

        return true;
    }

    public void SetupProperBoard()
    {
        foreach (var board in boardsOrderedByPlayerNumber)
            board.gameObject.SetActive(false);

        properBoard = boardsOrderedByPlayerNumber[GameManager.instance.GetPlayers().Count - 2];

        properBoard.gameObject.SetActive(true);

        for (int p = 0; p < properBoard.GetComponent<BoardDDB>().playerNames.Length; p++)
            properBoard.GetComponent<BoardDDB>().playerNames[p].text = GameManager.instance.GetPlayerNumber(p);

        counters = new int[GameManager.instance.GetPlayers().Count];
        countersEnabled = new bool[counters.Length];
        for (int c = 0; c < counters.Length; c++)
        {
            countersEnabled[c] = false;
            counters[c] = 1000;
            properBoard.marcadorText[c].text = counters[c].ToString();
        }

        playersReady = 0;
        counterRunning = false;
        currentReductionPerSecond = UnityEngine.Random.Range(minReductionPerSecond, maxReductionPerSecond);
    }

    private void Update()
    {
        //Update the text
        if (counterRunning)
            for (int c = 0; c < counters.Length; c++)
                if (countersEnabled[c])
                    properBoard.marcadorText[c].text = counters[c].ToString();
    }

    private void FixedUpdate()
    {
        //Update the counter
        if (counterRunning)
            for (int c = 0; c < counters.Length; c++)
                if (countersEnabled[c])
                    counters[c] -= Mathf.RoundToInt(currentReductionPerSecond * Time.fixedDeltaTime);
    }

    public void PlayerFingerDown()
    {
        if (!counterRunning)
        {
            counterRunning = playersReady >= GameManager.instance.GetPlayers().Count;
            playersReady++;
            if (playersReady >= counters.Length)
            {
                counterRunning = true;
                for (int c = 0; c < counters.Length; c++)
                    countersEnabled[c] = true;
            }
        }
    }

    public void PlayerFingerUp(int playerNumber)
    {
        playersReady--;
        if (!counterRunning)
        {
            if (playersReady < 0)
                playersReady = 0;
        }
        else
        {
            countersEnabled[playerNumber - 1] = false;
            if (playersReady <= 0)
            {
                counterRunning = false;
                FinishRound();
            }
        }
    }

    private void FinishRound()
    {
        int playersToCheck = counters.Length;
        int[] playerOrder = new int[counters.Length];

        for (int p = 0; p < counters.Length; p++)
        {
            int lowerPlayer = p;
            int lowerScore = counterInitialValue;
            for (int c = 0; c < counters.Length; c++)
            {
                if (counters[c] < lowerScore)
                {
                    lowerScore = counters[c];
                    lowerPlayer = c;
                }
            }
            playerOrder[p] = lowerPlayer;
        }

        String result = "";
        for (int r = 0; r < playerOrder.Length; r++)
        {
            result += GameManager.instance.GetPlayerNumber(r) + ": " + LocalizationManager.instance.GetLocalizedTextOf("DDB"+(r+1).ToString(), false) + (r+1 >= playerOrder.Length? "" : "\n\n");
        }

        GUIManager.instance.OpenInfoPopup("", result, "");

        NextTurn();
    }





}