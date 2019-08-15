using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using DrinkExtreme;

public abstract class AModeManager : MonoBehaviour
{
    
    [SerializeField] protected Mode mode;
    [SerializeField] public GameObject sceneCanvas;

    [HideInInspector] public int TurnCounter { get; private set; }
    [HideInInspector] private int CounterToRandomSentence;
    [HideInInspector] private int counterToCustomSentenceInsteadOfRandom;
    [HideInInspector] protected bool started = false;
    [HideInInspector] private Thread childThread;


    public void Awake()
    {
        Debug.Log("# Starting standard AModeManager inicialization.");
        Instantiate(Resources.Load(Utils.prefabsPath + Utils.nameOfGeneralGUIPrefab) as GameObject, sceneCanvas.transform);
        new GameManager();
        TurnCounter = 0;
        if (GameManager.instance.currentMode != mode)
            GameManager.instance.SetCurrentMode(mode);

        Debug.Log("# Standard AModeManager inicialization finished.");
    }

    public static void ThreadTimeCounter()
    {
        try
        {
            Debug.Log("ThreadTimeCounter starts");
            
            while (true)
            {
                Thread.Sleep(Utils.secondsBetweenTimeCountings * 1000);
                DataManager.instance.timeInCurrentMode += Utils.secondsBetweenTimeCountings;

                if (DataManager.instance.timeInCurrentMode%60==0) //Reduce the number of debug messages
                    Debug.Log("ThreadTimeCounter reporting: counted " + DataManager.instance.timeInCurrentMode + " seconds in the current mode");

            }

        }
        catch (ThreadAbortException e)
        {
            Debug.Log("ThreadTimeCounter Abort Exception: " + e.Message);
        }
        finally
        {
            //Do something when the thread is aborted
        }
    }


    public void Start()
    {
        if (mode == null)
            Debug.LogError("Mode not set in the mode manager");

        if (sceneCanvas == null)
            Debug.LogError("SceneCanvas not set in the mode manager");

        //Invert order of line "A" annd "B" (first line B and then line A) to increase the time until the first custom sentence is shown if the random sentences are not allowed by the user's configuration 
        counterToCustomSentenceInsteadOfRandom = Utils.NumberOfRandomSentenceToCustomSentence; //"A"
        SetupNextRandomSentence(); //"B"

        //GameManager.instance.SetCurrentMode(mode);

        if (mode.showChangeDifficultyPanelAtBeginning)
            GUIManager.instance.OpenConfigurationPanel(GUIManager.configurationPanels.ConfNivelPicante);
        if (mode.showPlayersPanelAtBeginning)
            GUIManager.instance.OpenConfigurationPanel(GUIManager.configurationPanels.ConfPlayers);
        if (mode.showTypeOfSentencesPanelAtBeginning)
            GUIManager.instance.OpenConfigurationPanel(GUIManager.configurationPanels.ConfTypeSentences);


        //Create the thread
        ThreadStart childref = new ThreadStart(ThreadTimeCounter);
        childThread = new Thread(childref);
        childThread.Start();
    }

    public void NextTurn()
    {
        GameManager.instance.NextTurn();
    }

    public bool _NextTurn()
    {

        if (!GameManager.instance.CheckCurrentNumberOfPlayersForCurrentMode())
            return false;

        Debug.Log("Next turn");
        TurnCounter++;

        CounterToRandomSentence--;
        if (CounterToRandomSentence <= 0)
        {
            ShowRandomSentence();
            SetupNextRandomSentence();
        }

        return true;
    }

    private void ShowRandomSentence()
    {

        LocalizedText text = null;

        if ((DataManager.instance.customSentencesAllowed && mode.allowCustomSentencesCard) && //The configurations allow custom sentences
            (counterToCustomSentenceInsteadOfRandom <= 0) && //It is time to show a custom sentence
            (DataManager.instance.sortedCustomSentences.Count > 0) && //There are custom sentences created
            (DataManager.instance.numOfCustomSentencesAlreadyShown < DataManager.instance.sortedCustomSentences.Count) ) //Do not repeat custom sentences
        {
            text = LocalizedText.GetRandomTextWithIdStartingWith(Utils.beginingOfCustomSentenceId, true, false, DataManager.instance.customSentences);
            counterToCustomSentenceInsteadOfRandom = Utils.NumberOfRandomSentenceToCustomSentence;
            DataManager.instance.numOfCustomSentencesAlreadyShown++;
        }

        else if (DataManager.instance.randomSentencesAllowed && mode.allowRandomSentencesCard)
        {
            
            text = LocalizedText.GetRandomTextWithIdStartingWith(Utils.beginingOfRandomSentenceId, true, true, LocalizationManager.instance.localizedTexts);
            counterToCustomSentenceInsteadOfRandom--;
        }

        if (text != null)
            GUIManager.instance.ShowRandomCard(text);
    }
    
    private void SetupNextRandomSentence()
    {
        CounterToRandomSentence = UnityEngine.Random.Range(Utils.minNumberOfTurnsToRandomSentence, Utils.maxNumberOfTurnsToRandomSentence+1);
        if (!DataManager.instance.randomSentencesAllowed || !mode.allowRandomSentencesCard)
            counterToCustomSentenceInsteadOfRandom = 0;
    }

    private void OnDestroy()
    {
        childThread.Abort();
    }
}
