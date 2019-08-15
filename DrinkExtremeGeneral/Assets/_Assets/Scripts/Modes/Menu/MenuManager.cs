using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DrinkExtreme;

public class MenuManager : AModeManager
{
    [SerializeField] private GameObject modesButtonsContainer;
    [SerializeField] private GameObject mainMenuModeButtonPrefab;
    [SerializeField] private Mode[] modes;
    [SerializeField] private GameObject modePreviewMenu;
    [SerializeField] private GameObject imagePreviewMenu;
    [SerializeField] private GameObject descriptionTextPreviewMenu;

    private Animator animator;
    private Mode selectedMode;

    public static MenuManager instance;
    private new void Awake()
    {
        base.Awake(); //Mandatory

        Debug.Log("Initializing a new MainMenuManager.");
        if (instance == null)
        {
            instance = this;
            Debug.Log("MainMenuManager initialization successful.");
        }
        else
        {
            Debug.LogError("MainMenuManager already exists. New initialization unsuccessful.");
        }
    }

    //Setup
    private new void Start()
    {
        base.Start(); //Mandatory
        started = true; //Mandatory
        OnEnable(); //Mandatory

        GenerateModeButtons();
        animator = sceneCanvas.GetComponent<Animator>();
        modePreviewMenu.SetActive(false);


        base.NextTurn(); //Recommended
    }

    private void OnEnable()
    {
        if (started)
            GameManager.instance.OnNextTurn += _NextTurn; //Mandatory
    }

    private void OnDisable()
    {
        if (started)
            GameManager.instance.OnNextTurn -= _NextTurn; //Mandatory
    }

    private new bool _NextTurn() //If you want to do a next turn you must execute "NextTurn()" or  "GameManager.instance.NextTurn()".
    {
        if (!base._NextTurn()) return false; //Mandatory

        return true;
    }

    public void GenerateModeButtons()
    {
        Debug.Log("Generating mode buttons");
        while (modesButtonsContainer.transform.childCount > 0)
            foreach (Transform child in modesButtonsContainer.transform)
                DestroyImmediate(child.gameObject);

        RectTransform buttonRectTransform = modesButtonsContainer.GetComponent<RectTransform>(); //To avoid possible errors

        for (int i = 0; i < modes.Length; i++)
        {
            GameObject button = Instantiate(mainMenuModeButtonPrefab, modesButtonsContainer.transform);

            //buttonRectTransform = button.GetComponent<RectTransform>();
            //buttonRectTransform.localPosition = new Vector2(0, (buttonRectTransform.rect.height) * (/*modes.Length/2*/-i));

            button.GetComponent<MenuButtonMode>().mode = modes[i];
            Localizer loc = button.GetComponent<Localizer>();
            loc.id = modes[i].artworkId;

            if (Application.isPlaying)
                loc.Localize();
            
        }

        RectTransform containerRectTransform = modesButtonsContainer.GetComponent<RectTransform>();
        containerRectTransform.sizeDelta = new Vector2(0, (buttonRectTransform.rect.height) * modes.Length);

    }

    public void SelectAndPreviewMode(Mode mode)
    {
        Debug.Log("Previewing mode " + mode.name + " (" + mode.modeId + ")");
        selectedMode = mode;
        modePreviewMenu.SetActive(true);
        animator.SetBool("PreviewMode", true);
        
        //Preview info
        Localizer loc = imagePreviewMenu.GetComponent<Localizer>();
        loc.id = selectedMode.artworkId;
        loc.Localize();

        loc = descriptionTextPreviewMenu.GetComponent<Localizer>();
        loc.id = selectedMode.descriptionId;
        loc.Localize();

        //TODO: (nth) If mode.TypeOfMode is PlayablePaidMode check if the mode is purchased and then, if it is not, do not show th "play button", show a "purchase button" instead (with a "paid mode" indicator)

        //TODO: (nth) If mode.TypeOfMode is ComingSoon do not show th "play button", show a "done"/"go back" button instead with a "comming soon" indicator
    }

    public void QuitModePreview()
    {
        Debug.Log("Quiting preview of mode " + selectedMode.name + " (" + selectedMode.modeId + ")");
        animator.SetBool("PreviewMode", false);
        selectedMode = null;
    }

    public void OpenLink(string type)
    {
        Utils.OpenExternalPredefinedLink(Utils.GetLink(type));
    }

    public void LoadSelectedMode()
    {
        GameManager.instance.LoadScene(selectedMode);
    }
    
}