using UnityEngine;

public class EngineControllerRpmMeter : MonoBehaviour
{
    [Header("Engine Data")]
    public float rpm = 800f;   // controlled by sound later

    [Header("RPM Meter")]
    public RectTransform rpmNeedle;
    public float minRPM = 0;
    public float maxRPM = 6000;
    public float minAngle = -120;
    public float maxAngle = 120;

    void Update()
    {
        float rpmPercent = Mathf.InverseLerp(minRPM, maxRPM, rpm);
        float needleAngle = Mathf.Lerp(minAngle, maxAngle, rpmPercent);
        rpmNeedle.localRotation = Quaternion.Euler(0, 0, needleAngle);
    }
}
