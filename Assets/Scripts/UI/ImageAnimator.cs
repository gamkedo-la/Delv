using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ImageAnimator: MonoBehaviour {

    public Sprite[] sprites;
    public Image imageToOverwrite;

	public bool playOnStart = false;
    public UnityEvent onDoneEvent;

    public float duration = 2f;

	IEnumerator DoPlay() {
		if (sprites.Length <= 0) {
			yield break;
		}

		// iterate through the sprites
		var startTime = Time.time;
		var endTime = startTime + duration;
		var numSprites = sprites.Length;

		// iterate per frame
		var frame = 0;
		while (Time.time < endTime) {
			var index = Mathf.FloorToInt((Time.time - startTime)/duration*numSprites);
			imageToOverwrite.sprite = sprites[index];
			//imageToOverwrite.preserveAspect = true;
			frame++;
			yield return null;
		}
        onDoneEvent.Invoke();
	}

    public void Play() {
    	StartCoroutine(DoPlay());
    }
    public void PlayOn(Image image) {
		imageToOverwrite = image;
    	StartCoroutine(DoPlay());
    }

	public void Awake() {
        onDoneEvent = new UnityEvent();
	}

	public void Start() {
		if (playOnStart) {
			Play();
		}
	}


}
