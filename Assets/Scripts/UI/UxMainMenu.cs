using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class UxMainMenu : UxPanel {
    [Header("UI Reference")]
    public Button playButton;
    public Toggle soloToggle;
    public Toggle coopToggle;
    public Button optionsButton;
    public Button creditsButton;
    public Button quitButton;

    [Header("Scroll Animation")]
    public RectTransform scrollImagePanel;
    public ImageAnimator scrollAnimator;
    public float fadeInTime = 2f;

    // Fmod UI sounds
    string SelectSound = "event:/UI/Select";
    string MouseoverSound = "event:/UI/Mouseover";
    FMOD.Studio.EventInstance lowpassfilter_SnapshotEv;

    VideoPlayer videoPlayer;
    RenderTexture renderTexture;

    [Header("Prefabs")]
    public GameObject optionsPrefab;
    public GameObject creditsPrefab;

    private GameManagerScript gameManager;
    private GameObject dialogueCanvasGo;

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
        if (scrollAnimator != null)
        {
            ScrollAnimate();
        }
        // Ugly hack... disable the dialogue
        dialogueCanvasGo = GameObject.Find("GameManager/DialogueCanvas");
        if (dialogueCanvasGo != null) {
            dialogueCanvasGo.SetActive(false);
        }
    }

    // set state of UI elements to match game config settings
    public void SetState() {
        if (gameManager != null) {
            soloToggle.isOn = (gameManager.PlayerCount == 1);
            coopToggle.isOn = (gameManager.PlayerCount != 1);
        }
    }

    public void OnPlayClick() {
        Hide();
        // Ugly hack... re-enable the dialogue
        if (dialogueCanvasGo != null) {
            dialogueCanvasGo.SetActive(true);
        }
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
        var panelGo = Instantiate(optionsPrefab, GetComponentInParent<Canvas>().gameObject.transform);
        // setup a callback, so when the sub menu/panel is done, we display the current panel again
        var uxPanel = panelGo.GetComponent<UxPanel>();
        uxPanel.onDoneEvent.AddListener(Enable);
        // now hide the current panel
        Disable();
        FMODUnity.RuntimeManager.PlayOneShot(SelectSound, transform.position);

    }

    public void OnCreditsClick() {
        // instantiate credits prefab (under canvas)
        var panelGo = Instantiate(creditsPrefab, GetComponentInParent<Canvas>().gameObject.transform);
        var uxPanel = panelGo.GetComponent<UxPanel>();
        uxPanel.onDoneEvent.AddListener(Enable);
        Disable();
        FMODUnity.RuntimeManager.PlayOneShot(SelectSound, transform.position);
    }

    public void OnMouseoverSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(MouseoverSound, transform.position);
    }

    public void OnQuitClick() {
        Application.Quit();
    }

    public void ScrollAnimate() {
        // setup image to not follow parent canvas group (i.e.: so it won't get hidden)
        var localCanvasGroup = scrollImagePanel.gameObject.AddComponent<CanvasGroup>();
        localCanvasGroup.ignoreParentGroups = true;
        // Hide the main panel
        Hide();
        // setup the scroll animator... when animation is done, fade the main menu back in
        scrollAnimator.onDoneEvent.AddListener(() => FadeIn(fadeInTime));
        // queue audio
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/MenuIntro_Scroll");
        // queue video
        scrollAnimator.PlayOn(scrollImagePanel.GetComponent<Image>());

    }

}