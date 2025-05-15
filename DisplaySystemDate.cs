using TMPro; // import TextMeshPro namespace
using UnityEngine;

public class DisplaySystemDate : MonoBehaviour
{
    public TextMeshProUGUI dateText; // referencing the TextMeshProUGUI component

    void Start()
    {
        if (dateText != null)
        {
            // current date and formatted
            string currentDate = System.DateTime.Now.ToString("dddd, dd MMM");
            // updating the text component with the current date
            dateText.text = currentDate;
        }
        else
        {
            Debug.LogError("Date Component not assigned!");
        }
    }
}