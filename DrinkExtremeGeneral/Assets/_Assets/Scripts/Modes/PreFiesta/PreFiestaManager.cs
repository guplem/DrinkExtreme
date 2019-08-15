using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using DrinkExtreme;

//TODO: Documentation
public class PreFiestaManager : AModeManager
{

    //private Animator animator; //Maybe necessary
    [SerializeField] private TextMeshProUGUI textZone;
    [SerializeField] private TextMeshProUGUI AddedByUserNote;
    private Localizer textZoneLocalizer;

    public static PreFiestaManager instance;
    private new void Awake()
    {
        base.Awake(); //Mandatory

        Debug.Log("Initializing a new PreFiestaManager.");
        if (instance == null)
        {
            instance = this;
            Debug.Log("PreFiestaManager initialization successful.");
        }
        else
        {
            Debug.LogError("PreFiestaManager already exists. New initialization unsuccessful.");
        }
    }

    //Setup
    private new void Start()
    {
        base.Start(); //Mandatory
        started = true; //Mandatory
        OnEnable(); //Mandatory

        textZoneLocalizer = textZone.GetComponent<Localizer>();

        NextTurn(); //Recommended
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

        LocalizedText localizedText = LocalizedText.GetRandomTextWithIdStartingWith(mode.SentenceIdBeggining, true, true, LocalizationManager.instance.localizedTexts);

        string text = localizedText.getTextAddingTimestamp();

        if (text.StartsWith(Utils.indicatorOfSentenceAddedByUser))
        {
            var splittedText = text.Split(new string[] { Utils.indicatorOfSentenceAddedByUser }, StringSplitOptions.None);
            text = "";
            for (int i = 0; i < splittedText.Length; i++)
            {
                if (i > 1)
                    text += Utils.indicatorOfSentenceAddedByUser;

                text += splittedText[i];
            }
            AddedByUserNote.gameObject.SetActive(true);
        }
        else
        {
            AddedByUserNote.gameObject.SetActive(false);
        }

        textZone.text = text;
        textZoneLocalizer.id = localizedText.id;

        return true;
    }

}