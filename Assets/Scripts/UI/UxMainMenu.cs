using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UxMainMenu : UxPanel {
    [Header("UI Reference")]
    public Button playButton;
    public Toggle soloToggle;
    public Toggle coopToggle;
    public Button optionsButton;
    public Button creditsButton;
    public Button quitButton;

    [Header("Prefabs")]
    public GameObject optionsPrefab;
    public GameObject creditsPrefab;

    [Header("State Variables")]
    public GameConfig gameConfig;

    public void Start() {
        // setup button callbacks
        playButton.onClick.AddListener(OnPlayClick);
        soloToggle.onValueChanged.AddListener(OnSoloChanged);
        coopToggle.onValueChanged.AddListener(OnCoopChanged);
        optionsButton.onClick.AddListener(OnOptionsClick);
        creditsButton.onClick.AddListener(OnCreditsClick);
        quitButton.onClick.AddListener(OnQuitClick);
        SetState();
        Display();
    }

    // set state of UI elements to match game config settings
    public void SetState() {
        if (gameConfig == null) return;
        soloToggle.isOn = gameConfig.soloPlay;
        coopToggle.isOn = !gameConfig.soloPlay;
    }

    public void OnPlayClick() {
        // instantiate options prefab (under canvas)
        //var panelGo = Instantiate(optionsPrefab, UxUtil.GetCanvas().gameObject.transform);
        //var uxPanel = panelGo.GetComponent<UxPanel>();
        //uxPanel.onDoneEvent.AddListener(OnSubPanelDone);
        //Hide();
    }

    public void OnSoloChanged(bool value) {
        Debug.Log("OnSoloChanged: " + value);
        gameConfig.soloPlay = value;
    }

    public void OnCoopChanged(bool value) {
        Debug.Log("OnCoopChanged: " + value);
    }

    public void OnOptionsClick() {
        // instantiate the options panel prefab
        var panelGo = Instantiate(optionsPrefab, UxUtil.GetCanvas().gameObject.transform);
        // setup a callback, so when the sub menu/panel is done, we display the current panel again
        var uxPanel = panelGo.GetComponent<UxPanel>();
        uxPanel.onDoneEvent.AddListener(Display);
        // now hide the current panel
        Hide();
    }

    public void OnCreditsClick() {
        // instantiate credits prefab (under canvas)
        //var panelGo = Instantiate(creditsPrefab, UxUtil.GetCanvas().gameObject.transform);
        //var uxPanel = panelGo.GetComponent<UxPanel>();
        //uxPanel.onDoneEvent.AddListener(OnSubPanelDone);
        //Hide();
    }

    public void OnQuitClick() {
        Application.Quit();
    }
}