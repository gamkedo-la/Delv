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
    public VideoClip scrollVideoClip;
    public float fadeInTime = 1.5f;

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
        if (scrollImagePanel != null && scrollVideoClip != null) {
            ScrollAnimate();
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

    public void ScrollLinkTexture() {
        // destroy any current image linked to the panel
        var image = scrollImagePanel.GetComponent<Image>();
        if (image != null) {
            DestroyImmediate(image);
        }
        // create rendertexture, matching size of panel
        renderTexture = new RenderTexture((int)scrollImagePanel.rect.width, (int)scrollImagePanel.rect.height, 24);
        // add new raw image
        var rawImage = scrollImagePanel.gameObject.AddComponent<RawImage>();
        rawImage.texture = renderTexture;
        // setup separate canvas group for scroll image
        var localCanvasGroup = scrollImagePanel.gameObject.AddComponent<CanvasGroup>();
        localCanvasGroup.ignoreParentGroups = true;
    }

    public void ScrollSetupVideo() {
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        videoPlayer.isLooping = false;
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.clip = scrollVideoClip;
        videoPlayer.targetTexture = renderTexture;
        videoPlayer.loopPointReached += OnVideoClipEnd;
    }

    void OnVideoClipEnd(VideoPlayer player) {
        Debug.Log("OnVideoClipEnd");
        FadeIn(fadeInTime);
    }

    public void ScrollAnimate() {
        // Hide the main panel
        Hide();
        ScrollLinkTexture();
        ScrollSetupVideo();
    }

}