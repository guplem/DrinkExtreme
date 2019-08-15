using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Text.RegularExpressions;
using DrinkExtreme;

//TODO: Documentation
public class VerdadOAtrevimientoManager : AModeManager
{

    [SerializeField] private GameObject gameZone;
    [SerializeField] private TextMeshProUGUI textZone;
    private Localizer textZoneLocalizer;

    [SerializeField] private Localizer nextPlayerText;
    [SerializeField] private Localizer currentPlayerText;

    [SerializeField] private GameObject gameZoneSelection;
    [SerializeField] private Localizer textZoneSelection;

    [SerializeField] private GameObject nextButton;
    [SerializeField] private TextMeshProUGUI addedByUserNote;
    private Localizer addedByUserNoteLocalizer;

    [SerializeField] private GameObject buttonsSelection;
        

    public enum VoaTypeOfSentence
    {
        verdad,
        atrevimiento
    }

    private static string verdadSentenceIdIndicator = "_V";
    private static string atrevimientoSentenceIdIndicator = "_A";
    private static string playingTextId = "Playing";
    private static string nextPlayerTextId = "NextPlayer";


    public static VerdadOAtrevimientoManager instance;
    private new void Awake()
    {
        base.Awake(); //Mandatory

        Debug.Log("Initializing a new VerdadOAtrevimientoManager.");
        if (instance == null)
        {
            instance = this;
            Debug.Log("VerdadOAtrevimientoManager initialization successful.");
        }
        else
        {
            Debug.LogError("VerdadOAtrevimientoManager already exists. New initialization unsuccessful.");
        }
    }


    private new void Start()
    {
        base.Start(); //Mandatory
        started = true; //Mandatory
        OnEnable(); //Mandatory

        textZoneLocalizer = textZone.GetComponent<Localizer>();

        NextTurn();
    }


    private void OnEnable()  //Mandatory
    {
        if (started)
            GameManager.instance.OnNextTurn += _NextTurn; //Mandatory
    }

    private void OnDisable() //Mandatory
    {
        if (started)
            GameManager.instance.OnNextTurn -= _NextTurn; //Mandatory
    }

    private new bool _NextTurn() //If you want to do a next turn you must execute "NextTurn()" or  "GameManager.instance.NextTurn()".
    {
        if (!base._NextTurn()) return false; //Mandatory

        GameManager.instance.nextPlayer();

        gameZone.SetActive(false);
        nextButton.SetActive(false);
        gameZoneSelection.SetActive(true);
        buttonsSelection.SetActive(true);


        currentPlayerText.id = playingTextId;
        currentPlayerText.additionalTextToAddAtTheEndOfTheText = " " + GameManager.instance.GetCurrentPlayer();
        currentPlayerText.Localize();

        nextPlayerText.id = nextPlayerTextId;
        nextPlayerText.additionalTextToAddAtTheEndOfTheText = " " + GameManager.instance.GetPlayerAfterTurns(1);
        nextPlayerText.Localize();

        textZoneSelection.additionalTextToAddAtTheBegginingOfTheText = GameManager.instance.GetCurrentPlayer() + " ";
        textZoneSelection.Localize();

        return true;
    }

    public void SelectTypeTruth()
    {
        SelectType(VoaTypeOfSentence.verdad);
    }

    public void SelectTypeDare()
    {
        SelectType(VoaTypeOfSentence.atrevimiento);
    }

    public void SelectType(VoaTypeOfSentence type)
    {
        //Display proper elements
        gameZone.SetActive(true);
        nextButton.SetActive(true);
        gameZoneSelection.SetActive(false);
        buttonsSelection.SetActive(false);

        string sentenceIdBeggining = mode.SentenceIdBeggining;

        switch (type)
        {
            case VoaTypeOfSentence.verdad:
                sentenceIdBeggining += verdadSentenceIdIndicator;
                break;
            case VoaTypeOfSentence.atrevimiento:
                sentenceIdBeggining += atrevimientoSentenceIdIndicator;
                break;
        }


        LocalizedText localizedText = LocalizedText.GetRandomTextWithIdStartingWith(sentenceIdBeggining, true, true,LocalizationManager.instance.localizedTexts);

        string text = localizedText.getTextAddingTimestamp();

        //Activate or deactivat the "added by user" note
        if (text.StartsWith(Utils.indicatorOfSentenceAddedByUser))
        {
            var splittedTextUserContribution = text.Split(new string[] { Utils.indicatorOfSentenceAddedByUser }, StringSplitOptions.None);
            text = "";
            for (int i = 0; i < splittedTextUserContribution.Length; i++)
            {
                if (i > 1)
                    text += Utils.indicatorOfSentenceAddedByUser;

                text += splittedTextUserContribution[i];
            }
            addedByUserNote.gameObject.SetActive(true);
        }
        else
        {
            addedByUserNote.gameObject.SetActive(false);
        }

        //Put player's names in text
        List<int> PlayerNumbers = new List<int>();
        int rndNum = 0;
        var splittedTextPlayers = text.Split(new string[] { Utils.indicatorOfPlayerNameInSentence }, StringSplitOptions.None);
        text = "";
        for (int i = 0; i < splittedTextPlayers.Length-1; i++)
        {
            do {
                rndNum = UnityEngine.Random.Range(0, GameManager.instance.GetPlayers().Count);
            } while (PlayerNumbers.Contains(rndNum));
            PlayerNumbers.Add(rndNum);

            if (i < splittedTextPlayers.Length - 1)
                text += splittedTextPlayers[i];
            text += GameManager.instance.GetPlayerNumber(rndNum);   
        }
        text += splittedTextPlayers[splittedTextPlayers.Length - 1];


        textZone.text = text;
        textZoneLocalizer.id = localizedText.id;
    }




}