using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Documentation
public class TemplateManager : AModeManager
{
    //Maybe necessary
    //private Animator animator; 
    //[SerializeField] private TextMeshProUGUI textZone;
    //[SerializeField] private TextMeshProUGUI AddedByUserNote;
    //private Localizer textZoneLocalizer;

    //Pre-Setup
    public static TemplateManager instance;
    private new void Awake()
    {
        base.Awake(); //Mandatory

        Debug.Log("Initializing a new TemplateManager.");
        if (instance == null)
        {
            instance = this;
            Debug.Log("TemplateManager initialization successful.");
        }
        else
        {
            Debug.LogError("TemplateManager already exists. New initialization unsuccessful.");
        }
    }
    

    //Setup
    private new void Start()
    {
        base.Start(); //Mandatory
        started = true; //Mandatory
        OnEnable(); //Mandatory

        //EXAMPLES:
        //animator = sceneCanvas.GetComponent<Animator>();
        //textZoneLocalizer = textZone.GetComponent<Localizer>();

        base.NextTurn(); //Recommended
    }

    private void OnEnable() //Mandatory
    {
        if (started)
            GameManager.instance.OnNextTurn += _NextTurn; //Mandatory
    }

    private void OnDisable() //Mandatory
    {
        if (started)
            GameManager.instance.OnNextTurn -= _NextTurn; //Mandatory
    }

    //If you want to do a next turn you must execute "NextTurn()" or  "GameManager.instance.NextTurn()".
    private new bool _NextTurn() //Use to perform action in each "nex turn" call
    {
        if (!base._NextTurn()) return false; //Mandatory

        //EXAMPLES:
        //AddedByUserNote.gameObject.SetActive(true/false);
        //textZone.text = text;
        //textZoneLocalizer.id = id;
        //GameManager.instance.nextPlayer();

        return true;
    }





}