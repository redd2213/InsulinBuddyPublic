using TMPro; // import TextMeshPro namespace
using UnityEngine;

public class DisplaySystemTime : MonoBehaviour
{
    public TextMeshProUGUI timeText; // referencing the TextMeshProUGUI component

    void Update()
    {
        if (timeText != null)
        {
            // current date and formatted
            string currentTime = System.DateTime.Now.ToString("HH:mm:ss");
            // updating the text component with the current date
            timeText.text = currentTime;
        }
        else
        {
            Debug.LogError("Time component not assigned!");
        }
    }
}