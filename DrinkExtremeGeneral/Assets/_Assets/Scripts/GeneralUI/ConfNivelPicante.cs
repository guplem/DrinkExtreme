using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DrinkExtreme;

public class ConfNivelPicante : AConfigurationPanel
{
    [SerializeField] private GameObject nivelPicanteSlider;
    private int valNivelPicante;

    private void Start()
    {
        nivelPicanteSlider.GetComponent<Slider>().value = DataManager.instance.difficulty;
        nivelPicanteSlider.GetComponent<Slider>().minValue = Utils.minDifficulty;
        nivelPicanteSlider.GetComponent<Slider>().maxValue = Utils.maxDifficulty;
    }

    public void changeDifficulty(float newDifficulty)
    {
        if (newDifficulty > Utils.maxDifficulty)
        {
            Debug.LogError("The desired difficulty is bigger than the allowed value: " + newDifficulty + ">" + Utils.maxDifficulty);
            return;
        }
        else if (newDifficulty < Utils.minDifficulty)
        {
            Debug.LogError("The desired difficulty is smaller than the allowed value: " + newDifficulty + "<" + Utils.minDifficulty);
            return;
        }
        else
        {
            DataManager.instance.setDifficulty(Mathf.RoundToInt(newDifficulty), true);
            return;
        }
    }
}
