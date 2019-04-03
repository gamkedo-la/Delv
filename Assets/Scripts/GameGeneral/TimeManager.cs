
using UnityEngine;

public class TimeManager : MonoBehaviour {

	public static TimeManager instance;

    public bool gameIsPaused = false;

    public float slowdownFactor = 0.5f;
    public float slowdownLength = 0.5f;

    public void Awake() 
    {
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
		}
	}


    public void SlowMo()
    {
        if (gameIsPaused == false)
        {
            Time.timeScale = slowdownFactor;
            Time.fixedDeltaTime = 0.02f * slowdownFactor;
        }
    }

    private void Update()
    {
        if (gameIsPaused == false)
        {
            if (Time.timeScale < 1)
            {
            Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);

            }

        }
        if (gameIsPaused == true)
        {
            Time.timeScale = 0f;
        }


    }


}
