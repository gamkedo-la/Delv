using System.Collections;
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
    bool runningFade = false;

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

    public virtual void FadeIn(float duration) {
        StartCoroutine(DoFadeIn(duration));
    }

	IEnumerator DoFadeIn(float duration)
	{
		if (duration > 0f)
		{
			var alphaDelta = Mathf.Clamp(0f, 1f, 1f - canvasGroup.alpha);
			var alphaFactor = alphaDelta / duration;
			// if a fade operation is already running... force it to stop, so that this new fade can take over
			if (runningFade)
			{
				runningFade = false;
				yield return null; // wait a frame so that any other fade coroutine can finish
			}
			runningFade = true;
			while (runningFade)
			{
				// decrement fade duration for each frame... once we hit zero, stop
				duration -= Time.deltaTime;
				var newAlpha = Mathf.Min(1.0f, canvasGroup.alpha + (Time.deltaTime * alphaFactor));
				canvasGroup.alpha = newAlpha;
				if (duration <= 0)
				{
					runningFade = false;
					break;
				}
				yield return null;  // wait until next frame
			}
		}
		// fade complete, reactivate screen fully
		Display();
	}

	public virtual void FadeOut(float duration) {
        StartCoroutine(DoFadeOut(duration));
    }

    IEnumerator DoFadeOut(float duration) {
        if (duration > 0f) {
            var alphaDelta = Mathf.Clamp(0f, 1f, canvasGroup.alpha);
            var alphaFactor = alphaDelta/duration;
            // if a fade operation is already running... force it to stop, so that this new fade can take over
            if (runningFade) {
                runningFade = false;
                yield return null; // wait a frame so that any other fade coroutine can finish
            }
            runningFade = true;
            while (runningFade) {
                // decrement fade duration for each frame... once we hit zero, stop
                duration -= Time.deltaTime;
                var newAlpha = Mathf.Max(0f, canvasGroup.alpha - (Time.deltaTime * alphaFactor));
                canvasGroup.alpha = newAlpha;
                if (duration <= 0) {
                    runningFade = false;
                    break;
                }
                yield return null;  // wait until next frame
            }
        }
        // fade complete, deactivate screen fully
        Hide();
    }

    void OnDestroy() {
        onDoneEvent.Invoke();
    }
}
