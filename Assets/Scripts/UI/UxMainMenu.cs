using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UxMainMenu : UxPanel {
    [Header("UI Reference")]
    public Button playButton;
    public Toggle soloToggle;
    public Toggle coopToggle;
    public Button optionsButton;
    public Button creditsButton;
    public Button quitButton;

    // Fmod UI sounds
    string SelectSound = "event:/UI/Select";
    string SelectSoundSingle = "event:/UI/Mouseover";
    FMOD.Studio.EventInstance lowpassfilter_SnapshotEv;


    [Header("Prefabs")]
    public GameObject optionsPrefab;
    public GameObject creditsPrefab;

    private GameManagerScript gameManager;

    public void Start() {
        gameManager = GameManagerScript.instance;
        // setup button callbacks
        playButton.onClick.AddListener(OnPlayClick);
        soloToggle.onValueChanged.AddListener(OnSoloChanged);
        optionsButton.onClick.AddListener(OnOptionsClick);
        creditsButton.onClick.AddListener(OnCreditsClick);
        quitButton.onClick.AddListener(OnQuitClick);
        SetState();
        Display();

    }

    // set state of UI elements to match game config settings
    public void SetState() {
        if (gameManager != null) {
            soloToggle.isOn = (gameManager.PlayerCount == 1);
            coopToggle.isOn = (gameManager.PlayerCount != 1);
        }
    }

    public void OnPlayClick() {
        // instantiate options prefab (under canvas)
        //var panelGo = Instantiate(optionsPrefab, UxUtil.GetCanvas().gameObject.transform);
        //var uxPanel = panelGo.GetComponent<UxPanel>();
        //uxPanel.onDoneEvent.AddListener(OnSubPanelDone);
        Hide();
        if (gameManager != null) {
            gameManager.StartTheGame();
        }

        FMODUnity.RuntimeManager.PlayOneShot(SelectSound, transform.position);

    }

    public void OnSoloChanged(bool value) {
        if (gameManager != null) {
            gameManager.PlayerCount = (value) ? 1 : 2;
        }
    }

    public void OnOptionsClick() {
        // instantiate the options panel prefab
        var panelGo = Instantiate(optionsPrefab, UxUtil.GetCanvas().gameObject.transform);
        // setup a callback, so when the sub menu/panel is done, we display the current panel again
        var uxPanel = panelGo.GetComponent<UxPanel>();
        uxPanel.onDoneEvent.AddListener(Enable);
        // now hide the current panel
        Disable();
        Debug.Log("hiding options");
        FMODUnity.RuntimeManager.PlayOneShot(SelectSound, transform.position);

    }

    public void OnCreditsClick() {
        // instantiate credits prefab (under canvas)
        //var panelGo = Instantiate(creditsPrefab, UxUtil.GetCanvas().gameObject.transform);
        //var uxPanel = panelGo.GetComponent<UxPanel>();
        //uxPanel.onDoneEvent.AddListener(OnSubPanelDone);
        //Hide();
        FMODUnity.RuntimeManager.PlayOneShot(SelectSound, transform.position);
    }

    public void OnQuitClick() {
        Application.Quit();
    }
}