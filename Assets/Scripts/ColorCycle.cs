using UnityEngine;

public class ColorCycle : MonoBehaviour
{
    public float cycleSpeed = 1.0f; // Speed of color change
    public Color[] colors; // Array of colors to cycle through
    private Material material;
    private int currentIndex = 0;
    private float timeElapsed = 0.0f;

    void Start()
    {
        material = GetComponent<Renderer>().material; // Get the material of the object
        material.color = colors[0]; // Set initial color

        if (colors.Length < 2)
        {
            Debug.LogWarning("Color array should have at least 2 colors for cycling.");
            enabled = false; // Disable the script if there aren't enough colors
        }
    }

    void Update()
    {
        // Update the time elapsed
        timeElapsed += Time.deltaTime;

        // Calculate current progress through the cycle
        float t = Mathf.PingPong(timeElapsed * cycleSpeed / colors.Length, 1.0f);

        // Interpolate between current and next color
        int nextIndex = (currentIndex + 1) % colors.Length;
        material.color = Color.Lerp(colors[currentIndex], colors[nextIndex], t);

        // If reaching the end of the current cycle segment, move to the next color
        if (t >= 0.99f)
        {
            currentIndex = nextIndex;
            timeElapsed = 0.0f; // Reset time elapsed for next cycle segment
        }
    }
}
