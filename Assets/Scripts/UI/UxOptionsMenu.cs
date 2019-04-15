using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UxOptionsMenu : UxPanel {
    [Header("UI Reference")]
    public Toggle p1InputXInputToggle;
    public Toggle p1InputDualshockToggle;
    public Toggle p1InputKeyboardToggle;
    public Toggle p2InputXInputToggle;
    public Toggle p2InputDualshockToggle;
    public Toggle p2InputAIToggle;
    public Button advancedButton;
    public Button okButton;
    public Slider masterVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider musicVolumeSlider;

    [Header("Prefabs")]
    public GameObject advancedPrefab;

    private GameManagerScript gameManager;
    private AudioSettings audioSettings;
    string SelectSound = "event:/UI/Select";
    string MouseoverSound = "event:/UI/Mouseover";


    public void Start() {
        audioSettings = GetComponent<AudioSettings>();
        gameManager = GameManagerScript.instance;
        // setup callbacks
        if (gameManager != null) {
            p1InputXInputToggle.onValueChanged.AddListener(
                (value)=>{if (value) gameManager.p1ControllerKind = ControllerKind.XInput;});
            p1InputDualshockToggle.onValueChanged.AddListener(
                (value)=>{if (value) gameManager.p1ControllerKind = ControllerKind.DualShock;});
            p1InputKeyboardToggle.onValueChanged.AddListener(
                (value)=>{if (value) gameManager.p1ControllerKind = ControllerKind.Keyboard;});
            p2InputXInputToggle.onValueChanged.AddListener(
                (value)=>{
                    if (value) {
                        gameManager.p2ControllerKind = ControllerKind.XInput;
                        gameManager.isAIBot = false;
                    }
                });
            p2InputDualshockToggle.onValueChanged.AddListener(
                (value)=>{
                    if (value) {
                        gameManager.p2ControllerKind = ControllerKind.DualShock;
                        gameManager.isAIBot = false;
                    }
                });
            p2InputAIToggle.onValueChanged.AddListener(
                (value)=>{
                    if (value) {
                        gameManager.p2ControllerKind = ControllerKind.AI;
                        gameManager.isAIBot = true;
                    }
                });
        }
        if (audioSettings != null) {
            masterVolumeSlider.onValueChanged.AddListener((value)=>{audioSettings.MasterVolume = value;});
            sfxVolumeSlider.onValueChanged.AddListener((value)=>{audioSettings.SFXVolume = value;});
            musicVolumeSlider.onValueChanged.AddListener((value)=>{audioSettings.MusicVolume = value;});
        }
        advancedButton.onClick.AddListener(OnAdvancedClick);
        okButton.onClick.AddListener(OnOkClick);
        SetState();
        Display();
    }

    // set state of UI elements to match game config settings
    public void SetState() {
        if (gameManager != null) {
            p1InputXInputToggle.isOn = gameManager.p1ControllerKind == ControllerKind.XInput;
            p1InputDualshockToggle.isOn = gameManager.p1ControllerKind == ControllerKind.DualShock;
            p1InputKeyboardToggle.isOn = gameManager.p1ControllerKind == ControllerKind.Keyboard;
            p2InputXInputToggle.isOn = gameManager.p2ControllerKind == ControllerKind.XInput;
            p2InputDualshockToggle.isOn = gameManager.p2ControllerKind == ControllerKind.DualShock;
            p2InputAIToggle.isOn = gameManager.p2ControllerKind == ControllerKind.AI;
        }
        if (audioSettings != null) {
            masterVolumeSlider.value = audioSettings.MasterVolume;
            sfxVolumeSlider.value = audioSettings.SFXVolume;
            musicVolumeSlider.value = audioSettings.MusicVolume;
        }
    }

    void Update() {
        if (isActive && Input.GetButtonDown("Pause")) {
            Destroy(gameObject);
        }
    }

    public void OnOkClick() {
        FMODUnity.RuntimeManager.PlayOneShot(SelectSound, transform.position);
        Destroy(gameObject);
    }

    public void OnMouseoverSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(MouseoverSound, transform.position);
    }

    public void OnAdvancedClick() {
        // instantiate the advanced panel prefab
        var panelGo = Instantiate(advancedPrefab, GetComponentInParent<Canvas>().gameObject.transform);
        // setup a callback, so when the sub menu/panel is done, we display the current panel again
        var uxPanel = panelGo.GetComponent<UxPanel>();
        uxPanel.onDoneEvent.AddListener(Enable);
        // now hide the current panel
        FMODUnity.RuntimeManager.PlayOneShot(SelectSound, transform.position);
        Disable();
    }

}
