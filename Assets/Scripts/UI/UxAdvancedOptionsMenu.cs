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
    public GameConfig gameConfig;

    public void Start() {
        // setup callbacks
        enableScreenShakeToggle.onValueChanged.AddListener(
            (value)=>{ gameConfig.enableScreenShake = value;});
        showDamageNumbersToggle.onValueChanged.AddListener(
            (value)=>{ gameConfig.showDamageNumbers = value;});
        maxParticlesDropdown.onValueChanged.AddListener(
            (value)=>{ 
                // value is index of selection in options list... need to options "text" value to an int and store in config
                var strValue = maxParticlesDropdown.options[value].text;
                int intValue = 0;
                Int32.TryParse(strValue, out intValue);
                gameConfig.maxParticles = intValue;
            });
        okButton.onClick.AddListener(OnOkClick);
        SetState();
        Display();
    }

    // set state of UI elements to match game config settings
    public void SetState() {
        if (gameConfig == null) return;
        enableScreenShakeToggle.isOn = gameConfig.enableScreenShake;
        showDamageNumbersToggle.isOn = gameConfig.showDamageNumbers;
        // gameConfig is an integer value, dropdown contains options list, where each element is a string
        // representing an selection of max particles setting.  Find the index of the options element
        // that matches the current config setting.
        var index = maxParticlesDropdown.options.FindIndex(a => a.text == gameConfig.maxParticles.ToString());
        if (index > 0) {
            maxParticlesDropdown.value = index;
        }
    }

    public void OnOkClick() {
        Destroy(gameObject);
    }

}
