using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfLang : AConfigurationPanel
{
    [SerializeField] private GameObject languagesContainerObject;
    [SerializeField] private GameObject LangButtonPrefab;

    private void Start()
    {
        foreach (string lang in LocalizationManager.instance.GetAllLocalizedLanguagesAndDialects())
        {
            GameObject languageButton = Instantiate(LangButtonPrefab, languagesContainerObject.transform);
            languageButton.GetComponent<ConfLangButton>().configureButton(LocalizationManager.instance.GetLanguageOrDialectName(lang), LocalizationManager.instance.GetLanguageOrDialectImg(lang), lang);
        }

    }

}
