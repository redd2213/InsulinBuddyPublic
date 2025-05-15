using UnityEngine;
using TMPro;
using System;

public class DataSaverWithLiveUnits : MonoBehaviour
{
    public TMP_InputField bloodGlucoseInput; // Reference to the Blood Glucose input field
    public TMP_InputField carbIntakeInput;  // Reference to the Carb Intake input field
    public TextMeshProUGUI lastUpdatedText; // Reference to the TextMeshPro field to display the last updated date

    private const string BloodGlucoseKey = "BloodGlucose";
    private const string CarbIntakeKey = "CarbIntake";
    private const string LastUpdatedKey = "LastUpdated";

    private void Start()
    {
        // Update units while the user types (add/remove units as necessary)
        bloodGlucoseInput.onValueChanged.AddListener(UpdateBloodGlucoseInput);
        carbIntakeInput.onValueChanged.AddListener(UpdateCarbIntakeInput);
    }

    // Live update for blood glucose input (add "mmol/L" as user types, remove if it's already there)
    private void UpdateBloodGlucoseInput(string value)
    {
        // Remove any existing units and leave only the numeric value
        string numericValue = value.Replace(" mmol/L", "").Trim();

        // Only append the unit if the user hasn't typed it already
        if (!numericValue.EndsWith(" mmol/L"))
        {
            // Reapply the unit after removing it
            bloodGlucoseInput.text = numericValue + " mmol/L";
            bloodGlucoseInput.caretPosition = bloodGlucoseInput.text.Length; // Keep the cursor at the end
        }
    }

    // Live update for carb intake input (add "g" as user types, remove if it's already there)
    private void UpdateCarbIntakeInput(string value)
    {
        // Remove any existing units and leave only the numeric value
        string numericValue = value.Replace(" g", "").Trim();

        // Only append the unit if the user hasn't typed it already
        if (!numericValue.EndsWith(" g"))
        {
            // Reapply the unit after removing it
            carbIntakeInput.text = numericValue + " g";
            carbIntakeInput.caretPosition = carbIntakeInput.text.Length; // Keep the cursor at the end
        }
    }

    // Save the input values and current date to PlayerPrefs
    public void SaveData()
    {
        // Strip units and validate the input
        string bloodGlucoseText = bloodGlucoseInput.text.Replace(" mmol/L", "").Trim();
        string carbIntakeText = carbIntakeInput.text.Replace(" g", "").Trim();

        if (float.TryParse(bloodGlucoseText, out float bloodGlucose) &&
            float.TryParse(carbIntakeText, out float carbIntake))
        {
            // Save the numeric values
            PlayerPrefs.SetFloat(BloodGlucoseKey, bloodGlucose);
            PlayerPrefs.SetFloat(CarbIntakeKey, carbIntake);

            // Save the current date and time in 24-hour European format
            string currentDate = DateTime.Now.ToString("MMMM d, 'at' HH:mm"); // Example: "January 28, at 15:19"
            PlayerPrefs.SetString(LastUpdatedKey, currentDate);

            PlayerPrefs.Save();

            // Update the last updated text
            lastUpdatedText.text = "Last updated on " + currentDate;

            Debug.Log("Data saved successfully!");
        }
        else
        {
            Debug.LogWarning("Invalid input! Please enter valid numeric values.");
        }
    }

    // Load the saved values and display them in the input fields (without parsing)
    public void LoadData()
    {
        if (PlayerPrefs.HasKey(BloodGlucoseKey) && PlayerPrefs.HasKey(CarbIntakeKey) && PlayerPrefs.HasKey(LastUpdatedKey))
        {
            // Retrieve saved values
            float bloodGlucose = PlayerPrefs.GetFloat(BloodGlucoseKey);
            float carbIntake = PlayerPrefs.GetFloat(CarbIntakeKey);
            string lastUpdated = PlayerPrefs.GetString(LastUpdatedKey);

            // Display values with units in the input fields
            bloodGlucoseInput.text = $"{bloodGlucose:F1} mmol/L"; // Blood glucose in mmol/L
            carbIntakeInput.text = $"{carbIntake:F1} g";          // Carb intake in grams

            // Display the last updated date
            lastUpdatedText.text = "Last updated on " + lastUpdated;

            Debug.Log("Data loaded successfully!");
        }
        else
        {
            Debug.LogWarning("No data found! Please save data first.");
            lastUpdatedText.text = "No data available.";
        }
    }
}
