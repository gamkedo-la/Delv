
using UnityEngine;

public class TimeManager : MonoBehaviour {


    public float slowdownFactor = 0.5f;
    public float slowdownLength = 0.5f;


    public void SlowMo()
    {
        if (PauseMenu.GameIsPaused == false)
        {
            Time.timeScale = slowdownFactor;
            Time.fixedDeltaTime = 0.02f * slowdownFactor;
        }
    }

    private void Update()
    {
        if (PauseMenu.GameIsPaused == false)
        {
            if (Time.timeScale < 1)
            {
            Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);

            }

        }
        if (PauseMenu.GameIsPaused == true)
        {
            Time.timeScale = 0f;
        }


    }


}
