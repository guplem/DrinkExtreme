using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConfCustomSentences : AConfigurationPanel
{
    [SerializeField] private GameObject newCustomSentenceInputFieldObject;
    [SerializeField] private GameObject customSentencesContainerObject;
    [SerializeField] private GameObject customSentenceInConfigPrefab;
    private string newSentenceText;

    private void Start()
    {
        buildList();
    }

    public void buildList()
    {
        Debug.Log("Building custom sentence list.");
        while (customSentencesContainerObject.transform.childCount < DataManager.instance.sortedCustomSentences.Count)
        {
            Instantiate(customSentenceInConfigPrefab, customSentencesContainerObject.transform);
        }

        while (customSentencesContainerObject.transform.childCount > DataManager.instance.sortedCustomSentences.Count)
        {
            int randomNumber = Random.Range(0, customSentencesContainerObject.transform.childCount);
            DestroyImmediate(customSentencesContainerObject.transform.GetChild(randomNumber).gameObject);
        }

        int customSentenceIndex = 0;
        foreach (Transform sentence in customSentencesContainerObject.transform)
        {
            LocalizedText customSentence = DataManager.instance.sortedCustomSentences[customSentenceIndex];
            sentence.GetComponent<ConfCustomSentencesSentence>().configureSentence(customSentence, customSentenceIndex, this);
            customSentenceIndex++;
        }
    }

    public void setNewSentenceText(string newSentenceText)
    {
        this.newSentenceText = newSentenceText;
    }

    public void addNewCustomSentence(string newSentenceText)
    {
        if (!string.IsNullOrEmpty(newSentenceText))
        {
            this.newSentenceText = newSentenceText;
            addNewCustomSentence();
        }
    }

    public void addNewCustomSentence()
    {
        if (!string.IsNullOrEmpty(this.newSentenceText))
        {
            string id = DataManager.instance.getNextCustomSentenceId();
            DataManager.instance.addCustomSentence(new LocalizedText(id, newSentenceText), true);
            buildList();
            newCustomSentenceInputFieldObject.GetComponent<TMP_InputField>().text = "";
        }
    }
}
