using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DrinkExtreme;

public class ConfTypeSentences : AConfigurationPanel
{
    [SerializeField] private GameObject TypesContainerObject;
    [SerializeField] private GameObject TypeOfSentenceButtonPrefab;

    private void Start()    
    {
        var KeysBuffer = new List<Utils.TypeOfSentence>(DataManager.instance.allowedTypesOfSentences.Keys);

        foreach (var key in KeysBuffer)
        {
            GameObject typeButton = Instantiate(TypeOfSentenceButtonPrefab, TypesContainerObject.transform);
            typeButton.GetComponent<Toggle>().isOn = DataManager.instance.allowedTypesOfSentences[key];
            typeButton.GetComponent<ConfTypeSentencesButton>().configureButton(key);
        }

    }

    public void CheckAndQuitConfTypeSentences()
    {
        if (GameManager.instance.CheckCurrentAllowedTypesOfSentences())
            gameObject.SetActive(false);
        GameManager.instance.NextTurn();
    }
}
