using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DrinkExtreme;

public class GUIManager : MonoBehaviour
{
    //[SerializeField] private GameObject GeneralConfMenu, ConfLang, ConfTypeSentences, ConfNivelPicante, ConfCustomSentences, ConfPlayers, ConfMiscelanea, WarningPopup, ConditionPopup, LinkPopup, InfoPopup, GoBackButton;
    [SerializeField] private GameObject GoBackButton, RandomCard, OpenConfigMenuButton, GeneralConfMenu, ConfLang, ConfTypeSentences, ConfNivelPicante, ConfCustomSentences, ConfPlayers, WarningPopup, InfoPopup, LinkPopup, ConditionPopup, ConfMiscelanea, rateAppPopup;
    
    [SerializeField] private Localizer WarningPopupText, ConditionPopupText, LinkPopupText, InfoPopupText;
    [SerializeField] private RandomCard randomCard;
    public static GUIManager instance;
    private Utils.Condition currentCondition;
    private Utils.Link currentLink;

    public void Awake()
    {
        Debug.Log("Initializing a new GUIManager.");        

        // Cada nuevo GUI manager destruye el anterior para facilitar la creación de modos nuevos
        instance = this;

        GoBackButton.SetActive(false);
        RandomCard.SetActive(false);
        OpenConfigMenuButton.SetActive(true);
        GeneralConfMenu.SetActive(false);
        ConfLang.SetActive(false);
        ConfTypeSentences.SetActive(false);
        ConfNivelPicante.SetActive(false);
        ConfCustomSentences.SetActive(false);
        ConfPlayers.SetActive(false);
        WarningPopup.SetActive(false);
        InfoPopup.SetActive(false);
        LinkPopup.SetActive(false);
        ConditionPopup.SetActive(false);
        ConfMiscelanea.SetActive(false);
        rateAppPopup.SetActive(false);

        Debug.Log("GUIManager initialization successful.");
    }

    private void Start()
    {
        GoBackButton.SetActive(GameManager.instance.currentMode.modeToGoBack != null);
    }

    public void GoToBackMode()
    {
        GameManager.instance.LoadScene(GameManager.instance.currentMode.modeToGoBack);
    }


    public enum configurationPanels
    {
        GeneralConfMenu,
        ConfLang,
        ConfTypeSentences,
        ConfNivelPicante,
        ConfCustomSentences,
        ConfPlayers,
        ConfMiscelanea
    }

    public void OpenConfigurationPanel(string panel)
    {
        switch (panel.ToUpper())
        {
            case "GENERALCONFMENU": OpenConfigurationPanel(configurationPanels.GeneralConfMenu); break;
            case "CONFLANG": OpenConfigurationPanel(configurationPanels.ConfLang); break;
            case "CONFTYPESENTENCES": OpenConfigurationPanel(configurationPanels.ConfTypeSentences); break;
            case "CONFNIVELPICANTE": OpenConfigurationPanel(configurationPanels.ConfNivelPicante); break;
            case "CONFCUSTOMSENTENCES": OpenConfigurationPanel(configurationPanels.ConfCustomSentences); break;
            case "CONFPLAYERS": OpenConfigurationPanel(configurationPanels.ConfPlayers); break;
            case "CONFMISCELANEA": OpenConfigurationPanel(configurationPanels.ConfMiscelanea); break;
            default: Debug.LogError("Unknown panel wanted to be opened: " + panel); break;
        }
    }

    public void OpenConfigurationPanel(configurationPanels panel)
    {
        switch (panel)
        {
            case configurationPanels.GeneralConfMenu: GeneralConfMenu.SetActive(true); break;
            case configurationPanels.ConfLang: ConfLang.SetActive(true); break;
            case configurationPanels.ConfTypeSentences: ConfTypeSentences.SetActive(true); break;
            case configurationPanels.ConfNivelPicante: ConfNivelPicante.SetActive(true); break;
            case configurationPanels.ConfCustomSentences: ConfCustomSentences.SetActive(true); break;
            case configurationPanels.ConfPlayers: ConfPlayers.SetActive(true); break;
            case configurationPanels.ConfMiscelanea: ConfMiscelanea.SetActive(true); break;
            default: Debug.LogError("Unnexpected panel to be opened"); break;
        }
    }

