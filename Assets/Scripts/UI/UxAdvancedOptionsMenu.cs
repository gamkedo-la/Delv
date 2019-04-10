using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UxAdvancedOptionsMenu : UxPanel {
    [Header("UI Reference")]
    public Toggle enableScreenShakeToggle;
    public Toggle showDamageNumbersToggle;
    public Dropdown maxParticlesDropdown;
    public Button okButton;

    [Header("State Variables")]
    private GameManagerScript gameManager;

    string SelectSound = "event:/UI/Select";
    string MouseoverSound = "event:/UI/Mouseover";

    public void Start() {
        gameManager = GameManagerScript.instance;
        // setup callbacks
        enableScreenShakeToggle.onValueChanged.AddListener(
            (value)=>{ gameManager.Screenshake = value;});
        showDamageNumbersToggle.onValueChanged.AddListener(
            (value)=>{ gameManager.DamageText = value;});
        maxParticlesDropdown.onValueChanged.AddListener(
            (value)=>{ 
                // value is index of selection in options list... need to options "text" value to an int and store in config
                var strValue = maxParticlesDropdown.options[value].text;
                int intValue = 0;
                Int32.TryParse(strValue, out intValue);
                gameManager.ParticleIntensity = intValue;
            });
        okButton.onClick.AddListener(OnOkClick);
        SetState();
        Display();
    }

    // set state of UI elements to match game config settings
    public void SetState() {
        if (gameManager == null) return;
        enableScreenShakeToggle.isOn = gameManager.Screenshake;
        showDamageNumbersToggle.isOn = gameManager.DamageText;
        // gameManager value is an integer value, dropdown contains options list, where each element is a string
        // representing an selection of max particles setting.  Find the index of the options element
        // that matches the current config setting.
        var index = maxParticlesDropdown.options.FindIndex(a => a.text == gameManager.ParticleIntensity.ToString());
        if (index > 0) {
            maxParticlesDropdown.value = index;
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

}
