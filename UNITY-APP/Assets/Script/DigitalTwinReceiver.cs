using UnityEngine;
using UnityEngine.UI;
using TMPro; // Needed for Text
using System.IO;
using System;
using System.Collections.Generic;

public class DigitalTwinReceiver : MonoBehaviour
{
    [Header("3D Components")]
    [Tooltip("Drag all parts that should spin here (e.g., Crankshaft, Fans, Pulley)")]
    public List<Transform> spinningPart = new List<Transform>();

    [Tooltip("Drag the object that changes color")]
    public List<Renderer> colorPart = new List<Renderer>();     

    [Header("UI Dashboard")]
    public TextMeshProUGUI rpmText;    // Drag 'Text_RPM' here
    public TextMeshProUGUI statusText; // Drag 'Text_Status' here

    [Header("Settings")]
    public float maxRPM = 7000f;

    // Internal Data
    private string filePath = "D:\\PythonScript\\EngineDigitalTwin\\AuduoRpmScript\\engine_data.json";
    public float currentRPM;
    private float smoothRPM;

    void Update()
    {
        // --- 1. READ DATA ---
        if (File.Exists(filePath))
        {
            try
            {
                string json = File.ReadAllText(filePath);
                TwinData_Final data = JsonUtility.FromJson<TwinData_Final>(json);
                if (data != null) currentRPM = data.rpm;
            }
            catch (Exception) { }
        }

        // --- 2. SMOOTHING ---
        smoothRPM = Mathf.Lerp(smoothRPM, currentRPM, Time.deltaTime * 5f);

        // --- 3. SPIN THE ENGINE ---
        foreach (Transform part in spinningPart)
        {
            // Check if the slot is actually filled to avoid errors
            if (part != null)
            {
                // Spin constantly based on RPM speed
            }    part.Rotate(Vector3.forward * smoothRPM * Time.deltaTime * 6f);
        }

        // --- 5. UPDATE UI DASHBOARD ---
        if (rpmText != null)
        {
            rpmText.text = Mathf.RoundToInt(smoothRPM).ToString("0000");
        }

        if (statusText != null)
        {
            if (smoothRPM < 1000) statusText.text = "IDLE";
            else if (smoothRPM < 5000) statusText.text = "CRUISING";
            else
            {
                statusText.text = "REDLINE!";
                statusText.color = Color.red; // Warning color
            }
        }

        // --- 6. COLOR CHANGE ---
        foreach (Renderer part in colorPart)
        {
            if (part != null)
            {
                Color targetColor;

                // Determine the target color based on RPM
                if (smoothRPM < 2000)
                    targetColor = Color.green;
                else if (smoothRPM < 5000)
                    targetColor = Color.yellow;
                else
                    targetColor = Color.red;

                // Apply the smooth color transition to this specific part
                part.material.color = Color.Lerp(part.material.color, targetColor, Time.deltaTime * 5);
            }
        }
    }
}

[System.Serializable]
public class TwinData_Final
{
    public float rpm;
    public float volume;
}