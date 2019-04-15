using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class UxPanel : MonoBehaviour {
    private CanvasGroup canvasGroup;

    [HideInInspector]
    public UnityEvent onDoneEvent;
    public bool hidden=false;
    public bool enabled=true;
    public bool isActive {
        get {
            return !hidden && enabled;
        }
    }

    void Awake() {
        onDoneEvent = new UnityEvent();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public virtual void Enable() {
        enabled = true;
        if (canvasGroup != null) {
            if (!hidden) {
                canvasGroup.blocksRaycasts = true; //this prevents the UI element to receive input events
                canvasGroup.interactable = true;
            }
        }
    }

    public virtual void Disable() {
        enabled = false;
        canvasGroup.blocksRaycasts = false; //this prevents the UI element to receive input events
        canvasGroup.interactable = false;
    }

    public virtual void Display() {
        hidden = false;
        if (canvasGroup != null) {
            canvasGroup.alpha = 1f; //this makes everything transparent
            if (enabled) {
                canvasGroup.blocksRaycasts = true; //this prevents the UI element to receive input events
                canvasGroup.interactable = true;
            }
        }
    }

    public virtual void Hide() {
        hidden = true;
        if (canvasGroup != null) {
            canvasGroup.alpha = 0f; //this makes everything transparent
            canvasGroup.blocksRaycasts = false; //this prevents the UI element to receive input events
            canvasGroup.interactable = false;
        }
    }

    void OnDestroy() {
        onDoneEvent.Invoke();
    }
}
