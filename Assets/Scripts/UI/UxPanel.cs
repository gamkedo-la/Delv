using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class UxPanel : MonoBehaviour {
    private CanvasGroup canvasGroup;

    [HideInInspector]
    public UnityEvent onDoneEvent;

    void Awake() {
        onDoneEvent = new UnityEvent();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public virtual void Display() {
        if (canvasGroup != null) {
            canvasGroup.alpha = 1f; //this makes everything transparent
            canvasGroup.blocksRaycasts = true; //this prevents the UI element to receive input events
            canvasGroup.interactable = true;
        }
    }

    public virtual void Hide() {
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
