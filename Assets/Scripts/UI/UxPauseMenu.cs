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

    //FMOD Low Pass Filter
    private string lowpassfilter_Snapshot = "snapshot:/Main_Menu_LPF";
    FMOD.Studio.EventInstance lowpassfilter_SnapshotEv;

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

    // Fmod UI sounds
    string SelectSound = "event:/UI/Select";
    string MouseoverSound = "event:/UI/Mouseover";
    string CloseMenuSound = "event:/UI/MainMenuClose";
    string OpenMenuSound = "event:/UI/MainMenuOpen";

    // Use this for initialization
    void Start()
    {
        timeManager = TimeManager.instance;
        if (timeManager != null) {
            timeManager.gameIsPaused = true;
            FMODUnity.RuntimeManager.PlayOneShot(OpenMenuSound, transform.position);
            lowpassfilter_SnapshotEv = FMODUnity.RuntimeManager.CreateInstance(lowpassfilter_Snapshot);
            lowpassfilter_SnapshotEv.start();
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
                FMODUnity.RuntimeManager.PlayOneShot(CloseMenuSound, transform.position);
                lowpassfilter_SnapshotEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                lowpassfilter_SnapshotEv.release();
            }
            Destroy(gameObject);
        }
    }

    public void OnResumeClick()
    {
        if (timeManager != null) {
            timeManager.gameIsPaused = false;
            FMODUnity.RuntimeManager.PlayOneShot(CloseMenuSound, transform.position);
            lowpassfilter_SnapshotEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            lowpassfilter_SnapshotEv.release();
        }
        Destroy(gameObject);
    }

    public void OnMenuClick()
    {
        if (timeManager != null) {
            timeManager.gameIsPaused = false;
        }
        FMODUnity.RuntimeManager.PlayOneShot(SelectSound, transform.position);
        Destroy(gameObject);
        lowpassfilter_SnapshotEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        lowpassfilter_SnapshotEv.release();
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
        FMODUnity.RuntimeManager.PlayOneShot(SelectSound, transform.position);
    }

    public void OnMouseoverSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(MouseoverSound, transform.position);
    }

    public void OnQuitClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot(SelectSound, transform.position);
        Application.Quit();
    }

}
