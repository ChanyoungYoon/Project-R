using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LightFlicker : MonoBehaviour
{
    Light _light;
    float intensity;

    public float min;
    public float frequency;

    float waitTime;

    private void Start()
    {
        _light = GetComponent<Light>();
        intensity = _light.intensity;
        waitTime = 1 / frequency;
        StartCoroutine(Flicker());
    }

    private IEnumerator Flicker()
    {
        while (true)
        {
            waitTime = 1 / frequency;
            _light.intensity = intensity * (Random.value*(1-min)+min);
            yield return new WaitForSecondsRealtime(waitTime*(Random.value));
        }
    }
}
