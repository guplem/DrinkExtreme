using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RandomCard : MonoBehaviour
{
    [SerializeField] private Animator animatorComponent;
    [SerializeField] private GameObject card;
    [SerializeField] private GameObject doneButton;
    [SerializeField] private GameObject backCard;
    [SerializeField] private TextMeshProUGUI textZone;
    private Image bg;
    private Localizer textZoneLocalizer;

    private void Start()
    {
        textZoneLocalizer = textZone.GetComponent<Localizer>();
        bg = GetComponent<Image>();

        card.SetActive(false);
        bg.raycastTarget = false;
        bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, 0);
    }

    public void OpenCard(LocalizedText text)
    {
        //Ensure the object is active
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        //Ensure execution of start component
        if (textZoneLocalizer == null || textZoneLocalizer == null)
            Start();

        //Setup card
        textZone.text = text.getTextAddingTimestamp();
        textZoneLocalizer.id = text.id;

        animatorComponent.SetTrigger("Open");
    }

    public void FlipCard()
    {
        animatorComponent.SetTrigger("Flip");
    }

    public void CloseCard()
    {
        animatorComponent.SetTrigger("Close");
    }
}
