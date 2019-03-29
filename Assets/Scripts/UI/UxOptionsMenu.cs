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

    [Header("State Variables")]
    public GameConfig gameConfig;

    public void Start() {
        // setup callbacks
        p1InputXInputToggle.onValueChanged.AddListener(
            (value)=>{if (value) gameConfig.p1ControllerKind = ControllerKind.XInput;});
        p1InputDualshockToggle.onValueChanged.AddListener(
            (value)=>{if (value) gameConfig.p1ControllerKind = ControllerKind.DualShock;});
        p1InputKeyboardToggle.onValueChanged.AddListener(
            (value)=>{if (value) gameConfig.p1ControllerKind = ControllerKind.Keyboard;});
        p2InputXInputToggle.onValueChanged.AddListener(
            (value)=>{if (value) gameConfig.p2ControllerKind = ControllerKind.XInput;});
        p2InputDualshockToggle.onValueChanged.AddListener(
            (value)=>{if (value) gameConfig.p2ControllerKind = ControllerKind.DualShock;});
        p2InputAIToggle.onValueChanged.AddListener(
            (value)=>{if (value) gameConfig.p2ControllerKind = ControllerKind.AI;});
        masterVolumeSlider.onValueChanged.AddListener((value)=>{gameConfig.masterVolume = value;});
        sfxVolumeSlider.onValueChanged.AddListener((value)=>{gameConfig.sfxVolume = value;});
        musicVolumeSlider.onValueChanged.AddListener((value)=>{gameConfig.musicVolume = value;});
        advancedButton.onClick.AddListener(OnAdvancedClick);
        okButton.onClick.AddListener(OnOkClick);
        SetState();
        Display();
    }

    // set state of UI elements to match game config settings
    public void SetState() {
        if (gameConfig == null) return;
        p1InputXInputToggle.isOn = gameConfig.p1ControllerKind == ControllerKind.XInput;
        p1InputDualshockToggle.isOn = gameConfig.p1ControllerKind == ControllerKind.DualShock;
        p1InputKeyboardToggle.isOn = gameConfig.p1ControllerKind == ControllerKind.Keyboard;
        p2InputXInputToggle.isOn = gameConfig.p2ControllerKind == ControllerKind.XInput;
        p2InputDualshockToggle.isOn = gameConfig.p2ControllerKind == ControllerKind.DualShock;
        p2InputAIToggle.isOn = gameConfig.p2ControllerKind == ControllerKind.AI;
	masterVolumeSlider.value = gameConfig.masterVolume;
	sfxVolumeSlider.value = gameConfig.sfxVolume;
	musicVolumeSlider.value = gameConfig.musicVolume;
    }

    public void OnOkClick() {
        Destroy(gameObject);
    }

    public void OnAdvancedClick() {
        // instantiate the advanced panel prefab
        var panelGo = Instantiate(advancedPrefab, UxUtil.GetCanvas().gameObject.transform);
        // setup a callback, so when the sub menu/panel is done, we display the current panel again
        var uxPanel = panelGo.GetComponent<UxPanel>();
        uxPanel.onDoneEvent.AddListener(Display);
        // now hide the current panel
        Hide();
    }

}
