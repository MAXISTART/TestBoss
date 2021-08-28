using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShaker : MonoBehaviour
{

    public CinemachineVirtualCamera cam;
    public float ShakeDuration = 0.3f;          // Time the Camera Shake effect will last
    public float ShakeAmplitude = 1.2f;         // Cinemachine Noise Profile Parameter
    public float ShakeFrequency = 2.0f;         // Cinemachine Noise Profile Parameter
    public float ShakeStopDamp = 0.5f;         // Stop Shaking Damp
    public bool Stop = true;

    private CinemachineBasicMultiChannelPerlin cameraNoise;
    private float ShakeElapsedTime = 0f;

    private float OriginShakeAmplitude;
    private float OriginShakeFrequency;

    void Start()
    {
        cameraNoise = cam.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        ShakeElapsedTime = ShakeDuration;
        OriginShakeAmplitude = cameraNoise.m_AmplitudeGain;
        OriginShakeFrequency = cameraNoise.m_FrequencyGain;
    }

    public void Shake(float _ShakeDuration, float _ShakeAmplitude, float _ShakeFrequency, float _ShakeStopDamp) {
        ShakeElapsedTime = _ShakeDuration;
        ShakeAmplitude = _ShakeAmplitude;
        ShakeFrequency = _ShakeFrequency;
        ShakeStopDamp = _ShakeStopDamp;
        Stop = false;
    }


    void Update()
    {
        if (Stop) return;
        // If the Cinemachine componet is not set, avoid update
        if (cam != null && cameraNoise != null)
        {
            // If Camera Shake effect is still playing
            if (ShakeElapsedTime > 0)
            {
                // Set Cinemachine Camera Noise parameters
                cameraNoise.m_AmplitudeGain = ShakeAmplitude;
                cameraNoise.m_FrequencyGain = ShakeFrequency;

                // Update Shake Timer
                ShakeElapsedTime -= Time.deltaTime;
            }
            else
            {
                // If Camera Shake effect is over, reset variables                
                cameraNoise.m_AmplitudeGain = Mathf.Lerp(cameraNoise.m_AmplitudeGain, OriginShakeAmplitude, ShakeStopDamp);
                cameraNoise.m_FrequencyGain = Mathf.Lerp(cameraNoise.m_FrequencyGain, OriginShakeFrequency, ShakeStopDamp);
                if (cameraNoise.m_AmplitudeGain <= OriginShakeAmplitude) {
                    Stop = true;
                    ShakeElapsedTime = 0f;
                }
            }
        }
    }
}
