using UnityEngine;
using System.Collections.Generic; // Needed for Lists

public class LiveGraph : MonoBehaviour
{
    [Header("Settings")]
    public DigitalTwinReceiver dataSource; // Drag your Engine object here
    public LineRenderer lineRenderer;      // Drag your Graph_Container here

    public int maxDataPoints = 100; // How much history to show
    public float graphHeight = 5.0f; // Visual height multiplier
    public float graphWidth = 10.0f; // Visual width

    // Store history
    private List<float> rpmHistory = new List<float>();

    void Start()
    {
        // Safety check
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        // Set fixed number of points
        lineRenderer.positionCount = maxDataPoints;
    }

    void Update()
    {
        if (dataSource == null) return;

        // 1. Get current RPM (Normalized 0.0 to 1.0)
        // We assume 7000 is max RPM. Adjust if your Python max is different.
        float normalizedRPM = dataSource.currentRPM / 7000f;

        // Clamp it just in case
        normalizedRPM = Mathf.Clamp01(normalizedRPM);

        // 2. Add to History
        rpmHistory.Add(normalizedRPM);

        // 3. Remove old data
        if (rpmHistory.Count > maxDataPoints)
        {
            rpmHistory.RemoveAt(0);
        }

        // 4. Draw the Line
        DrawGraph();
    }

    void DrawGraph()
    {
        // Distance between each point on the X axis
        float stepSize = graphWidth / maxDataPoints;

        for (int i = 0; i < rpmHistory.Count; i++)
        {
            // X = Time (moving left to right)
            // Y = RPM (Height)
            // Z = 0

            float xPos = i * stepSize;
            float yPos = rpmHistory[i] * graphHeight;

            // Set the point on the line relative to this object
            // "i" corresponds to the index of the line vertex
            // We start filling from the end if history is not full yet
            int index = i + (maxDataPoints - rpmHistory.Count);

            if (index < maxDataPoints)
            {
                // Convert local position to world position if needed, 
                // but LineRenderer usually works in local space if "Use World Space" is unchecked.
                lineRenderer.SetPosition(i, new Vector3(xPos, yPos, 0));
            }
        }
    }
}