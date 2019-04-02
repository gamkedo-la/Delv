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
    //public Slider masterVolumeSlider;
    //public Slider sfxVolumeSlider;
    //public Slider musicVolumeSlider;

    [Header("Prefabs")]
    public GameObject advancedPrefab;

    private GameManagerScript gameManager;

    public void Start() {
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
            //masterVolumeSlider.onValueChanged.AddListener((value)=>{gameManager.masterVolume = value;});
            //sfxVolumeSlider.onValueChanged.AddListener((value)=>{gameManager.sfxVolume = value;});
            //musicVolumeSlider.onValueChanged.AddListener((value)=>{gameManager.musicVolume = value;});
        }
        advancedButton.onClick.AddListener(OnAdvancedClick);
        okButton.onClick.AddListener(OnOkClick);
        SetState();
        Display();
    }

    // set state of UI elements to match game config settings
    public void SetState() {
        if (gameManager == null) return;
        p1InputXInputToggle.isOn = gameManager.p1ControllerKind == ControllerKind.XInput;
        p1InputDualshockToggle.isOn = gameManager.p1ControllerKind == ControllerKind.DualShock;
        p1InputKeyboardToggle.isOn = gameManager.p1ControllerKind == ControllerKind.Keyboard;
        p2InputXInputToggle.isOn = gameManager.p2ControllerKind == ControllerKind.XInput;
        p2InputDualshockToggle.isOn = gameManager.p2ControllerKind == ControllerKind.DualShock;
        p2InputAIToggle.isOn = gameManager.p2ControllerKind == ControllerKind.AI;
        //masterVolumeSlider.value = gameManager.masterVolume;
        //sfxVolumeSlider.value = gameManager.sfxVolume;
        //musicVolumeSlider.value = gameManager.musicVolume;
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
