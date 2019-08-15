using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DrinkExtreme;

public class ConfTypeSentencesButton : MonoBehaviour
{


    private Utils.TypeOfSentence typeOfSentence;
    [SerializeField] private GameObject typeOfSentenceText;


    public void configureButton(Utils.TypeOfSentence typeOfSentence)
    {
        this.typeOfSentence = typeOfSentence;
        typeOfSentenceText.GetComponent<Localizer>().configure(Utils.GetTypeOfSentenceStringOf(typeOfSentence), Localizer.ComponentToLocalize.TextMeshPro, true, true);
    }

    public void buttonToggled(bool state)
    {
        DataManager.instance.setStateOfTypeOfSentence(typeOfSentence, state, true);
    }

}
