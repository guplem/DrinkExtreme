using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonMode : MonoBehaviour
{

    [HideInInspector] public Mode mode;

    public void PreviewMode()
    {
        switch (mode.typeOfMode)
        {
            case Mode.TypeOfMode.PlayableFreeMode:
                MenuManager.instance.SelectAndPreviewMode(mode);
                break;
            case Mode.TypeOfMode.ComingSoon:
                MenuManager.instance.SelectAndPreviewMode(mode);
                Debug.Log("Clicking on a mode that is not avaliable yet.");
                break;
            case Mode.TypeOfMode.BuyProAction:
                Debug.Log("There are no purchases in this game's version");
                break;
            case Mode.TypeOfMode.PlayablePaidMode:
                MenuManager.instance.SelectAndPreviewMode(mode);
                Debug.LogWarning("Playable Paid Mode not implemented yet.");
                break;
            default:
                Debug.LogWarning("Unexpected typeOfMode by " + mode.name + " (" + mode + ")" );
                break;
        }

    }

    private void Start()
    {
        //Do not show BuyPro button if the purchase has been already made
        if (mode.typeOfMode == Mode.TypeOfMode.BuyProAction)
            if (DataManager.instance.showAds == false)
                Destroy(gameObject);

    }
}