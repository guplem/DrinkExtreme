using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DrinkExtreme;

public class Localizer : MonoBehaviour
{
    public enum ComponentToLocalize
    {
        TextMeshPro = 0,
        Image = 1
    };


    public ComponentToLocalize componentToLocalize;
    [Space(20)]
    public string additionalTextToAddAtTheEndOfTheText;
    public string id;
    public string additionalTextToAddAtTheBegginingOfTheText;
    [Space(20)]
    public bool automaticallyLocalizeOnEnable = true;
    public bool registerTimestampAtLocalize;

    private string currentLanguage = "";
    private bool started = false;


    private void Start()
    {
        started = true;
        OnEnable();
    }


    private void OnEnable()
    {
        if (started)
        {
            LocalizationManager.OnLocalizeAllAction += Localize;

            if (automaticallyLocalizeOnEnable)
                if (currentLanguage != DataManager.instance.language)
                    Localize();
        }
    }

    private void OnDisable()
    {
        if (started)
            LocalizationManager.OnLocalizeAllAction -= Localize;
    }

    public void configure(string id, ComponentToLocalize componentToLocalize, bool automaticallyLocalizeOnEnable, bool forceLocalizeAfterConfiguration)
    {
        this.id = id;
        this.componentToLocalize = componentToLocalize;
        this.automaticallyLocalizeOnEnable = automaticallyLocalizeOnEnable;
        if (forceLocalizeAfterConfiguration)
            Localize();
    }

    public void Localize()
    {
        if (string.IsNullOrEmpty(id))
        {
            Debug.LogWarning("Trying to localize the object '" + gameObject.name + "' but the 'id' in the 'Localizer' component is null or empty (may be intended)");
            //return;
        }

        if (!id.StartsWith(Utils.beginingOfCustomSentenceId))
        {
            currentLanguage = DataManager.instance.language;

            switch (componentToLocalize)
            {
                case ComponentToLocalize.TextMeshPro:
                    GetComponent<TextMeshProUGUI>().text = additionalTextToAddAtTheBegginingOfTheText + LocalizationManager.instance.GetLocalizedTextOf(id, registerTimestampAtLocalize) + additionalTextToAddAtTheEndOfTheText;
                    break;
                case ComponentToLocalize.Image:
                    GetComponent<Image>().sprite = LocalizationManager.instance.GetLocalizedSpriteOf(id, registerTimestampAtLocalize);
                    break;
                default:
                    break;
            }
        }
        
    }

}