using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class UxPauseMenu : UxPanel 
{
    [Header("UI Reference")]
    public Button resumeButton;
    public Button settingsButton;
    public Button mainMenuButton;
    public Button quitButton;

    [Header("Prefabs")]
    public GameObject optionsPrefab;

    // game state references
    TimeManager timeManager;
    bool _paused = false;
    bool paused {
        get {
            return (timeManager != null) ? timeManager.gameIsPaused : _paused;
        }
        set {
            if (timeManager != null) {
                timeManager.gameIsPaused = value;
            }
            _paused = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        timeManager = TimeManager.instance;
        if (timeManager != null) {
            timeManager.gameIsPaused = true;
        }

        resumeButton.onClick.AddListener(OnResumeClick);
        settingsButton.onClick.AddListener(OnSettingsClick);
        mainMenuButton.onClick.AddListener(OnMenuClick);
        quitButton.onClick.AddListener(OnQuitClick);
        Display();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (timeManager != null) {
                timeManager.gameIsPaused = false;
            }
            Destroy(gameObject);
        }
    }

    public void OnResumeClick()
    {
        if (timeManager != null) {
            timeManager.gameIsPaused = false;
        }
        Destroy(gameObject);
    }

    public void OnMenuClick()
    {
        if (timeManager != null) {
            timeManager.gameIsPaused = false;
        }
        Destroy(gameObject);
        SceneManager.LoadScene(0);
    }

    public void OnSettingsClick() {
        // instantiate the options panel prefab
        var panelGo = Instantiate(optionsPrefab, GetComponentInParent<Canvas>().gameObject.transform);
        // setup a callback, so when the sub menu/panel is done, we display the current panel again
        var uxPanel = panelGo.GetComponent<UxPanel>();
        uxPanel.onDoneEvent.AddListener(Display);
        // now hide the current panel
        Hide();
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }

}
