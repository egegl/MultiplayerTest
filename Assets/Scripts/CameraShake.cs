using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
REFERENCE THIS SCRIPT BY "CameraShake.Instance.Shake(instensity float, length float);"
FOLLOWED A CINEMACHINE2D CAMERA SHAKE TUTORIAL TO WRITE IT
*/

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance
    {
        get; private set;
    }
    private CinemachineVirtualCamera cmvcamera;
    private float shakeTimer;
    private float shakeTimerTotal;
    private float startingIntensity;
    private void Awake()
    {
        Instance = this;
        cmvcamera = GetComponent<CinemachineVirtualCamera>();
    }
    public void Shake(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cmvcamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;

        startingIntensity = intensity;
        shakeTimer = time;
        shakeTimerTotal = time;
    }
    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cmvcamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, 1 - (shakeTimer / shakeTimerTotal));
        }
    }
}