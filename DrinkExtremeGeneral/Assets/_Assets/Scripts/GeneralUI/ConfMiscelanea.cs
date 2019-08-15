using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfMiscelanea : MonoBehaviour
{
 
    [SerializeField] private GameObject RandomSentencesToggleObject;
    [SerializeField] private GameObject CustomSentencesToggleObject;

    private void Start()
    {
        RandomSentencesToggleObject.GetComponent<Toggle>().isOn = DataManager.instance.randomSentencesAllowed;
        CustomSentencesToggleObject.GetComponent<Toggle>().isOn = DataManager.instance.customSentencesAllowed;
    }

    public void EnableRandomSentences(bool state)
    {
        DataManager.instance.setRandomSentencesAllowed(state, true);
    }

    public void EnableCustomSentences(bool state)
    {
        DataManager.instance.setCustomSentencesAllowed(state, true);
    }

}
