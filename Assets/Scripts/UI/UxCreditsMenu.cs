using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UxCreditsMenu : UxPanel {
    [Header("UI Reference")]
    public Button okButton;

    string SelectSound = "event:/UI/Select";
    string MouseoverSound = "event:/UI/Mouseover";

    public void Start() {
        // setup callbacks
        okButton.onClick.AddListener(OnOkClick);
        Display();
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

}
