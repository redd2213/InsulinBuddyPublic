using UnityEngine;
using TMPro;

public class Calculator : MonoBehaviour
{
    // Input fields for user input
    public TMP_InputField glucoseInput;
    public TMP_InputField carbsInput;
    public TMP_InputField targetGlucoseInput;
    public TMP_InputField carbRatioInput;
    public TMP_InputField sensitivityFactorInput;

    // Text element to display the result
    public TMP_Text resultText;

    private const string GlucoseUnit = " mmol/L";
    private const string CarbsUnit = " g";
    private const string TargetGlucoseUnit = " mmol/L";
    private const string CarbRatioUnit = " g/unit";
    private const string SensitivityFactorUnit = " mmol/L/unit";

    private void Start()
    {
        // Set up event listeners for appending units dynamically
        glucoseInput.onValueChanged.AddListener((value) => AppendUnit(glucoseInput, GlucoseUnit));
        carbsInput.onValueChanged.AddListener((value) => AppendUnit(carbsInput, CarbsUnit));
        targetGlucoseInput.onValueChanged.AddListener((value) => AppendUnit(targetGlucoseInput, TargetGlucoseUnit));
        carbRatioInput.onValueChanged.AddListener((value) => AppendUnit(carbRatioInput, CarbRatioUnit));
        sensitivityFactorInput.onValueChanged.AddListener((value) => AppendUnit(sensitivityFactorInput, SensitivityFactorUnit));
    }

    private void AppendUnit(TMP_InputField inputField, string unit)
    {
        if (inputField.text.EndsWith(unit)) return; // Prevent adding the unit multiple times

        string textWithoutUnit = inputField.text.Replace(unit, "").Trim(); // Remove any existing unit
        inputField.text = textWithoutUnit + unit; // Append the unit
        inputField.caretPosition = textWithoutUnit.Length; // Set caret position
    }

    // Method to calculate the insulin dose
    public void CalculateInsulinDose()
    {
        // Validate inputs and parse values
        if (float.TryParse(RemoveUnit(glucoseInput.text, GlucoseUnit), out float glucose) &&
            float.TryParse(RemoveUnit(carbsInput.text, CarbsUnit), out float carbs) &&
            float.TryParse(RemoveUnit(targetGlucoseInput.text, TargetGlucoseUnit), out float targetGlucose) &&
            float.TryParse(RemoveUnit(carbRatioInput.text, CarbRatioUnit), out float carbRatio) &&
            float.TryParse(RemoveUnit(sensitivityFactorInput.text, SensitivityFactorUnit), out float sensitivityFactor))
        {
            // Calculate correction and meal doses
            float correctionDose = (glucose - targetGlucose) / sensitivityFactor;
            float mealDose = carbs / carbRatio;
            float totalDose = correctionDose + mealDose;

            // Display the result
            resultText.text = $"Recommended Dose: {totalDose:F2} units";
            Debug.Log($"Result displayed: {resultText.text}"); // Debug to confirm text is updated.
        }
        else
        {
            // Display error message if inputs are invalid
            resultText.text = "Invalid input. Please check all fields.";
            Debug.Log("Invalid input."); // Debug to confirm invalid input handling.
        }
    }

    private string RemoveUnit(string input, string unit)
    {
        return input.Replace(unit, "").Trim();
    }
}
