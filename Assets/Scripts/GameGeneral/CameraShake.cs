using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private bool _isShaking = false;
    private int _shakeCount;
    private float _shakeIntensity, _shakeSpeed, _baseX, _baseY;
    private Vector3 _nextShakePosition;
    public Transform ShakeAxis; 

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_isShaking)
        {
            ShakeAxis.localPosition = Vector3.MoveTowards(ShakeAxis.localPosition, _nextShakePosition, Time.deltaTime * _shakeSpeed);

            if (Vector2.Distance(ShakeAxis.localPosition, _nextShakePosition) < _shakeIntensity / 5f)
            {
                _shakeCount--;

                if (_shakeCount <= 0)
                {
                    _isShaking = false;
                    ShakeAxis.localPosition = new Vector3(_baseX, _baseY, ShakeAxis.localPosition.z);
                }
                else if (_shakeCount <= 1)
                {
                    _nextShakePosition = new Vector3(_baseX, _baseY, ShakeAxis.localPosition.z);
                }
                else
                {
                    DetermineNextShakePosition();
                }
            }
        }

    }

    public void Shake(float intensity, int shakes, float speed)
    {
        enabled = true;
        _isShaking = true;
        _shakeCount = shakes;
        _shakeIntensity = intensity;
        _shakeSpeed = speed;

        DetermineNextShakePosition();
    }


    private void DetermineNextShakePosition()
    {
        _nextShakePosition = new Vector3(Random.Range(-_shakeIntensity, _shakeIntensity),
            Random.Range(-_shakeIntensity, _shakeIntensity),
            ShakeAxis.localPosition.z);
    }
}
