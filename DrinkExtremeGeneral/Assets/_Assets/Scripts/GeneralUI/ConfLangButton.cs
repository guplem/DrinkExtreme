using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfLangButton : MonoBehaviour
{

    private string languageId = "";
    [SerializeField] private TextMeshProUGUI languageNameText;
    [SerializeField] private Image languageButtonImageComponent;


    public void configureButton(string languageName, Sprite languageImage, string languageId)
    {
        this.languageId = languageId;
        languageButtonImageComponent.sprite = languageImage;
        languageNameText.text = languageName;
    }

    public void selectLanguage()
    {
        if (!string.IsNullOrEmpty(languageId))
        {
            LocalizationManager.instance.LoadLanguage(languageId);
        }
    }

}