    public void OpenConfigurationPanels(List<configurationPanels> panels)
    {
        foreach (configurationPanels panel in panels)
        {
            OpenConfigurationPanel(panel);
        }
    }

    public void OpenWarningPopup(string textId, string textAtBeggining, string textAtEnd) //Show text on screen to warn about something
    {
        WarningPopup.SetActive(true);
        WarningPopupText.id = textId;
        WarningPopupText.additionalTextToAddAtTheBegginingOfTheText = textAtBeggining;
        WarningPopupText.additionalTextToAddAtTheEndOfTheText = textAtEnd;
        WarningPopupText.Localize();
    }

    public void OpenInfoPopup(string textId, string textAtBeggining, string textAtEnd) //Show text on the screen
    {
        InfoPopup.SetActive(true);
        InfoPopupText.id = textId;
        InfoPopupText.additionalTextToAddAtTheBegginingOfTheText = textAtBeggining;
        InfoPopupText.additionalTextToAddAtTheEndOfTheText = textAtEnd;
        InfoPopupText.Localize();
    }
    
    public void OpenLinkPopup(string textId, Utils.Link link, string textAtBeggining, string textAtEnd) //Opens a panel that opens the link if accepted
    {
        LinkPopup.SetActive(true);
        LinkPopupText.id = textId;
        LinkPopupText.additionalTextToAddAtTheBegginingOfTheText = textAtBeggining;
        LinkPopupText.additionalTextToAddAtTheEndOfTheText = textAtEnd;
        LinkPopupText.Localize();
        currentLink = link;
    }

    public void OpenRateAppPopup()
    {
        Debug.Log("Opening rate app popup");
        rateAppPopup.SetActive(true);
        currentLink = Utils.GetLink("RATE");
        DataManager.instance.setRateAppPopupShown(true, false);
    }

    public void DeclineOpenRateAppPopup()
    {
        if (currentLink == Utils.GetLink("RATE"))
        {
            currentLink = Utils.Link.None;
            rateAppPopup.SetActive(false);
            DataManager.instance.setRateAppPopupShown(true, false);
        }
        else
        {
            Debug.LogError("Declined to open the link to rate the app but another link was set as current. Maybe another link popup was activated causing the error.");
        }
    }

    public void AcceptOpenRateApp()
    {

        if (currentLink == Utils.GetLink("RATE"))
        {
            Utils.OpenExternalPredefinedLink(currentLink);
            currentLink = Utils.Link.None;
            rateAppPopup.SetActive(false);
            DataManager.instance.setRateAppPopupShown(true, true);
        }
        else
        {
            Debug.LogError("Accepted to open the link to rate the app but another link was set as current. Maybe another link popup was activated causing the error.");
        }
    }

    public void AcceptOpenCurrentLink()
    {
        Utils.OpenExternalPredefinedLink(currentLink);
        currentLink = Utils.Link.None;
        LinkPopup.SetActive(false);
    }

    public void DeclineOpenCurrentLink()
    {
        currentLink = Utils.Link.None;
        LinkPopup.SetActive(false);
    }

    public void OpenConditionPopup(string textId, Utils.Condition condition) //Asks for something and saves the result
    {
        ConditionPopup.SetActive(true);
        ConditionPopupText.id = textId;
        ConditionPopupText.Localize();
        currentCondition = condition;
    }

    public void AcceptCurrentConditionPopup()
    {
        SavesManager.instance.SaveCondition(currentCondition, true);
        currentCondition = Utils.Condition.None;
        ConditionPopup.SetActive(false);
    }

    public void DeclineCurrentConditionPopup()
    {
        SavesManager.instance.SaveCondition(currentCondition, false);
        currentCondition = Utils.Condition.None;
        ConditionPopup.SetActive(false);
    }

    public void ShowRandomCard(LocalizedText text)
    {
        randomCard.OpenCard(text);
    }


}
