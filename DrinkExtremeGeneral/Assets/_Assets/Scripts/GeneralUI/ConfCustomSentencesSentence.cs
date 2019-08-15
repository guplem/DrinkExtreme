using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConfCustomSentencesSentence : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI sentenceText;
    private int indexSentenceInCustomSentenceList;
    private ConfCustomSentences confCustomSentences;

    internal void configureSentence(LocalizedText sentence, int indexSentenceInCustomSentenceList, ConfCustomSentences confCustomSentences)
    {
        this.sentenceText.text = sentence.getTextWithoutAddingTimestamp();
        this.indexSentenceInCustomSentenceList = indexSentenceInCustomSentenceList;
        this.confCustomSentences = confCustomSentences;
    }

    public void removeCustomSentence()
    {
        DataManager.instance.removeCustomSentence(indexSentenceInCustomSentenceList, true);
        confCustomSentences.buildList();
    }
}
