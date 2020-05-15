using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.25f;
    private float dampingSpeed = 1f;
    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnEnable()
    {
        initialPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeDuration > 0)
        {
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            transform.localPosition = initialPosition;
        }
    }

    public void triggerShake()
    {
        shakeDuration = 0.25f;
    }
}
