using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mode")]
public class Mode : ScriptableObject
{
    [Header("Basic configuration")]
    public string sceneName;
    public string modeId;
    public string modeNameId;
    public string artworkId;
    public string descriptionId;
    public string SentenceIdBeggining;
    public TypeOfMode typeOfMode;
    public Mode modeToGoBack;

    [Header("Players")]
    public int minNumberOfPlayers;
    public int maxNumberOfPlayers;

    [Header("Configuration panels at beginning")]
    public bool showPlayersPanelAtBeginning;
    public bool showTypeOfSentencesPanelAtBeginning;
    public bool showChangeDifficultyPanelAtBeginning;

    [Header("Interruptions")]
    public bool allowRandomSentencesCard;
    public bool allowCustomSentencesCard;

    [Header("Ads")]
    public bool displayBannerAd;
    //var to enable feedback/user sending sentences //TBD







    public enum TypeOfMode
    {
        PlayableFreeMode,
        PlayablePaidMode,
        ComingSoon,
        BuyProAction,
    }
    
}
