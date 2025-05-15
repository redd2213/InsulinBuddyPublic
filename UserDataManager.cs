using UnityEngine;
using TMPro;

public class UserDataManager : MonoBehaviour
{
    public TMP_InputField usernameInput; // Input field for username
    public TMP_InputField ageInput;      // Input field for age
    public TMP_InputField genderInput;   // Input field for gender
    public TextMeshProUGUI feedbackText; // Text for feedback messages

    // Save user data
    public void SaveUserData()
    {
        string username = usernameInput.text;
        string age = ageInput.text;
        string gender = genderInput.text;

        // Save data using PlayerPrefs
        PlayerPrefs.SetString("Username", username);
        PlayerPrefs.SetString("Age", age);
        PlayerPrefs.SetString("Gender", gender);
        PlayerPrefs.Save();

        feedbackText.text = "User data saved!";
    }

    // Load user data
    public void LoadUserData()
    {
        string username = PlayerPrefs.GetString("Username", "No Username");
        string age = PlayerPrefs.GetString("Age", "No Age");
        string gender = PlayerPrefs.GetString("Gender", "No Gender");

        // Display the loaded data in the input fields
        usernameInput.text = username;
        ageInput.text = age;
        genderInput.text = gender;

        feedbackText.text = "User data loaded!";
    }
}
