
using UnityEngine;

public class TimeManager : MonoBehaviour {

    public float slowdownFactor = 0.5f;
    public float slowdownLength = 0.5f;

     public void SlowMo()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = 0.02f * slowdownFactor;
    }

    private void Update()
    {
        Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
    }
}
